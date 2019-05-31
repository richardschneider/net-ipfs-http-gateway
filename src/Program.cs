using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ipfs.Engine;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ipfs.HttpGateway
{
    public class Program
    {
        /// <summary>
        ///   The IPFS Core API engine.
        /// </summary>
        public static IpfsEngine IpfsEngine;
        const string passphrase = "this is not a secure pass phrase";

        public static void Main(string[] args)
        {
            IpfsEngine = new IpfsEngine(passphrase.ToCharArray());
            IpfsEngine.Start();

            try
            {
                CreateWebHostBuilder(args)
                    .Build()
                    .Run();
            }
            finally
            {
                IpfsEngine?.Stop();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var url = "http://127.0.0.1:8080";

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(url)
                .UseStartup<Startup>();
        }
    }
}
