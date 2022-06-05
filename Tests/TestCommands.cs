using ConsoleRouting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestCommands
    {
        Router router = new RouterBuilder().AddAssemblyOf<TestCommands>().Build();

        [TestMethod]
        public void PlainCommandAttribute()
        {
            var arguments = router.Parse("tool");
            var result = router.Bind(arguments);

            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Tool", result.Route.Method.Name);
            Assert.AreEqual(0, result.Bind.Parameters.Length);
        }

    

        [TestMethod]
        public void Binding()
        {
            var arguments = router.Parse("action William will --foo --bar fubar");
            var result = router.Bind(arguments);

            Assert.AreEqual(true, result.Ok);
            Assert.AreEqual(result.BindCount, 1);
            Assert.AreEqual(4, result.Bind.Parameters.Count());
            Assert.IsTrue(result.Candidates.Count > 1);
            
            var bind = result.Bind;
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var paramlist = bind.Route.Method.GetRoutingParameters();
            Assert.AreEqual(paramlist.Count(), 4);

            Assert.AreEqual("William", bind.Parameters[0]);
            Assert.AreEqual("will", bind.Parameters[1]);
            Assert.AreEqual(true, (bind.Parameters[2] as Flag).IsSet); // -foo

            Assert.AreEqual("fubar", (Flag<string>)bind.Parameters[3]); // -bar fubar
        }

        [TestMethod]
        public void Nesting()
        {
            var arguments = router.Parse("main action hello");
            var result = router.Bind(arguments);
            Assert.AreEqual("Action", result.Route.Method.Name);
            Assert.AreEqual("main", result.Route.Nodes.First().Names.First());
            Assert.AreEqual("Action", result.Route.Nodes.Skip(1).First().Names.First());
            Assert.AreEqual(1, result.BindCount);

            arguments = router.Parse("main sub detail hello");
            result = router.Bind(arguments);
            Assert.AreEqual("main", result.Route.Nodes.First().Names.First());
            Assert.AreEqual("Detail", result.Route.Method.Name);
            Assert.AreEqual("sub", result.Route.Nodes.Skip(1).First().Names.First());
            Assert.AreEqual("Detail", result.Route.Nodes.Skip(2).First().Names.First());
            Assert.AreEqual(1, result.BindCount);
        }

        [TestMethod]
        public void ForgetSubCommand()
        {
            var arguments = router.Parse("mainfirst");
            var result = router.Bind(arguments);
            var count = result.Candidates.Count(RouteMatch.Partial);
            Assert.AreEqual(2, count);
        }
    }
}
