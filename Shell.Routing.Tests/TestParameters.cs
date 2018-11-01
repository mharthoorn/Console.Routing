﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestParameters
    {
        Router router = Routing<ToolModule>.Router;

        [TestMethod]
        public void SingleLiteral()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = Utils.ParseArguments("action Foo");
            var result = router.Bind(arguments);

            Assert.AreEqual(result.Count, 1);

            var bind = result.Bind;
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var parameters = bind.Route.Method.GetRoutingParameters();
            Assert.AreEqual(parameters.Count(), 1);

            Assert.AreEqual(1, bind.Arguments.Length);
            Assert.AreEqual(bind.Arguments[0], "Foo");
        }


        [TestMethod]
        public void FlagValue()
        {
            var arguments = Utils.ParseArguments("-a -b --test abc");
            arguments.TryGet("test", out Flag flag);
            Assert.AreEqual("test", flag.Name);
            arguments.TryGetFlagValue("test", out var value);
            Assert.AreEqual("abc", value);
        }

        [TestMethod]
        public void FlagValueBinding()
        {
            // since we match on used parameter count, flag vaues are a special case
            var arguments = Utils.ParseArguments("save --all --pattern '{id}-{id}'");
            var result = router.Bind(arguments);
            Assert.AreEqual(1, result.Count);

        }

    }
}