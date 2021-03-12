using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestBuilder
    {
        [TestMethod]
        public void TestGlobals()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var router = new RouterBuilder().Add(assembly).Build();
            var args=  Arguments.Parse("--alpha --beta --gamma");
            var result = router.Bind(args);
            
            Assert.IsTrue(BuilderSettings.Beta);
            Assert.IsTrue(BuilderSettings.Gamma);
            Assert.IsFalse(BuilderSettings.Delta);
        }
    }

    [Global]
    public static class BuilderSettings
    {
        public static bool Beta { get; set; }
        public static bool Gamma { get; set; }
        public static bool Delta { get; set; }
    }
}
