using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Core.Templates
{
    [TestClass]
    public class TVariable
    {
        [TestMethod]
        public void Collection()
        {
            VariableCollection variables = new VariableCollection
            {
                new Variable("var"),
                new Variable("var"),
                new Variable("var2")
            };
            Assert.AreEqual(2, variables.Count);
            Assert.IsTrue(variables.Contains(new Variable("var2")));
            Assert.IsTrue(variables.Remove(new Variable("var2")));
            Assert.AreEqual(1, variables.Count);
            variables.Clear();
            StringTemplate st = new StringTemplate("abc", new[] { new Variable("A") });
            variables.Collect(new[] { st });
            Assert.IsTrue(variables.Contains(new Variable("A")));
        }
    }
}
