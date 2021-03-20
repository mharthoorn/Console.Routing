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
        public void OptionalBecauseAttribute()
        {
            var args = router.Parse("optionaltryme");
            var result = router.Bind(args);
            
            Assert.AreEqual("OptionalTryMe", result.Bind.Route.Method.Name);
        }

        [TestMethod]
        public void OptionalBecauseNullable()
        {

            var args = router.Parse("nullabletryme");
            var result = router.Bind(args);
            
            Assert.AreEqual("NullableTryMe", result.Bind.Route.Method.Name);
        }

        [TestMethod]
        public void OptionalBecauseDefault()
        {
            var args = router.Parse("defaulttryme");
            var result = router.Bind(args);

            Assert.AreEqual("DefaultTryMe", result.Bind.Route.Method.Name);
        }

    }
}
