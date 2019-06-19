using Ipfs.CoreApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
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

        [TestMethod]
        public async Task Should_serve_website()
        {
            const string html = "<p>good afternoon from IPFS!</p>";
            var options = new AddFileOptions
            {
                Wrap = true
            };
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(html));
            var node = await ipfs.FileSystem.AddAsync(ms, "index.html", options);
            var url = gateway.IpfsUrl(node.Id);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(url);
            Assert.AreEqual(html, content);
        }

        [TestMethod]
        public async Task Should_browse_directory_without_index()
        {
            const string html = "<p>good afternoon from IPFS!</p>";
            var options = new AddFileOptions
            {
                Wrap = true
            };
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(html));
            var node = await ipfs.FileSystem.AddAsync(ms, "foo.html", options);
            var url = gateway.IpfsUrl(node.Id);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(url);
            StringAssert.Contains(content, "<html>");
            StringAssert.Contains(content, node.Id.Encode());
            StringAssert.Contains(content, "foo.html");
        }
    }
}
