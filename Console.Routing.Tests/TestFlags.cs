using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestFlags
    {
        Router router = new RouteBuilder().AddAssemblyOf<TestFlags>().Build();

        [TestMethod]
        public void BaseFlags()
        {
            Arguments args; bool found;

            // long flag found
            args = Arguments.Parse("-a -b --test");
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

            args = Arguments.Parse("-a --test");

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

            args = Arguments.Parse("-a -t");
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

            args = Arguments.Parse("-a -txy");

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
            var arguments = Arguments.Create("commit", "-m", "\"ux: change layout\""); // git parameters

            var result = router.Bind(arguments);
            Assert.AreEqual(result.Bind.Route.Method.Name, "Commit");
            Assert.AreEqual(1, result.Count);

        }

    }

}
