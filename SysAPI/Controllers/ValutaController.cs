using Helpers.Messages;
using MessageClient;
using Microsoft.AspNetCore.Mvc;

namespace SysAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValutaController : ControllerBase
    {
        private readonly MessageClient<ValutaResponseMessage> _valutaResponseMessageClient;
        private readonly MessageClient<ValutaRequestMessage> _valutaRequestMessageClient;

        public ValutaController(MessageClient<ValutaResponseMessage> valutaResponseMessageClient, MessageClient<ValutaRequestMessage> valutaRequestMessageClient)
        {
            _valutaResponseMessageClient = valutaResponseMessageClient;
            _valutaRequestMessageClient = valutaRequestMessageClient;
        }

        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        //private readonly ILogger<ValutaController> _logger;

        //public ValutaController(ILogger<ValutaController> logger)
        //{
        //    _logger = logger;
        //}

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<ValutaItems> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new ValutaItems
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        internal static class MessageWaiter
        {
            public static async Task<T?>? WaitForMessage<T>(MessageClient<T> messageClient, string clientId, int timeout = 5000)
            {
                var tcs = new TaskCompletionSource<T?>();
                var cancellationTokenSource = new CancellationTokenSource(timeout);
                cancellationTokenSource.Token.Register(() => tcs.TrySetResult(default));

                using (
                    var connection = messageClient.ListenUsingTopic<T>(message =>
                    {
                        tcs.TrySetResult(message);
                    }, "customer" + clientId, clientId)
                )
                {
                }

                return await tcs.Task;
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostValutaExchange(ValutaItems valuta)
        {
            _valutaRequestMessageClient.SendUsingTopic(new ValutaRequestMessage
            {
                ValutaId = valuta.ValutaId,
                Status = valuta.Status,
                ToCurrencyCode = valuta.ToCurrencyCode,
                FromCurrencyCode = valuta.FromCurrencyCode,
                Value = valuta.Value,
            }, "newValuta");

            var response = await MessageWaiter.WaitForMessage(_valutaResponseMessageClient, valuta.ValutaId.ToString())!;

            return response != null ? response.Status : "Transaction timed out";

        }


    }
}
