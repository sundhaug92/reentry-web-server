using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace reentry_web_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var urls = new string[] {};
            var localhostOnly = args.All((s) => s != "everyone");
            if (!localhostOnly)
            {
                Console.WriteLine("Listening on all interfaces");
                urls.Append("http://*:5000");
            }
            else
            {
                Console.WriteLine("Listening only on localhost");
                urls.Append("http://localhost:5000");
            }
            CreateHostBuilder(args, urls).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string[] urls) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseUrls(urls);
                });
    }
}
