using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests;

[TestClass]
public class TestArrays
{

    [TestMethod]
    public void RemainingArray()
    {
        Router router = new RouterBuilder().AddModule<ConsumingTestModule>().Build();
        var args = router.Parse("remainingarray a b c d e");
        var result = router.Bind(args);

        Assert.AreEqual("RemainingArray", result.Bind.Route.Method.Name);
        Assert.AreEqual(3, result.Bind.Parameters.Length);
        Assert.IsInstanceOfType(result.Bind.Parameters[2], typeof(string[]));
        var array = result.Bind.Parameters[2] as string[];
        Assert.AreEqual("c", array[0]);
        Assert.AreEqual("d", array[1]);
        Assert.AreEqual("e", array[2]);
    }


    [TestMethod]
    public void RemainingArrayEmpty()
    {
        Router router = new RouterBuilder().AddModule<ConsumingTestModule>().Build();
        var args = router.Parse("remainingarray a b");
        var result = router.Bind(args);

        Assert.AreEqual("RemainingArray", result.Bind.Route.Method.Name);
        Assert.AreEqual(3, result.Bind.Parameters.Length);
        Assert.IsInstanceOfType(result.Bind.Parameters[2], typeof(string[]));
        var array = result.Bind.Parameters[2] as string[];
        Assert.AreEqual(0, array.Length);
    }

    [TestMethod]
    public void AllArray()
    {
        Router router = new RouterBuilder().AddModule<ConsumingTestModule>().Build();
        var args = router.Parse("allarray a b");
        var result = router.Bind(args);

        Assert.AreEqual("AllArray", result.Bind.Route.Method.Name);
        Assert.AreEqual(3, result.Bind.Parameters.Length);
        Assert.IsInstanceOfType(result.Bind.Parameters[2], typeof(string[]));
        var array = result.Bind.Parameters[2] as string[];
        Assert.AreEqual("a", array[0]);
        Assert.AreEqual("b", array[1]);
    }

    [TestMethod]
    public void AllArrayNotFullyConsumed()
    {
        Router router = new RouterBuilder().AddModule<ConsumingTestModule>().Build();
        var args = router.Parse("allarray a b c");
        var result = router.Bind(args);

        Assert.IsFalse(result.Ok);
        Assert.AreEqual(1, result.Candidates.Count);
        Assert.AreEqual("AllArray", result.Candidates[0].Route.Method.Name);
    }

    [TestMethod]
    public void RemainingArguments()
    {
        Router router = new RouterBuilder().AddModule<ConsumingTestModule>().Build();
        var args = router.Parse("remainingarguments a b c d e");
        var result = router.Bind(args);

        Assert.AreEqual("RemainingArguments", result.Bind.Route.Method.Name);
        Assert.AreEqual(3, result.Bind.Parameters.Length);
        Assert.IsInstanceOfType(result.Bind.Parameters[2], typeof(Arguments));
        var arguments = result.Bind.Parameters[2] as Arguments;
        Assert.AreEqual("c", arguments[0].Value);
        Assert.AreEqual("d", arguments[1].Value);
        Assert.AreEqual("e", arguments[2].Value);
    }
}