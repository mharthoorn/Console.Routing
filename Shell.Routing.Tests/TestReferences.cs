﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Shell.Routing.Tests
{
    [TestClass]
    public class TestReferences
    {
        // This class tests commands as they are used in known (reference) tools
        // Like git
        // Work in progress. Some plans to implement other parameter styles later,
        // like single dash flags (windows), and slash flags, like msdos.

        Router router = Routing<ToolModule>.Router;

        [TestMethod]
        public void CommitMessage()
        {
            var arguments = Utils.CreateArguments("commit", "-m", "\"ux: change layout\""); // git
            var result = router.Bind(arguments);
            Assert.AreEqual(result.Bind.Route.Method.Name, "Commit");
        }

    }
}
