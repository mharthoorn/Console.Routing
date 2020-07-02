using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestGlobals 
    {
        Router router = Routing<TestGlobals>.Router;

        [TestMethod]
        public void BasicFlagBind()
        {
            Arguments args; 

            args = Utils.ParseArguments("main sub --debug");
            Globals.Bind(typeof(SomeSettings), args);

            Assert.AreEqual("debug", SomeSettings.Debug.Name);
            Assert.IsTrue(SomeSettings.Debug.Set);
        }

        [TestMethod]
        public void RemovalOfGlobals()
        {
            var args = Utils.ParseArguments("do --mouse --cat --dog");
            Globals.Bind(typeof(AnimalSettings), args);

            Assert.AreEqual("mouse", AnimalSettings.Mouse.Name);
            Assert.IsTrue(AnimalSettings.Mouse.Set);

            Assert.IsTrue(AnimalSettings.Cat);

            Assert.AreEqual(null, AnimalSettings.Canary);

            Assert.AreEqual(2, args.Count);

        }

        public void Multiflag()
        {
            var args = Utils.ParseArguments("do -mcd");
            Globals.Bind(typeof(AnimalSettings), args);

            Assert.AreEqual("mouse", AnimalSettings.Mouse.Name);
            Assert.IsTrue(AnimalSettings.Mouse.Set);

            Assert.IsTrue(AnimalSettings.Cat);

            Assert.AreEqual(null, AnimalSettings.Canary);

            Assert.AreEqual(2, args.Count);
        }
    }

}
