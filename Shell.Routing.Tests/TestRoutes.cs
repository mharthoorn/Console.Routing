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
        public void BaseFlags()
        {
            Arguments args; bool found;

            // long flag found
            args = Utils.ParseArguments("-a -b --test");
            found = args.TryGet<Flag>("test", out var test);
            Assert.AreEqual(true, found); // broken
            Assert.AreEqual("test", test.Name); // broken

            // shortflag found
            found = args.TryGet<Flag>("a", out var flagx);
            Assert.AreEqual(true, found);
            Assert.AreEqual("a", flagx.Name); // broken

            found = args.TryGet<Flag>("q", out var flagq);
            Assert.AreEqual(false, found);
            Assert.AreEqual(null, flagq); // broken
        }

        [TestMethod]
        public void LongFlags()
        {
            Arguments args; bool found;

            args = Utils.ParseArguments("-a --test");

            found = args.TryGet<Flag>("test", out var test);
            Assert.AreEqual(true, found);
            Assert.AreEqual("test", test.Name); // broken
            Assert.AreEqual(true, test.Set); // broken

            // you cannot abreviate long flags. .
            found = args.TryGet<Flag>("tes", out test);
            Assert.AreEqual(false, found); // broken
            Assert.AreEqual(null, test); // broken

            found = args.TryGet<Flag>("testing", out test);
            Assert.AreEqual(false, found); // broken
            Assert.AreEqual(null, test); // broken

            found = args.TryGet<Flag>("t", out test);
            Assert.AreEqual(false, found);
            Assert.AreEqual(null, test); 

        }

        [TestMethod]
        public void ShortFlags()
        {
            Arguments args; bool found;

            args = Utils.ParseArguments("-a -t");
            found = args.TryGet<Flag>("test", out var test);
            Assert.AreEqual(true, found); 
            Assert.AreEqual("t", test.Name); 
            Assert.AreEqual(true, test.Short);

            found = args.TryGet<Flag>("t", out var t);
            Assert.AreEqual(true, found); 
            Assert.AreEqual("t", t.Name); 
            Assert.AreEqual(true, test.Short);
        }


        [TestMethod]
        public void MultiFlags()
        {
            Arguments args; bool found;

            args = Utils.ParseArguments("-a -txy");

            found = args.TryGet<Flag>("test", out var test);
            Assert.AreEqual(true, found);
            Assert.AreEqual("t", test.Name); 
            Assert.AreEqual(true, test.Set); 

            found = args.TryGet<Flag>("t", out test);
            Assert.AreEqual(true, found);
            Assert.AreEqual("t", test.Name); 


            found = args.TryGet<Flag>("x", out test);
            Assert.AreEqual(true, found);
            Assert.AreEqual("x", test.Name); 

            found = args.TryGet<Flag>("y", out test);
            Assert.AreEqual(true, found);
            Assert.AreEqual("y", test.Name); 

            found = args.TryGet<Flag>("q", out test);
            Assert.AreEqual(false, found);
            Assert.AreEqual(null, test); 
        }


        [TestMethod]
        public void TestSectionDefault()
        {
            var arguments = Utils.ParseArguments("tool");
            router.ConsumeCommands(arguments, out var routes);
            var route = routes.First();
            Assert.AreEqual(route.Method.Name, "Tool");
        }

        [TestMethod]
        public void TestSingleValue()
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
        public void TestOptions()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action William will -foo -bar fubar");

            router.ConsumeCommands(arguments, out var routes);
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

            router.ConsumeCommands(arguments, out var routes);
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
            args1.TryGet<Flag>("test", out var option1);
            Assert.AreEqual(null, "abc"); // broken

            var args2 = Utils.ParseArguments("-a -b -test:efg");
            args2.TryGet<Assignment>("test", out var option2); // TryGetOptionValue("test", out var option2);
            Assert.AreEqual(option2.Value, "efg"); // broken


            var args3 = Utils.ParseArguments("-a -b -test: hij");
            args3.TryGet<Assignment>("test", out var option3);
            Assert.AreEqual(option3.Value, "hij");
        }

        [TestMethod]
        public void TestOpionValues()
        {
            var line = "save -all -pattern '{id}-{id}'";
            var args = Utils.ParseArguments(line);
            router.ConsumeCommands(args, out var routes);
            var binds = router.Bind(routes, args).ToList();
            Assert.AreEqual(1, binds.Count);

        }


    }
}
