using Helpers.Messages;
using MessageClient.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValutaClient.Core.Repositories;
using ValutaClient.Core.Requests;

namespace ValutaClient
{
    public class ValutaServiceFactory
    {
        public static ValutaService CreateValutaService()
        {
            var easyNetQFactory = new EasyNetQFactory();
            var newValutaClient = easyNetQFactory.CreateTopicMessageClient<ValutaRequestMessage>("ValutaService", "newValuta");
            var valutaChangedClient = easyNetQFactory.CreatePubSubMessageClient<ValutaResponseMessage>("");

            var dataContext = new DB.Database();
            var valutaRepository = new ValutaRepository(dataContext);
            var valutaCRUD = new ValutaCRUD(valutaRepository);
            //var valutaResponseMapper = new Core.Mappers.ValutaResponseMapper();

            return new ValutaService(
               newValutaClient,
               valutaChangedClient,
               valutaCRUD
               );
        }
    }
}
