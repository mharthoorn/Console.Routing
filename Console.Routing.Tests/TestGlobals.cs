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
            Arguments args; 

            args = Arguments.Parse("main sub --debug");
            Binder.Bind(typeof(SomeSettings), args);

            Assert.AreEqual("debug", SomeSettings.Debug.Name);
            Assert.IsTrue(SomeSettings.Debug.Set);
        }

        [TestMethod]
        public void RemovalOfGlobals()
        {
            var args = Arguments.Parse("train --mouse --cat --dog");
            Binder.Bind(typeof(AnimalSettings), args);

            Assert.AreEqual("mouse", AnimalSettings.Mouse.Name);
            Assert.IsTrue(AnimalSettings.Mouse.Set);

            Assert.IsTrue(AnimalSettings.Cat);

            Assert.AreEqual(null, AnimalSettings.Canary);

            Assert.AreEqual(2, args.Count);

        }

        [TestMethod]
        public void GlobalBindingInRouting()
        {
            var args = Arguments.Parse("train --mouse --cat --dog");
            var result = router.Handle(args);

            Assert.IsTrue(result.Ok);
            Assert.AreEqual(2, result.Arguments.Count);

            Assert.AreEqual("mouse", AnimalSettings.Mouse.Name);
            Assert.IsTrue(AnimalSettings.Mouse.Set);

         
            var x = result.Bind.Route;

        }

        public void Multiflag()
        {
            var args = Arguments.Parse("do -mcd");
            Binder.Bind(typeof(AnimalSettings), args);

            Assert.AreEqual("mouse", AnimalSettings.Mouse.Name);
            Assert.IsTrue(AnimalSettings.Mouse.Set);

            Assert.IsTrue(AnimalSettings.Cat);

            Assert.AreEqual(null, AnimalSettings.Canary);

            Assert.AreEqual(2, args.Count);
        }
    }

}
