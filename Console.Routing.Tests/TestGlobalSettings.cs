using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestGlobalSettings
    {
        Router router = Routing<TestGlobalSettings>.Router;

        [TestMethod]
        public void GlobalSettings()
        {
            Arguments args; RoutingResult result;

            args = Utils.ParseArguments("main sub --debug");
            result = router.Bind(args);
            Assert.AreEqual(true, Globalsettings.Debug);

            args = Utils.ParseArguments("main sub");
            result = router.Bind(args);
            Assert.AreEqual(false, Globalsettings.Debug);
        }
    }

}
