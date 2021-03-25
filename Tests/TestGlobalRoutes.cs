using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestGlobalRoutes
    {
        Router router = new RouterBuilder()
            .AddAssemblyOf<GlobalCommands>()
            .Build();

        [TestMethod]
        public void GlobalRoute()
        {
            var args = router.Parse("ding dong bar bor");

            var result = router.Bind(args);

            Assert.AreEqual("Foo", result.Route.Method.Name);
        }

    }

    
    [Module]
    public class GlobalCommands
    {
        [Command, Capture("bar"), Hidden]
        public void Foo(Arguments arguments)
        {
            
        }
    }
}
