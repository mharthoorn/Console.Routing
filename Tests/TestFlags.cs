using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConsoleRouting.Tests;

[TestClass]
public class TestFlags
{
 
    [TestMethod]
    public void BaseFlags()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        Arguments args; bool found;

        // long flag found
        args = router.Parse("-a -b --test");
        found = args.TryGet<Flag>("test", out var test);
        Assert.AreEqual(true, found); // broken
        Assert.AreEqual("test", test.Name); // broken

        // shortflag found
        found = args.TryGet<Flag>("a", out var flagx);
        Assert.AreEqual(true, found);
        Assert.AreEqual("a", flagx.Name); // broken

        found = args.TryGet<Flag>("q", out var flagq);
        Assert.AreEqual(false, found);
        Assert.AreEqual(null, flagq); // broken
    }

    [TestMethod]
    public void LongFlags()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        Arguments args; bool found;

        args = router.Parse("-a --test");

        found = args.TryGet<Flag>("test", out var test);
        Assert.AreEqual(true, found);
        Assert.AreEqual("test", test.Name); // broken
        Assert.AreEqual(true, test.IsSet); // broken

        // you cannot abreviate long flags. .
        found = args.TryGet<Flag>("tes", out test);
        Assert.AreEqual(false, found); // broken
        Assert.AreEqual(null, test); // broken

        found = args.TryGet<Flag>("testing", out test);
        Assert.AreEqual(false, found); // broken
        Assert.AreEqual(null, test); // broken

        found = args.TryGet<Flag>("t", out test);
        Assert.AreEqual(false, found);
        Assert.AreEqual(null, test); 

    }

    [TestMethod]
    public void ShortFlags()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        Arguments args; bool found;

        args = router.Parse("-a -t");
        found = args.TryGet<Flag>("test", out var test);
        Assert.AreEqual(true, found); 
        Assert.AreEqual("t", test.Name); 
        Assert.AreEqual(true, test.Short);

        found = args.TryGet<Flag>("t", out var t);
        Assert.AreEqual(true, found); 
        Assert.AreEqual("t", t.Name); 
        Assert.AreEqual(true, test.Short);
    }


    [TestMethod]
    public void MultiFlags()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        Arguments args; bool found;

        args = router.Parse("-a -txy");

        found = args.TryGet<Flag>("test", out var test);
        Assert.AreEqual(true, found);
        Assert.AreEqual("t", test.Name); 
        Assert.AreEqual(true, test.IsSet); 

        found = args.TryGet<Flag>("t", out test);
        Assert.AreEqual(true, found);
        Assert.AreEqual("t", test.Name); 


        found = args.TryGet<Flag>("x", out test);
        Assert.AreEqual(true, found);
        Assert.AreEqual("x", test.Name); 

        found = args.TryGet<Flag>("y", out test);
        Assert.AreEqual(true, found);
        Assert.AreEqual("y", test.Name); 

        found = args.TryGet<Flag>("q", out test);
        Assert.AreEqual(false, found);
        Assert.AreEqual(null, test); 
    }

    [TestMethod]
    public void Generics()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        var fs = new Flag<Format>("option", Format.Json);
        
        var t = fs.GetType();
        var b = t.GetGenericTypeDefinition() == typeof(Flag<>);
        var arg = t.GetGenericArguments()[0]; 
        
        Console.WriteLine(b);
    }

    [TestMethod]
    public void TestFlagValues()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        var args = router.Parse("parse --format xml");
        var result = router.Bind(args);

        Assert.AreEqual("Parse", result.Bind.Route.Method.Name);
        Assert.AreEqual("xml", (result.Bind.Parameters[0] as Flag<string>).Value);

    }
    [TestMethod]
    public void TestFlagWithoutValue()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        var args = router.Parse("flagrun command --speed fast");
        var result = router.Bind(args);

        Assert.AreEqual("FlagRun", result.Bind.Route.Method.Name);
        Assert.AreEqual(true, (result.Bind.Parameters[1] as Flag<string>).IsSet);
        Assert.AreEqual("fast", (result.Bind.Parameters[1] as Flag<string>).Value);

        

        args = router.Parse("flagrun command --speed");
        result = router.Bind(args);
        Assert.IsFalse(result.Ok);
      
        // The Arguments parameter at the end of flagwithargs method, consumes remaining arguments,
        // which causes a failed flag to pass as correct.
        args = router.Parse("flagwithargs command --speed");
        result = router.Bind(args);
        Assert.IsFalse(result.Ok);

        args = router.Parse("flagrun command");
        result = router.Bind(args);

        Assert.AreEqual("FlagRun", result.Bind.Route.Method.Name);
        Assert.AreEqual(false, (result.Bind.Parameters[1] as Flag<string>).IsSet);
        Assert.AreEqual(null, (result.Bind.Parameters[1] as Flag<string>).Value);


       
    }

    [TestMethod]
    public void TestEnumFlags()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        // Enum Parsing
        var args = router.Parse("typedparse --format json");
        var result = router.Bind(args);

        Assert.AreEqual("TypedParse", result.Bind.Route.Method.Name);
        Assert.AreEqual(Format.Json, (result.Bind.Parameters[0] as Flag<Format>).Value);
        Assert.IsTrue((result.Bind.Parameters[0] as Flag<Format>).IsSet);

        // Test invalid value
        args = router.Parse("typedparse --format bson");
        result = router.Bind(args);
        Assert.IsFalse(result.Ok);
        // but should have candidates.

        // Test incomplete flag: no value given
        args = router.Parse("typedparse --format");
        result = router.Bind(args);
        Assert.IsFalse(result.Ok);
    }

    [TestMethod]
    public void TypedFlagNotSet()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        // Test not set flag:
        var args = router.Parse("typedparse");
        var result = router.Bind(args);

        Assert.AreEqual("TypedParse", result.Bind.Route.Method.Name);
        Assert.IsFalse((result.Bind.Parameters[0] as Flag<Format>).IsSet);
    }


    [TestMethod]
    public void TestIntFlags()
    {
        Router router = new RouterBuilder().AddModule<FlagTestModule>().Build();

        var args = router.Parse("intparse --number 4");
        var result = router.Bind(args);

        Assert.AreEqual("IntParse", result.Bind.Route.Method.Name);
        Assert.AreEqual(4, (result.Bind.Parameters[0] as Flag<int>).Value);

        // Value not given:
        args = router.Parse("intparse");
        result = router.Bind(args);

        Assert.AreEqual("IntParse", result.Bind.Route.Method.Name);
        Assert.AreEqual(0, (result.Bind.Parameters[0] as Flag<int>).Value);
        Assert.AreEqual(false, (result.Bind.Parameters[0] as Flag<int>).IsSet);
    }

    [TestMethod]
    public void FlagValues_GitCommit()
    {
        Router router = new RouterBuilder().AddModule<Git>().Build();

        var arguments = router.Parse("commit", "-m", "\"ux: change layout\""); // git parameters

        var result = router.Bind(arguments);
        Assert.AreEqual(result.Bind.Route.Method.Name, "Commit");
        Assert.AreEqual(1, result.BindCount);

    }

    [TestMethod]
    public void MixedFlags()
    {
        Router router = new RouterBuilder().AddModule<MixedFlags>().Build();

        // We know this works.
        var args = router.Parse("search alias Entity --pages 2");
        var result = router.Bind(args);
        Assert.AreEqual("Search", result.Bind.Route.Method.Name);
        Assert.AreEqual("2", (result.Bind.Parameters[2] as Flag<string>).Value);
        Assert.AreEqual(false, (result.Bind.Parameters[3] as Flag).IsSet);

        // We know this works works too.
        args = router.Parse("search alias Entity --split");
        result = router.Bind(args);
        Assert.AreEqual("Search", result.Bind.Route.Method.Name);
        Assert.AreEqual(false, (result.Bind.Parameters[2] as Flag<string>).IsSet);
        Assert.AreEqual(true, (result.Bind.Parameters[3] as Flag).IsSet);

        // But the combination did not work (was caused by mixing arg count with param index)
        args = router.Parse("search alias Entity --pages 2 --split");
        result = router.Bind(args);
        Assert.AreEqual("Search", result.Bind.Route.Method.Name);
        Assert.AreEqual(true, (result.Bind.Parameters[2] as Flag<string>).IsSet);
        Assert.AreEqual("2", (result.Bind.Parameters[2] as Flag<string>).Value);
        Assert.AreEqual(true, (result.Bind.Parameters[3] as Flag).IsSet);

    }

}
