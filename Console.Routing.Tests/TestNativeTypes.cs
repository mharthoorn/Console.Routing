using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestNativeTypes
    {
        Router router = new RouterBuilder().AddAssemblyOf<TestNativeTypes>().Build();

        [TestMethod]
        public void Booleans()
        {
            // ActionB should be matched, with '--verbose' set to false.
            var arguments = Arguments.Parse("actionb");
            var result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // ActionB should be matched, with '--verbose' set to true.
            arguments = Arguments.Parse("actionb --verbose");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // ActionB should be matched, with '--verbose' set to true.
            arguments = Arguments.Parse("actionb -v");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // ActionB should NOT be matched.
            arguments = Arguments.Parse("actionb --alt");
            result = router.Bind(arguments);
            Assert.AreEqual(0, result.BindCount);

            var rep = router.Routes.First(r => r.Method.Name == "ActionB").AsText();
            Assert.AreEqual("--verbose", rep);
        }
        
        [TestMethod]
        public void Integers()
        {
            var args = Arguments.Parse("add 3 4");
            var result = router.Bind(args);
            Assert.AreEqual(1, result.BindCount);
            Assert.AreEqual(3, result.Bind.Parameters[0]);
            Assert.AreEqual(4, result.Bind.Parameters[1]);
        }
    }
}
