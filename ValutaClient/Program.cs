// See https://aka.ms/new-console-template for more information

using Helpers.Messages;
using MessageClient;
using Microsoft.Identity.Client;
using System.Diagnostics;
using ValutaClient;
using ValutaClient.DB;

namespace Program
{
    public class Program
    {
        
        public static async Task Main(string[] args) 
        {
            var valutaService = ValutaServiceFactory.CreateValutaService("Valuta");
            valutaService.Start();
            //var a = await ExchangeRateProvider.GetAllCurrencyLiveRatesAsync("EUR");

            //Database db = new Database();
            //Database.SaveData(a);
            Database.LoadData("DKK");

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



