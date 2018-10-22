using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestReferences
    {
        Router router = Routing<ToolModule>.Router;

        [TestMethod]
        public void CommitMessage()
        {
            var arguments = Utils.CreateArguments("commit", "-m", "\"ux: change layout\""); // git
            router.ConsumeCommands(arguments, out var routes);
            var binds = router.Bind(routes, arguments).ToList();
            var route = routes.First();
            Assert.AreEqual(route.Method.Name, "Commit");
        }

    }
}
