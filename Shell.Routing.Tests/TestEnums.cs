using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestEnums
    {
        Router router = Routing<ToolModule>.Router;

        [TestMethod]
        public void EnumValue()
        {
            // Matching the case sensitivity
            var args = Utils.ParseArguments("bump Minor");
            var result = router.Bind(args);
            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Bump", result.Route.Method.Name);

            // Case insensitive
            args = Utils.ParseArguments("bump minor");
            result = router.Bind(args);

            Assert.AreEqual(1, result.Routes.Count());
            Assert.AreEqual("Bump", result.Route.Method.Name);

            // Not a valid enum value
            args = Utils.ParseArguments("bump minora");
            result = router.Bind(args);

            Assert.AreEqual(0, result.Routes.Count());

        }
    }

}
