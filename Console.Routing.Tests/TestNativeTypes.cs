using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestNativeTypes
    {
        Router router = new RouteBuilder().AddAssemblyOf<TestNativeTypes>().Build();

        [TestMethod]
        public void BooleanParams()
        {
            // ActionB should be matched, with '--verbose' set to false.
            var arguments = Arguments.Parse("actionb");
            var result = router.Bind(arguments);
            Assert.AreEqual(1, result.Count);

            // ActionB should be matched, with '--verbose' set to true.
            arguments = Arguments.Parse("actionb --verbose");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.Count);

            // ActionB should be matched, with '--verbose' set to true.
            arguments = Arguments.Parse("actionb -v");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.Count);

            // ActionB should NOT be matched.
            arguments = Arguments.Parse("actionb --alt");
            result = router.Bind(arguments);
            Assert.AreEqual(0, result.Count);

            var rep = router.Routes.First(r => r.Method.Name == "ActionB").Representation();
            Assert.AreEqual("--verbose", rep);
        }
        
    }
}
