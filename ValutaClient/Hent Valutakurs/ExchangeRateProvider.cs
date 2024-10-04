using Serilog;
using System.Globalization;
using System.Xml;

public static class ExchangeRateProvider
{

    #region Methods

    //Gets the rates to EUROS. For example, DKK (as of the time of writing this comment) will have the Value of ~7.4955
    public static async Task<IList<ExchangeRate>> GetAllCurrencyLiveRatesAsync()
    {
        //add euro with rate 1
        var ratesToEuro = new List<ExchangeRate>
        {
            new ExchangeRate
            {
                CurrencyCode = "EUR",
                Value = 1,
                Timestamp = DateTime.UtcNow
            }
        };

        //get exchange rates to euro from European Central Bank
        try
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");

            //load XML document
            var document = new XmlDocument();
            document.Load(stream);

            //add namespaces
            var namespaces = new XmlNamespaceManager(document.NameTable);
            namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
            namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

            //get daily rates
            var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
            if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out var updateDate))
            {
                updateDate = DateTime.UtcNow;
            }

            foreach (XmlNode currency in dailyRates.ChildNodes)
            {
                //get rate
                if (!decimal.TryParse(currency.Attributes["rate"].Value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var currencyRate))
                {
                    continue;
                }

                ratesToEuro.Add(new ExchangeRate()
                {
                    CurrencyCode = currency.Attributes["currency"].Value,
                    Value = currencyRate,
                    Timestamp = updateDate
                });
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"GetCurrencyLiveRatesAsync: {ex.Message}");
        }

        return ratesToEuro;

        ////use only currencies that are supported by ECB
        //var exchangeRateCurrency = ratesToEuro.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
        //if (exchangeRateCurrency == null)
        //{
        //    throw new Exception("You can use ECB (European Central Bank) exchange rate provider only when the primary exchange rate currency is supported by ECB.");
        //}

        ////return result for the selected (not euro) currency
        //return ratesToEuro.Select(rate => new ExchangeRate
        //{
        //    CurrencyCode = rate.CurrencyCode,
        //    Value = Math.Round(rate.Value / exchangeRateCurrency.Value, 4),
        //    Timestamp = rate.Timestamp
        //}).ToList();
    }

    #endregion
}