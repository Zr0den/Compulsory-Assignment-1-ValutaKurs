// See https://aka.ms/new-console-template for more information

using Helpers.Messages;
using MessageClient;
using Microsoft.Identity.Client;
using System.Diagnostics;
using ValutaClient;

namespace Program
{
    public class Program
    {
        
        public static async Task Main(string[] args) 
        {
            
            //var a = await ExchangeRateProvider.GetAllCurrencyLiveRatesAsync("EUR");

            //Database db = new Database();
            //db.SaveData(a);
            //db.LoadData("DKK");

            var b = 1;

            var running = true;
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                Console.WriteLine("Exiting...");
                running = false;
            };

            while (running)
            {
            }

        }

        
    }
    

}



