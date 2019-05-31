using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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
                using (var gateway = new GatewayHost(IpfsEngine))
                {
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            finally
            {
                IpfsEngine.Stop();
            }
        }

    }
}
