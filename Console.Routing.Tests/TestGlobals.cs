using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestGlobals 
    {
        Router router = new RouteBuilder().AddAssemblyOf<TestGlobals>().Build();

        [TestMethod]
        public void BasicFlagBind()
        {
            SomeSettings.Debug = false;
            var args = Arguments.Parse("main sub --debug");

            Binder.Bind(typeof(SomeSettings), args);

            Assert.IsTrue(SomeSettings.Debug);
        }

        [TestMethod]
        public void RemovalOfGlobals()
        {
            AnimalSettings.Mouse = false;
            AnimalSettings.Cat = false;
            AnimalSettings.Canary = false;

            var args = Arguments.Parse("train --mouse --cat --dog");
            Binder.Bind(typeof(AnimalSettings), args);

            Assert.IsTrue(AnimalSettings.Mouse);
            Assert.IsTrue(AnimalSettings.Cat);
            Assert.IsFalse(AnimalSettings.Canary);
            Assert.AreEqual(2, args.Count);

        }

        [TestMethod]
        public void GlobalBindingInRouting()
        {
            var args = Arguments.Parse("train --mouse --cat --dog");
            var result = router.Handle(args);

            Assert.IsTrue(result.Ok);
            Assert.AreEqual(2, result.Arguments.Count);

            Assert.IsTrue(AnimalSettings.Mouse);

         
            var x = result.Bind.Route;

        }

        public void Multiflag()
        {
            AnimalSettings.Mouse = false;
            AnimalSettings.Cat = false;
            AnimalSettings.Canary = false;

            var args = Arguments.Parse("do -mcd");
            Binder.Bind(typeof(AnimalSettings), args);

            Assert.IsTrue(AnimalSettings.Mouse);
            Assert.IsTrue(AnimalSettings.Cat);
            Assert.IsFalse(AnimalSettings.Canary);

            Assert.AreEqual(2, args.Count);
        }
    }

}
