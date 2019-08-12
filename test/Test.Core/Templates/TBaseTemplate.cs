using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Core.Templates
{
    [TestClass]
    public class TBaseTemplate
    {
        [TestMethod]
        public void Variable()
        {
            TextFileTemplate tp = new TextFileTemplate(new StringTemplate("content", new Variable[] { new Variable("var") }));
            Assert.IsTrue(tp.GetVariables().Contains(new Variable("var")));
        }
    }
}
