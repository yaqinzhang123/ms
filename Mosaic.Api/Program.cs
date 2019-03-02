using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mosaic.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {    
                BuildWebHost(args).Run();
        }
            public static IWebHost BuildWebHost(string[] args) =>
               WebHost.CreateDefaultBuilder(args)
              .ConfigureLogging((hostingContext,logging)=> 
              {
                  logging.AddFilter("System", LogLevel.Warning);
                  logging.AddFilter("Microsoft", LogLevel.Warning);
                  logging.AddLog4Net();
              })
              .UseStartup<Startup>()
              .UseUrls("http://*:1460")
              .Build();
    }
    }

