using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestGlobals 
    {
        Router router = new RouterBuilder().AddAssemblyOf<TestGlobals>().Build();

        [TestMethod]
        public void BasicFlagBind()
        {
            SomeSettings.Debug = false;
            var args = router.Parse("main sub --debug");

            router.Binder.Bind(typeof(SomeSettings), args);

            Assert.IsTrue(SomeSettings.Debug);
        }

        [TestMethod]
        public void RemovalOfGlobals()
        {
            AnimalSettings.Mouse = false;
            AnimalSettings.Cat = false;
            AnimalSettings.Canary = false;

            var args = router.Parser.Parse("train --mouse --cat --dog");

            router.Binder.Bind(typeof(AnimalSettings), args);

            Assert.IsTrue(AnimalSettings.Mouse);
            Assert.IsTrue(AnimalSettings.Cat);
            Assert.IsFalse(AnimalSettings.Canary);
            Assert.AreEqual(2, args.Count);

        }

        [TestMethod]
        public void GlobalBindingInRouting()
        {
            var args = router.Parser.Parse("train --mouse --cat --dog");
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

            var args = router.Parse("do -mcd");
           
            router.Binder.Bind(typeof(AnimalSettings), args);

            Assert.IsTrue(AnimalSettings.Mouse);
            Assert.IsTrue(AnimalSettings.Cat);
            Assert.IsFalse(AnimalSettings.Canary);

            Assert.AreEqual(2, args.Count);
        }
    }
}
