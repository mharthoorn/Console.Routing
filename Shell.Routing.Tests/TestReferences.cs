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
            var result = router.Bind(arguments);
            Assert.AreEqual(result.Bind.Endpoint.Method.Name, "Commit");
        }

    }
}
