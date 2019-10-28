using System;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace RobinClient.utils
{
    public class Sender
    {
        private WebSocket client;

        public Action<string> OnResult;

        private string url;

        public Sender(string url)
        {
            this.url = url;
            client = new WebSocket(this.url);
            client.Log.Level = LogLevel.Fatal;
        }

        public void Send(string value)
        {
            client.OnOpen += (_, e) => Console.WriteLine("ws opened");
            client.OnMessage += (_, e) => OnResult(e.Data);
            client.OnClose += (_, e) =>
            {
                Task.Run(() =>
                {
                    while (!client.IsAlive)
                    {
                        Task.Delay(1000).Wait();
                        client.Connect();
                    }
                });
            };

            client.Connect();
            if (client.IsAlive)
            {
                client.Send(value);
            }
        }
    }
}
