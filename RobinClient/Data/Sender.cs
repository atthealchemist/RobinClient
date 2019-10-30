using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebSocket4Net;

namespace RobinClient.utils
{
    public class Sender
    {
        private WebSocket client;

        public Action<Query> OnResult;

        public Sender(string url)
        {
            client = new WebSocket(url);

            client.Opened += (sender, e) => Console.WriteLine("ws opened");
            client.Error += (sender, e) => Console.WriteLine($"ws error: {e.Exception}");
            client.Closed += (sender, e) => Console.WriteLine("ws closed");
            client.MessageReceived += (sender, e) =>
            {
                Console.WriteLine($"ws got: {e.Message}");
                var response = JsonConvert.DeserializeObject<Query>(e.Message);
                OnResult(response);
            };
        }

        public void Connect()
        {
            if (client.State == WebSocketState.Closed || client.State == WebSocketState.None)
            {
                client.Open();
            }
        }

        public void Send(string value)
        {
            var query = new Query
            {
                Number = decimal.Parse(value, CultureInfo.InvariantCulture),
                Status = "Idle"
            };
            var json = JsonConvert.SerializeObject(query);
            client.Send(json);
        }
    }
}
