using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;

namespace reentry_web_server
{
    class ExportDataHub : Hub
    {
        IHubContext<ExportDataHub> _hubContext = null;
        private Timer eventTimer;
        public const string HubUrl = "/ExportData";

        public ExportDataHub(IHubContext<ExportDataHub> hubContext)
        {
            _hubContext = hubContext;
            eventTimer = new Timer(1000.0f / 60f);
            eventTimer.Elapsed += delegate (object source, ElapsedEventArgs e)
            {
                var basePath = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "Low\\Wilhelmsen Studios\\ReEntry\\Export\\";

                var dictObj = new Dictionary<string, object>();
                // OBC
                if (File.Exists(basePath + "Gemini\\outputOBC.json"))
                {
                    try
                    {
                        var obc = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(basePath + "Gemini\\outputOBC.json"));
                        dictObj["OBC"] = obc;
                    }
                    catch(IOException){
                        dictObj["OBC"] = null;
                    }
                }
                else
                    dictObj["OBC"] = null;
                _hubContext.Clients.All.SendAsync("ExportData", JsonSerializer.Serialize(dictObj));
            };
            eventTimer.Enabled = true;
        }
        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}