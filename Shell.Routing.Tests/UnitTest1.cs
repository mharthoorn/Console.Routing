using Harthoorn.Shell.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestRoutes
    {
        Router router = new Router(Assembly.GetAssembly(typeof(ToolCommands)));

        [TestMethod]
        public void TestSectionDefault()
        {
            var arguments = Utils.ParseArguments("tool");
            var routes = router.GetCommandRoutes(arguments);
            var route = routes.First();
            Assert.AreEqual(route.Method.Name, "Tool");
        }

        [TestMethod]
        public void TestSingleValue()
        {
            var arguments = Utils.ParseArguments("single Foo");
            var routes = router.GetCommandRoutes(arguments);
            var method = routes.First().Method;
            if (method.TryBind(arguments, out var values))
            {
                Assert.AreEqual(method.Name, "Single");
                Assert.AreEqual(values.Length, 1);
                Assert.AreEqual(values[0], "Foo");
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
