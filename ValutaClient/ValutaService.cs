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
            Action<ValutaRequestMessage> callback = HandleNewValuta;
            _newValutaClient.Connect();
            _newValutaClient.ListenUsingTopic(callback, "", "newValuta");
        }
        
        private void HandleNewValuta(ValutaRequestMessage valuta)
        {

        }

    }
}
