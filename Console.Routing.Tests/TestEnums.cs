using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestEnums
    {
        Router router = new RouterBuilder().AddAssemblyOf<TestEnums>().Build();

        [TestMethod]
        public void EnumValue()
        {
            // Matching the case sensitivity
            var args = Arguments.Parse("bump Minor");
            var result = router.Bind(args);
            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Bump", result.Route.Method.Name);

            // Case insensitive
            args = Arguments.Parse("bump minor");
            result = router.Bind(args);

            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Bump", result.Route.Method.Name);

            // Not a valid enum value
            args = Arguments.Parse("bump minora");
            result = router.Bind(args);

            Assert.AreEqual(0, result.Routes.Count());

        }
    }

}
