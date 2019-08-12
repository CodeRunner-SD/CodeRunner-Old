using CodeRunner.Helpers;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.App.Helpers
{
    [TestClass]
    public class TResolveContext
    {
        [TestMethod]
        public void Basic()
        {
            ResolveContext context = new ResolveContext();
            context.FromArgumentList(new string[] { "a=a", "b=b" });
            Assert.AreEqual("a", context.GetVariable<string>(new Variable("a")));
            Assert.AreEqual("b", context.GetVariable<string>(new Variable("b")));
        }
    }
}
