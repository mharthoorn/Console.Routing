using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class PocoTests
    {
        Router router = new RouterBuilder().AddAssemblyOf<PocoTests>().Build();

        [TestMethod]
        public void PocoFields()
        {
            var args = router.Parse("curl --crlf --append --url http://hello.world");
            var result = router.Bind(args);
            
            Assert.IsTrue(result.Ok);
            Assert.AreEqual("Curl", result.Bind.Route.Method.Name);

            if (result.Bind.Parameters[0] is CurlSettings settings)
            {  
                Assert.AreEqual(true, settings.CrlF);
                Assert.AreEqual(true, settings.Append);
                Assert.AreEqual(true, settings.Url.IsSet);
                Assert.AreEqual("http://hello.world", settings.Url.Value);

                Assert.AreEqual(false, settings.UseAscii);
                Assert.AreEqual(false, settings.Basic);
                Assert.AreEqual(false, settings.RemoteName.IsSet);

                Assert.AreEqual(false, settings.UnusedBool);
                Assert.AreEqual(false, settings.UnusedFlag.IsSet);
            }
            else Assert.Fail();
        }

        [TestMethod]
        public void PocoProperties() 
        {
            var args = router.Parse("curl --use-ascii --basic --remotename server");
            var result = router.Bind(args);

            Assert.IsTrue(result.Ok);
            Assert.AreEqual("Curl", result.Bind.Route.Method.Name);

            if (result.Bind.Parameters[0] is CurlSettings settings)
            {
                Assert.AreEqual(true, settings.UseAscii);
                Assert.AreEqual(true, settings.Basic);
                Assert.AreEqual(true, settings.RemoteName.IsSet);
                Assert.AreEqual("server", settings.RemoteName.Value);

                Assert.AreEqual(false, settings.CrlF);
                Assert.AreEqual(false, settings.Append);
                Assert.AreEqual(false, settings.Url.IsSet);
                
                Assert.AreEqual(false, settings.UnusedBool);
                Assert.AreEqual(false, settings.UnusedFlag.IsSet);
            }
            else Assert.Fail();
        }
         
    }
}
