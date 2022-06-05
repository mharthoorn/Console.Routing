using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestEnums
    {
        Router router = new RouterBuilder().AddModule<EnumCommands>().Build();

        [TestMethod]
        public void EnumValue()
        {
            // Matching the case sensitivity
            var args = router.Parse("bump Minor");
            var result = router.Bind(args);
            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Bump", result.Route.Method.Name);

            // Case insensitive
            args = router.Parse("bump minor");
            result = router.Bind(args);

            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Bump", result.Route.Method.Name);

            // Not a valid enum value
            args = router.Parse("bump minora");
            result = router.Bind(args);

            Assert.AreEqual(0, result.Routes.Count());

        }
    }

}
