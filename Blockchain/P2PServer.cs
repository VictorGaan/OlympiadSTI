using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Blockchain
{
    public class P2PServer : WebSocketBehavior
    {
        WebSocketServer wss = null;
        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:8080");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(File.ReadAllText(Helper.DefaultFilePath));
            Send(JsonConvert.SerializeObject(newChain));
            File.WriteAllText(Helper.DefaultFilePath, JsonConvert.SerializeObject(newChain));
        }
        public void Close()
        {
            wss.Stop();
        }
    }
}
