using ConsoleRouting.Tests.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests
{
    [TestClass]
    public class TestDocumentation
    {
        [TestMethod]
        public void ParamKeys()
        {
            var method = typeof(DocumentationTestModule).GetMethod("Run");
            var parameters = method.GetParameters();

            string key0 = DocKeys.BuildParamKey(parameters[0].ParameterType);
            Assert.AreEqual("System.String", key0);

            string key1 = DocKeys.BuildParamKey(parameters[1].ParameterType);
            Assert.AreEqual("System.Boolean", key1);

            string key2 = DocKeys.BuildParamKey(parameters[2].ParameterType);
            Assert.AreEqual("ConsoleRouting.Flag{System.Int32}", key2);
        }
        
    }
}
