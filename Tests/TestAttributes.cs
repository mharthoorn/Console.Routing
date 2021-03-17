using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestAttributes
    {
        Router router = new RouterBuilder()
            .AddAssemblyOf<TestParameters>()
            .Build();

        [TestMethod]
        public void Optionality()
        {
            var args = router.Parse("tryme");
            var result = router.Bind(args);
            
            Assert.AreEqual("TryMe", result.Bind.Route.Method.Name);
        }

    }
}
