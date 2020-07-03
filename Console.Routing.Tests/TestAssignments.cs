using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestAssignments
    {
        Router router = new RouteBuilder().AddAssemblyOf<ToolModule>().Build();

        [TestMethod]
        public void BasicAssignments()
        {
            var arguments = Arguments.Parse("single a=b");
            var result = router.Bind(arguments);
        
            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Single", result.Route.Method.Name);
            Assert.AreEqual(2, result.Arguments.Count);

            var args = result.Bind.Arguments;

            Assert.AreEqual(1, args.Length);
            Assert.IsTrue(args[0] is Assignment);
            
            var assignment = args[0] as Assignment;
            Assert.AreEqual("a", assignment.Name);
            Assert.AreEqual("b", assignment.Value);
        }

        [TestMethod]
        public void TestExpressions()
        {
            // even though the query might contain an equals sign, it should not be treated as an assignment

            var arguments = Arguments.Parse("expression -f 'name.where(given=''john'')'");
            var result = router.Bind(arguments);
            
            var args = result.Bind.Arguments;
            Assert.IsTrue(args[0] is Flag);
            if (args[0] is Flag f)
                Assert.AreEqual("f", f.Name);

            Assert.IsTrue(args[1] is string);
            if (args[0] is string raw)
                Assert.AreEqual("'name.where(given=''john'')'", raw);
        }


        [TestMethod]
        public void TestMix()
        { 
            // even though the query might contain an equals sign, it should not be treated as an assignment

            var arguments = Arguments.Parse("mix format=xml 'name.where(given=''john'')'");
            var result = router.Bind(arguments);

            var args = result.Bind.Arguments;
            Assert.IsTrue(args[0] is Assignment);
            if (args[0] is Assignment a)
            { 
                Assert.AreEqual("format", a.Name);
                Assert.AreEqual("xml", a.Value);
            }

            Assert.IsTrue(args[1] is string);
            if (args[0] is string raw)
                Assert.AreEqual("'name.where(given=''john'')'", raw);
        }

    }

}
