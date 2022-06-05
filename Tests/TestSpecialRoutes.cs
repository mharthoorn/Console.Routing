using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestSpecialRoutes
    {
        [TestMethod]
        public void DefaultCommand()
        {
            Router router = new RouterBuilder().AddAssemblyOf<TestCommands>().Build();

            var arguments = router.Parse("");
            var result = router.Bind(arguments);

            Assert.AreEqual("Info", result.Route.Method.Name);
        }

        [TestMethod]
        public void FallbackCommand()
        {
            Router router = new RouterBuilder().AddModule<FallbackTestModule>().Build();

            var arguments = router.Parse("a b c d e f g");
            var result = router.Bind(arguments);

            Assert.AreEqual("CommandThree", result.Route.Method.Name);

            Assert.AreEqual(1, result.Bind.Parameters.Length);
            var args = result.Bind.Parameters[0] as string[];
            Assert.IsNotNull(args);

            Assert.AreEqual(7, args.Length);
        }

    }
}
