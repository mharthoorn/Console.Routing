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

            // fallback should not happen with just wrong parameters. Only when there are no candidates.

            arguments = router.Parse("CommandOne"); // wrong parameter count
            result = router.Bind(arguments);
            Assert.IsFalse(result.Ok);
            Assert.AreEqual(1, result.Candidates.Count);
            Assert.AreEqual("CommandOne", result.Candidates[0].Route.Method.Name);

            arguments = router.Parse("CommandOne a b "); // wrong parameter count
            result = router.Bind(arguments);
            Assert.IsFalse(result.Ok);
            Assert.AreEqual(1, result.Candidates.Count);
            Assert.AreEqual("CommandOne", result.Candidates[0].Route.Method.Name);


        }

    }
}
