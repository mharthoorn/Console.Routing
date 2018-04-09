using Harthoorn.Shell.Routing;
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


    }
}
