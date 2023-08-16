using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests;

[TestClass]
public class TestAssignments
{
    Router router = new RouterBuilder().AddAssemblyOf<ToolModule>().Build();

    [TestMethod]
    public void BasicAssignments()
    {
        var arguments = router.Parser.Parse("single a=b");
        var result = router.Bind(arguments);
    
        Assert.AreEqual(1, result.Routes.Count());
        Assert.AreEqual("Single", result.Route.Method.Name);
        Assert.AreEqual(2, result.Arguments.Count);

        var args = result.Bind.Parameters;

        Assert.AreEqual(1, args.Length);
        Assert.IsTrue(args[0] is Assignment);
        
        var assignment = args[0] as Assignment;
        Assert.AreEqual("a", assignment.Key);
        Assert.AreEqual("b", assignment.Value);
    }

    [TestMethod]
    public void TestExpressions()
    {
        // even though the query might contain an equals sign, it should not be treated as an assignment

        var arguments = router.Parse("expression -f 'name.where(given=''john'')'");
        var result = router.Bind(arguments);
        
        var args = result.Bind.Parameters;
        Assert.IsTrue(args[0] is Flag);
        if (args[0] is Flag f)
            Assert.AreEqual("f", f.Name);

        Assert.IsTrue(args[1] is string);
        if (args[0] is string raw)
            Assert.AreEqual("'name.where(given=''john'')'", raw);
    }


    [TestMethod]
    public void TestMix()
    { 
        // even though the query might contain an equals sign, it should not be treated as an assignment

        var arguments = router.Parse("mix format=xml 'name.where(given=''john'')'");
        var result = router.Bind(arguments);

        var args = result.Bind.Parameters;
        Assert.IsTrue(args[0] is Assignment);
        if (args[0] is Assignment a)
        { 
            Assert.AreEqual("format", a.Key);
            Assert.AreEqual("xml", a.Value);
        }

        Assert.IsTrue(args[1] is string);
        if (args[0] is string raw)
            Assert.AreEqual("'name.where(given=''john'')'", raw);
    }

    [TestMethod]
    public void AssignmentsAreOptional() 
    { 
        // even though the query might contain an equals sign, it should not be treated as an assignment

        var arguments = router.Parse("orderfries");
        var result = router.Bind(arguments);

        var args = result.Bind.Parameters;
        Assert.IsTrue(args[0] is Assignment);
        if (args[0] is Assignment assignment)
        {
            Assert.AreEqual(false, assignment.Provided);
            Assert.AreEqual(null, assignment.Key);
            Assert.AreEqual(null, assignment.Value);

        }
    }

    [TestMethod]
    public void TestAssignmentsArray()
    {
        var arguments = router.Parse("assignmentsarray number=3 name=John animal=Dog");
        var result = router.Bind(arguments);
        Assert.AreEqual(1, result.Bind.Parameters.Length);
        Assert.IsTrue(result.Bind.Parameters[0] is Assignment[]);
        if (result.Bind.Parameters[0] is Assignment[] a)
        {
            Assert.AreEqual("number", a[0].Key);
            Assert.AreEqual("3", a[0].Value);

            Assert.AreEqual("name", a[1].Key);
            Assert.AreEqual("John", a[1].Value);

            Assert.AreEqual("animal", a[2].Key);
            Assert.AreEqual("Dog", a[2].Value);
        }


    }

}
