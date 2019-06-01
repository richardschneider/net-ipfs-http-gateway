using Ipfs.CoreApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.HttpGateway
{
    /// <summary>
    ///   Acts as a bridge between traditional web browsers and IPFS. Through the gateway,
    ///   users can browse files and websites stored in IPFS as if they were stored
    ///   in a traditional web server.
    /// </summary>
    public class GatewayHost : IDisposable
    {
        ICoreApi ipfs;
        string listeningUrl;
        bool disposedValue;
        Thread thread;
        CancellationTokenSource cancel = new CancellationTokenSource();

        /// <summary>
        ///   Creates a web host that bridges IPFS and HTTP on "http://127.0.0.1:8080"
        /// </summary>
        /// <param name="ipfs">
        ///   The IPFS core features.
        /// </param>
        /// <remarks>
        ///   This starts the web host on a separate thread.  Use the Dispose method to
        ///   stop the web host.
        /// </remarks>
        public GatewayHost(ICoreApi ipfs)
            : this(ipfs, "http://127.0.0.1:8080")
        {
        }

        /// <summary>
        ///   Creates a web host that bridges IPFS and HTTP on the specified URL.
        /// </summary>
        /// <param name="ipfs">
        ///   The IPFS core features.
        /// </param>
        /// <param name="url">
        ///   The url to listen on, typically something like "http://127.0.0.1:8080".
        /// </param>
        /// <remarks>
        ///   This starts the web host on a separate thread.  Use the Dispose method to
        ///   stop the web host.
        /// </remarks>
        public GatewayHost(ICoreApi ipfs, string url)
        {
            this.ipfs = ipfs;
            this.listeningUrl = url;
            thread = new Thread(Runner)
            {
                IsBackground = true
            };
            thread.Start();
        }

        /// <summary>
        ///   The web host thread.
        /// </summary>
        void Runner()
        {
            try
            {
                var host = WebHost.CreateDefaultBuilder()
                    .UseUrls(listeningUrl)
                    .Configure(app =>
                    {
                        app.UseStaticFiles();
                        app.UseMvc();
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<ICoreApi>(ipfs);
                        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                    })
                    .Build();
                host.RunAsync(cancel.Token).Wait();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cancel.Cancel();
                }
                disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
