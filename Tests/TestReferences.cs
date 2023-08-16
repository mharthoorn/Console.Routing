using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests;

[TestClass]
public class TestReferences
{
    // This class tests commands as they are used in known (reference) tools
    // Like git
    // Work in progress. Some plans to implement other parameter styles later,
    // like single dash flags (windows), and slash flags, like msdos.

    Router router = new RouterBuilder().AddAssemblyOf<TestReferences>().Build();

    [TestMethod]
    public void CommitMessage()
    {
        var arguments = router.Parse("commit", "-m", "\"ux: change layout\""); // git
        var result = router.Bind(arguments);
        Assert.AreEqual(result.Bind.Route.Method.Name, "Commit");
    }

}
