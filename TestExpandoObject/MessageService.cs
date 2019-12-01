using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    public class MessageService
    {
        private readonly IMessageBus _messageBus;

        public MessageService(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void SendRawJson<T>(string json)
        {
            var message = JsonConvert.DeserializeObject<T>(json) as dynamic;

            _messageBus.Send(message);
        }
    }
}
