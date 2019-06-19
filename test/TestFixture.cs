using Ipfs.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Common.Logging;
using Common.Logging.XSimple;

namespace Ipfs.HttpGateway
{
    [TestClass]
    public class TestFixture
    {
        const string passphrase = "this is not a secure pass phrase";
        public static IpfsEngine Ipfs = new IpfsEngine(passphrase.ToCharArray());
        public static GatewayHost Gateway;

        static TestFixture()
        {
            Ipfs.Options.Repository.Folder = Path.Combine(Path.GetTempPath(), "ipfs-test");
            Ipfs.Options.KeyChain.DefaultKeySize = 512;
            Ipfs.Config.SetAsync(
                "Addresses.Swarm",
                JToken.FromObject(new string[] { "/ip4/0.0.0.0/tcp/0" })
            ).Wait();
            Ipfs.Start();

            Gateway = new GatewayHost(Ipfs, "http://127.0.0.1:0");
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // set logger factory
            var properties = new Common.Logging.Configuration.NameValueCollection
            {
                ["level"] = "DEBUG",
                ["showLogName"] = "true",
                ["showDateTime"] = "true",
                ["dateTimeFormat"] = "HH:mm:ss.fff"

            };
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(properties);
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Gateway?.Dispose();
            Ipfs?.Stop();

            if (Directory.Exists(Ipfs.Options.Repository.Folder))
            {
                Directory.Delete(Ipfs.Options.Repository.Folder, true);
            }
        }
    }
}
