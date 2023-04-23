using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestParameters
    {
        Router router = new RouterBuilder().AddAssemblyOf<TestParameters>().Build();

        [TestMethod]
        public void SingleLiteral()
        {
            // ToolCommands.Single(string name) // 1 matching bind

            var arguments = router.Parse("action Foo");
            var result = router.Bind(arguments);

            Assert.AreEqual(result.BindCount, 1);

            var bind = result.Bind;
            Assert.AreEqual(bind.Route.Method.Name, "Action");

            var parameters = bind.Route.Method.GetRoutingParameters();
            Assert.AreEqual(parameters.Count(), 1);

            Assert.AreEqual(1, bind.Parameters.Length);
            Assert.AreEqual("Foo", bind.Parameters[0]);
        }


        [TestMethod]
        public void FlagValue()
        {
            var arguments = router.Parse("-a -b --test abc");
            var parameter = Parameter.Create<Flag>("test");

            arguments.TryGet(parameter, out Flag flag);
            Assert.AreEqual("test", flag.Name);
            arguments.TryGetOptionString(parameter, out var value);
            Assert.AreEqual("abc", value);
        }

        [TestMethod]
        public void FlagValueBinding()
        {
            // since we match on used parameter count, flag vaues are a special case
            // one FlagValue consumes 2 command line arguments
            var arguments = router.Parse("save --all --pattern '{id}-{id}'");
            var result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

        }

        [TestMethod]
        public void AlternateParamNames()
        {
            var arguments = router.Parse("-?"); // should route to ToolModule.Info
            var result = router.Bind(arguments);
            string s = result.ToString();
            Assert.AreEqual(1, result.BindCount);

        }

        [TestMethod]
        public void TextualPresentation()
        {
            var arguments = router.Parse("intparse --number 3");
            var result = router.Bind(arguments);
            string text = result.Bind.Route.Method.ParametersAsText();
            Assert.AreEqual("--number <value>", text);
        }

        [TestMethod]
        public void ParameterTextualPresentation()
        {
            var method = typeof(FlagTestModule).GetMethod(nameof(FlagTestModule.TypedParse));
            
            var text = method.ParametersAsText();
            Assert.AreEqual("--format <value>", text);

        }

        [TestMethod]
        public void DefaultValues()
        {
            var arguments = router.Parse("cmdwithdefault"); 
            var result = router.Bind(arguments);

            Assert.AreEqual(1, result.BindCount);
            var bind = result.Binds.First();
            var param = bind.Parameters.First();
            // no value is set, but the default is true
            Assert.AreEqual(true, param);
        }

        [TestMethod]
        public void BugOnFlags()
        {
            //THIS error is caused by the count mixup.
            // In -var, the v is counter twice (--variables and --values)
            // And the a is counted for zero. 
            // Totalling 3

            //This shouldn't bind, because there is not r. But it does.
            var arguments = router.Parse("shortflagbug -var"); 
            var result = router.Bind(arguments);
            Assert.AreEqual(0, result.BindCount);
            Assert.AreEqual(1, result.Candidates.Count);

            // fixing this, will require a rewrite of Binder.TryBindParametersx
        }

        [TestMethod]
        public void CaseSensitiveShortFlags()
        {
            // should bind to --values
            var arguments = router.Parse("paramcasesensitive -v");
            var result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);


            // should bind to --Variables
            arguments = router.Parse("paramcasesensitive -V");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // should bind to --multiline
            arguments = router.Parse("paramcasesensitive -m");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // SHOULD NOT BIND to --multiline (case sensitive)
            arguments = router.Parse("paramcasesensitive -M");
            result = router.Bind(arguments);
            Assert.AreEqual(0, result.BindCount);

            // SHOULD BIND to --multiline (case insensitive)
            arguments = router.Parse("paramcasesensitive --MUltiLIne");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);

            // should bind to --values and --Variables
            arguments = router.Parse("paramcasesensitive -vV");
            result = router.Bind(arguments);
            Assert.AreEqual(1, result.BindCount);
        }

    }
}
