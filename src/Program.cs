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
    /// <summary>
    ///    Runs the gateway as a separate process.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///   The IPFS Core API engine.
        /// </summary>
        static IpfsEngine IpfsEngine;
        const string passphrase = "this is not a secure pass phrase";

        /// <summary>
        ///   The main entry point of the program.
        /// </summary>
        /// <param name="args">TODO</param>
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
