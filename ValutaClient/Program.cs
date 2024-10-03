// See https://aka.ms/new-console-template for more information

using Helpers;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry;
using System.Diagnostics;

namespace Program
{
    public class Program
    {
        public static async Task Main(string[] args) 
        {
            var connectionEstablished = false;

            Thread.Sleep(1000);
            using var activity = Monitoring.ActivitySource.StartActivity();

            using var bus = ConnectionHelper.GetRMQConnection();
            //var request = new ServiceBRequest(); TODO
            var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
            var propagationContext = new PropagationContext(activityContext, Baggage.Current);
            var propagator = new TraceContextPropagator();
            //propagator.Inject(propagationContext, request-TODO-, (r, key, value) =>
            //{
            //    r.Header.Add(key, value);
            //});
            var a = await ExchangeRateProvider.GetAllCurrencyLiveRatesAsync("DKK");
            var b = 1;
        }
        
    }
    

}



