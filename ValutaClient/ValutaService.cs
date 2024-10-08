using Helpers;
using Helpers.Messages;
using MessageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValutaClient.Core.Requests;

namespace ValutaClient
{
    public class ValutaService
    {
        private readonly MessageClient<ValutaRequestMessage> _newValutaClient;
        private readonly MessageClient<ValutaResponseMessage> _valutaChangedClient;
        private readonly ValutaCRUD _valutaCRUD;

        public ValutaService(MessageClient<ValutaRequestMessage> newValutaClient, MessageClient<ValutaResponseMessage> valutaChangedClient, ValutaCRUD valutaCRUD)
        {
            _newValutaClient = newValutaClient;
            _valutaChangedClient = valutaChangedClient;
            _valutaCRUD = valutaCRUD;
        }

        public void Start()
        {
            Action<ValutaRequestMessage> callback = CalculateExchangeRate;
            _newValutaClient.Connect();
            _newValutaClient.ListenUsingTopic(callback, "", "newValuta");
        }

        public async void CalculateExchangeRate(ValutaRequestMessage msg)
        {
            using var activity = Monitoring.ActivitySource.StartActivity();

            ExchangeRate? exchangeRateFrom = await DB.Database.LoadData(msg.FromCurrencyCode);
            ExchangeRate? exchangeRateTo;
            if (exchangeRateFrom?.Timestamp < DateTime.Now.AddHours(-1))
            {
                //Data more than 1 hour old - get new data
                IList<ExchangeRate> data = await ExchangeRateProvider.GetAllCurrencyLiveRatesAsync();  
                exchangeRateFrom = data.Where(x => x.CurrencyCode == msg.FromCurrencyCode).FirstOrDefault();
                exchangeRateTo = data.Where(x => x.CurrencyCode == msg.ToCurrencyCode).FirstOrDefault();

                await DB.Database.SaveData(data);
            }
            else
            {
                exchangeRateTo = await DB.Database.LoadData(msg.ToCurrencyCode);
            }

            decimal rate = Math.Round(exchangeRateTo.Value / exchangeRateFrom.Value, 5);
            ValutaResponseMessage response = new ValutaResponseMessage()
            {
                FromCurrencyCode = exchangeRateFrom.CurrencyCode,
                ToCurrencyCode = exchangeRateTo.CurrencyCode,
                Value = msg.Value,
                Rate = rate
            };

            _valutaChangedClient.SendUsingTopic<ValutaResponseMessage>(response, "Rate Calculated");
        }

        public async void GetSupportedCurrencyCodes()
        {
            using var activity = Monitoring.ActivitySource.StartActivity();

            StringBuilder sb = new StringBuilder();
            List<ExchangeRate> data = await DB.Database.GetSupportedCurrencies();
            foreach (var item in data)
            {
                sb.Append(item.CurrencyCode);
                sb.Append(", ");
            }
            sb.Length--;

            ValutaResponseMessage response = new ValutaResponseMessage()
            {
                FromCurrencyCode = sb.ToString()
            };

            _valutaChangedClient.SendUsingTopic<ValutaResponseMessage>(response, "Supported Currencies");
        }
    }
}
