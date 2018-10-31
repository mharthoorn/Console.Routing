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
        public void PlainCommandAttribute()
        {
            var arguments = Utils.ParseArguments("tool");
            var result = router.Bind(arguments);

            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Tool", result.Route.Method.Name);
        }

        [TestMethod]
        public void DefaultCommand()
        {
            var arguments = Utils.ParseArguments("");
            var result = router.Bind(arguments);

            Assert.AreEqual("Info", result.Route.Method.Name);
        }


        [TestMethod]
        public void Binding()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action William will --foo --bar fubar");
            var result = router.Bind(arguments);

            Assert.AreEqual(true, result.Ok);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(null, result.Candidates);
            Assert.AreEqual(4, result.Bind.Arguments.Count());
            // action(name, alias, foo, bar)

            var bind = result.Bind;
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var routingparams = bind.Route.Method.GetRoutingParameters();
            Assert.AreEqual(routingparams.Count(), 4);

            Assert.AreEqual(bind.Arguments[0], "William");
            Assert.AreEqual(bind.Arguments[1], "will");
            Assert.AreEqual(((Flag)bind.Arguments[2]).Set, true); // -foo

            Assert.AreEqual("fubar", ((FlagValue)bind.Arguments[3])); // -bar fubar
        }

        [TestMethod]
        public void Nesting()
        {
            var arguments = Utils.ParseArguments("main action hello");
            var result = router.Bind(arguments);
            Assert.AreEqual("Action", result.Route.Method.Name);
            Assert.AreEqual("main", result.Route.Nodes.First().Names.First());
            Assert.AreEqual("Action", result.Route.Nodes.Skip(1).First().Names.First());
            Assert.AreEqual(1, result.Count);

            arguments = Utils.ParseArguments("main sub detail hello");
            result = router.Bind(arguments);
            Assert.AreEqual("main", result.Route.Nodes.First().Names.First());
            Assert.AreEqual("Detail", result.Route.Method.Name);
            Assert.AreEqual("sub", result.Route.Nodes.Skip(1).First().Names.First());
            Assert.AreEqual("Detail", result.Route.Nodes.Skip(2).First().Names.First());
            Assert.AreEqual(1, result.Count);
        }
    }
}
