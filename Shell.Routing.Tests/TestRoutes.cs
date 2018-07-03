using Shell.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestRoutes
    {
        Router router = new Router(Assembly.GetAssembly(typeof(ToolCommands)));

        [TestMethod]
        public void TestSectionDefault()
        {
            var arguments = Utils.ParseArguments("tool");
            var routes = router.GetCommandRoutes(arguments);
            var route = routes.First();
            Assert.AreEqual(route.Method.Name, "Tool");
        }

        [TestMethod]
        public void TestSingleValue()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action Foo");

            var routes = router.GetCommandRoutes(arguments);
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
        public void TestOptions()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action William will -foo -bar fubar");

            var routes = router.GetCommandRoutes(arguments);
            Assert.AreEqual(routes.Count(), 2);

            var binds = router.Bind(routes, arguments).ToList();
            Assert.AreEqual(binds.Count, 1);

            var bind = binds.First();
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var routingparams = bind.Route.GetRoutingParameters();
            Assert.AreEqual(routingparams.Count(), 4);

            Assert.AreEqual(bind.Arguments[0], "William");
            Assert.AreEqual(bind.Arguments[1], "will");
            Assert.AreEqual(((Option)bind.Arguments[2]).Set, true); // -foo

            Assert.AreEqual("fubar", ((OptionValue)bind.Arguments[3])); // -bar fubar
        }

        [TestMethod]
        public void TestAliases()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("-t");

            var routes = router.GetCommandRoutes(arguments);
            Assert.AreEqual(routes.Count(), 1);

            var binds = router.Bind(routes, arguments).ToList();
            Assert.AreEqual(binds.Count, 1);

            var bind = binds.First();
            Assert.AreEqual(bind.Route.Method.Name, "Tool");
        }

        [TestMethod]
        public void TestOptionValue()
        {
            var args1 = Utils.ParseArguments("-a -b -test abc");
            args1.TryGetOptionValue("test", out var option1);
            Assert.AreEqual(option1.Value, "abc");

            var args2 = Utils.ParseArguments("-a -b -test:efg");
            args2.TryGetOptionValue("test", out var option2);
            Assert.AreEqual(option2.Value, "efg");


            var args3 = Utils.ParseArguments("-a -b -test: hij");
            args3.TryGetOptionValue("test", out var option3);
            Assert.AreEqual(option3.Value, "hij");
        }

        [TestMethod]
        public void TestOpionValues()
        {
            var line = "save -all -pattern '{id}-{id}'";
            var args = Utils.ParseArguments(line);
            var routes = router.GetCommandRoutes(args);
            var binds = router.Bind(routes, args).ToList();
            Assert.AreEqual(1, binds.Count);

        }


    }
}
