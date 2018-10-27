using Shell.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestCommands
    {
        Router router = Routing<ToolModule>.Router;

        [TestMethod]
        public void TestSectionDefault()
        {
            var arguments = Utils.ParseArguments("tool");
            var result = router.Bind(arguments);

            var route = result.Candindates.First();
            Assert.AreEqual(route.Method.Name, "Tool");
        }

        [TestMethod]
        public void SingleLiteral()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action Foo");
            var result = router.Bind(arguments);
            
            Assert.AreEqual(result.Candindates.Count, 2);
            Assert.AreEqual(result.Count, 1);

            var bind = result.Bind;
            Assert.AreEqual(bind.Endpoint.Method.Name, "Action");

            var routingparams = bind.Endpoint.Method.GetRoutingParameters();
            Assert.AreEqual(routingparams.Count(), 1);

            Assert.AreEqual(bind.Arguments[0], "Foo");
        }

    
        [TestMethod]
        public void Binding()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action William will --foo --bar fubar");
            var result = router.Bind(arguments);
            
            Assert.AreEqual(result.Candindates.Count, 2);
                // action(name) 
                // action(name, alias, foo, bar)

            Assert.AreEqual(result.Count, 1);
            // action(name, alias, foo, bar)

            var bind = result.Bind;
            Assert.AreEqual(bind.Endpoint.Method.Name, "Action");

            var routingparams = bind.Endpoint.Method.GetRoutingParameters();
            Assert.AreEqual(routingparams.Count(), 4);

            Assert.AreEqual(bind.Arguments[0], "William");
            Assert.AreEqual(bind.Arguments[1], "will");
            Assert.AreEqual(((Flag)bind.Arguments[2]).Set, true); // -foo

            Assert.AreEqual("fubar", ((FlagValue)bind.Arguments[3])); // -bar fubar
        }

        [TestMethod]
        public void TestOptionValue()
        {
            var args1 = Utils.ParseArguments("-a -b --test abc");
            args1.TryGet("test", out Flag option1);
            Assert.AreEqual("test",  option1.Name);
            args1.TryGetFlagValue("test", out var value);
            Assert.AreEqual("abc", value);
        }

        [TestMethod]
        public void TestOpionValues()
        {
            var line = "save --all --pattern '{id}-{id}'";
            var args = Utils.ParseArguments(line);
            var result = router.Bind(args);
            Assert.AreEqual(1, result.Count);

        }

        [TestMethod]
        public void Nesting()
        {
            var line = "main sub detail hello";
            var args = Utils.ParseArguments(line);
            var result = router.Bind(args);
            Assert.AreEqual(1, result.Count);
        }
    }
}
