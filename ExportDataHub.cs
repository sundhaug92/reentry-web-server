using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        FileSystemWatcher watcher;
        public const string HubUrl = "/ExportData";

        public ExportDataHub(IHubContext<ExportDataHub> hubContext)
        {
            _hubContext = hubContext;

            watcher = new FileSystemWatcher();
            watcher.Path = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "Low\\Wilhelmsen Studios\\ReEntry\\Export\\";
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;
            watcher.Filter = "*.json";
            watcher.IncludeSubdirectories = true;
            watcher.Changed += OnFileChanged;
            watcher.Created += OnFileChanged;
            watcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            var basePath = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "Low\\Wilhelmsen Studios\\ReEntry\\Export\\";
            var dictObj = new Dictionary<string, object>();
            if(e.FullPath.EndsWith("outputOBC.json"))
            {
                try
                    {
                        var obc = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(basePath + "Gemini\\outputOBC.json"));
                        dictObj["OBC"] = obc;
                    }
                    catch(IOException){
                        dictObj["OBC"] = null;
                    }
                _hubContext.Clients.All.SendAsync("ExportData", JsonSerializer.Serialize(dictObj));
            }
            else if(e.FullPath.EndsWith("outputAGC.json"))
            {
                try
                    {
                        var agc = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(basePath + "Apollo\\outputAGC.json"));
                        dictObj["CSM_AGC"] = agc;
                    }
                    catch(IOException){
                        dictObj["CSM_AGC"] = null;
                    }
                _hubContext.Clients.All.SendAsync("ExportData", JsonSerializer.Serialize(dictObj));
            }
            else if(e.FullPath.EndsWith("outputAGC.json"))
            {
                try
                    {
                        var agc = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(basePath + "Apollo\\outputLGC.json"));
                        dictObj["LM_AGC"] = agc;
                    }
                    catch(IOException){
                        dictObj["LM_AGC"] = null;
                    }
                _hubContext.Clients.All.SendAsync("ExportData", JsonSerializer.Serialize(dictObj));
            }
        }

        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        public override Task OnConnectedAsync()
        {
            Debug.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Debug.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}