using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestNativeTypes
    {
        

        [TestMethod]
        public void Booleans()
        {
            Router router = new RouterBuilder().AddModule<NativesModule>().Build();
            // ActionB should be matched, with '--verbose' set to false.
            var arguments = router.Parse("actionb");
            var result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // ActionB should be matched, with '--verbose' set to true.
            arguments = router.Parse("actionb --verbose");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // ActionB should be matched, with '--verbose' set to true.
            arguments = router.Parse("actionb -v");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // ActionB should NOT be matched.
            arguments = router.Parse("actionb --alt");
            result = router.Bind(arguments);
            Assert.AreEqual(0, result.BindCount);

            var rep = router.Routes.First(r => r.Method.Name == "ActionB").AsText();
            Assert.AreEqual("--verbose", rep);
        }
        
        [TestMethod]
        public void Integers()
        {
            Router router = new RouterBuilder().AddModule<NativesModule>().Build();
            var args = router.Parse("add 3 4");
            var result = router.Bind(args);
            Assert.AreEqual(1, result.BindCount);
            Assert.AreEqual(3, result.Bind.Parameters[0]);
            Assert.AreEqual(4, result.Bind.Parameters[1]);
        }
    }
}
