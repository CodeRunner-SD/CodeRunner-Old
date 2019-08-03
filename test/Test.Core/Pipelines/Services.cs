using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core
{
    [TestClass]
    public class Services
    {
        [TestMethod]
        public void Provider()
        {
            ServiceProvider provider = new ServiceProvider();
            {
                var scope = provider.CreateScope("a").Result;
                Assert.AreEqual("a", scope.Name);
                Assert.IsFalse(scope.TryGet<object>(out _));

                scope.Add("str");
                Assert.AreEqual("str", scope.Get<string>());

                scope.Add("str-id", "id");
                Assert.AreEqual("str-id", scope.Get<string>("id"));

                scope.Remove<string>();
                Assert.IsFalse(scope.TryGet<string>(out _));
                Assert.IsTrue(scope.TryGet<string>(out _, "id"));

                scope.Add(0);
                Assert.AreEqual(0, scope.Get<int>());

                scope.Replace(1);
                Assert.AreEqual(1, scope.Get<int>());
            }
            {
                var scope = provider.CreateScope("b").Result;
                Assert.AreEqual("b", scope.Name);
                Assert.IsFalse(scope.TryGet<object>(out _));

                Assert.AreEqual("str-id", scope.Get<string>("id"));
                Assert.AreEqual(1, scope.Get<int>());
            }
        }
    }
}
