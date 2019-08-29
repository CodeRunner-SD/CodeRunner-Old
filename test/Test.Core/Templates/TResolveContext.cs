using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Core.Templates
{
    [TestClass]
    public class TResolveContext
    {
        [TestMethod]
        public void Basic()
        {
            ResolveContext context = new ResolveContext();
            _ = context.WithVariable("a", "a");
            Assert.IsTrue(context.HasVariable("a"));
            _ = context.WithVariable("a", "b");
            Assert.AreEqual("b", context.GetVariable<string>(new Variable("a")));
            Assert.IsFalse(context.TryGetVariable<int>(new Variable("a"), out _));
            Assert.IsTrue(context.TryGetVariable<string>(new Variable("a"), out _));
            _ = context.WithoutVariable("a");
            Assert.IsFalse(context.HasVariable("a"));
            _ = Assert.ThrowsException<Exception>(() => context.GetVariable<string>(new Variable("a").Required()));
            Assert.IsFalse(context.TryGetVariable<string>(new Variable("a"), out _));
            Assert.AreEqual("c", context.GetVariable<string>(new Variable("a").NotRequired("c")));
        }
    }
}
