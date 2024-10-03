// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

namespace Program
{
    public class Program
    {
        public static async Task Main(string[] args) 
        {
            var a = await ExchangeRateProvider.GetAllCurrencyLiveRatesAsync("DKK");
            var b = 1;
        }
        
    }
    

}



