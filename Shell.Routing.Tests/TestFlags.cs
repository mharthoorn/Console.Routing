using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace Shell.Routing.Tests
{ 
    [TestClass]
    public class TestFlags
    {
        Router router = new Router(Assembly.GetAssembly(typeof(ToolModule)));

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
        public void FlagValues_GitCommit()
        {
            var arguments = Utils.CreateArguments("commit", "-m", "\"ux: change layout\""); // git
            
            router.ConsumeCommands(arguments, out var routes);
            var route = routes.First();
            Assert.AreEqual(route.Method.Name, "Commit");


            var binds = router.Bind(routes, arguments).ToList();
            Assert.AreEqual(1, binds.Count);
            

        }

    }

}
