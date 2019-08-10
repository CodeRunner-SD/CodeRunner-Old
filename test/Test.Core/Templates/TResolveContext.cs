using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Core.Templates
{
    [TestClass]
    public class TResolveContext
    {
        [TestMethod]
        public void Basic()
        {
            ResolveContext context = new ResolveContext();
            context.WithVariable("a", "a");
            Assert.IsTrue(context.HasVariable("a"));
            Assert.AreEqual("a", context.GetVariable<string>(new Variable("a")));
            context.WithoutVariable("a");
            Assert.IsFalse(context.HasVariable("a"));
        }
    }
}
