using Ipfs.CoreApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
        IWebHost host;
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
            
            // Build the web host.
            host = WebHost.CreateDefaultBuilder()
                .UseUrls(listeningUrl)
                .Configure(app =>
                {
                    app.UseDeveloperExceptionPage();
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new ManifestEmbeddedFileProvider(this.GetType().Assembly, "wwwroot")
                    });
                    app.UseMvc();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ICoreApi>(ipfs);
                    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                })
                .Build();

            var thread = new Thread(Runner)
            {
                IsBackground = true
            };

            thread.Start();
        }

        /// <summary>
        ///   Gets the url to the IPFS path.
        /// </summary>
        /// <param name="path">
        ///   The path to an IPFS file or directory.  For example,
        ///   "Qmhash" or "Qmhash/this/and/that".
        /// </param>
        /// <returns>
        ///   The fully qualified URL to the IPFS <paramref name="path"/>.  For example,
        ///   "http://127.0.0.1:8080/ipfs/Qmhash".
        /// </returns>
        public string IpfsUrl(string path)
        {
            if (path.StartsWith('/'))
            {
                path = path.Substring(1);
            }

            return $"{listeningUrl}/ipfs/{path}";
        }

        /// <summary>
        ///   The web host thread.
        /// </summary>
        void Runner()
        {
            try
            {
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
