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

            string key0 = DocumentationHelper.GetParamKey(parameters[0].ParameterType);
            Assert.AreEqual("System.String", key0);

            string key1 = DocumentationHelper.GetParamKey(parameters[1].ParameterType);
            Assert.AreEqual("System.Boolean", key1);

            string key2 = DocumentationHelper.GetParamKey(parameters[2].ParameterType);
            Assert.AreEqual("ConsoleRouting.Flag{System.Int32}", key2);
        }
        
    }
}
