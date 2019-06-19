using Ipfs.CoreApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ipfs.HttpGateway
{
    [TestClass]
    public class GatewayHostTest
    {
        GatewayHost gateway = TestFixture.Gateway;
        ICoreApi ipfs = TestFixture.Ipfs;

        [TestMethod]
        public void Should_start_and_stop()
        {
            for (int i = 0; i < 3; ++i)
            {
                var g = new GatewayHost(ipfs, "http://127.0.0.1:0");
                g.Dispose();
            }
        }

        [TestMethod]
        public async Task Should_have_a_URL_for_content()
        {
            var node = await ipfs.FileSystem.AddTextAsync("hello");
            var url = gateway.IpfsUrl(node.Id);
            StringAssert.StartsWith(url, "http");
        }

        [TestMethod]
        public async Task Should_serve_the_content()
        {
            const string text = "good afternoon from IPFS!";
            var node = await ipfs.FileSystem.AddTextAsync(text);
            var url = gateway.IpfsUrl(node.Id);

            var httpClient = new HttpClient();

            var content = await httpClient.GetStringAsync(url);
            Assert.AreEqual(text, content);
        }

    }
}
