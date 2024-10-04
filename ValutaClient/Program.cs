// See https://aka.ms/new-console-template for more information

using Microsoft.Identity.Client;
using System.Diagnostics;
using ValutaClient.Database;

namespace Program
{
    public class Program
    {
        public static async Task Main(string[] args) 
        {

            //var a = await ExchangeRateProvider.GetAllCurrencyLiveRatesAsync("EUR");

            Database db = new Database();
            //db.SaveData(a);
            db.LoadData("DKK");

            var b = 1;

        }
        
    }
    

}



