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
            router.ConsumeCommands(arguments, out var routes);
            var route = routes.First();
            Assert.AreEqual(route.Method.Name, "Tool");
        }

        [TestMethod]
        public void SingleLiteral()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action Foo");

            router.ConsumeCommands(arguments, out var routes);
            Assert.AreEqual(routes.Count(), 2);

            var binds = router.Bind(routes, arguments).ToList();
            Assert.AreEqual(binds.Count, 1);

            var bind = binds.First();
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var routingparams = bind.Route.GetRoutingParameters();
            Assert.AreEqual(routingparams.Count(), 1);

            Assert.AreEqual(bind.Arguments[0], "Foo");
        }

    
        [TestMethod]
        public void Binding()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action William will --foo --bar fubar");
             
            router.ConsumeCommands(arguments, out var routes);
            Assert.AreEqual(routes.Count(), 2);
                // action(name) 
                // action(name, alias, foo, bar)

            var binds = router.Bind(routes, arguments).ToList();
            Assert.AreEqual(binds.Count, 1);
                // action(name, alias, foo, bar)

            var bind = binds.First();
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var routingparams = bind.Route.GetRoutingParameters();
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
            router.ConsumeCommands(args, out var routes);
            var binds = router.Bind(routes, args).ToList();
            Assert.AreEqual(1, binds.Count);

        }
    }
}
