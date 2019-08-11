using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Core.Pipelines
{
    [TestClass]
    public class TServiceProvider
    {
        [TestMethod]
        public async Task Basic()
        {
            ServiceProvider provider = new ServiceProvider();
            {
                ServiceScope scope = await provider.CreateScope("a");
                Assert.AreEqual("a", scope.Name);
                Assert.IsFalse(scope.TryGet<object>(out _));

                scope.Add("str");
                Assert.AreEqual("str", scope.Get<string>());
                Assert.AreEqual("a", scope.GetSource<string>());

                scope.Add("str-id", "id");
                Assert.AreEqual("str-id", scope.Get<string>("id"));

                scope.Remove<string>();
                Assert.IsFalse(scope.TryGet<string>(out _));
                Assert.IsTrue(scope.TryGet<string>(out _, "id"));

                scope.Replace(0);
                Assert.AreEqual(0, scope.Get<int>());

                scope.Replace(1);
                Assert.AreEqual(1, scope.Get<int>());

                scope.Add(1.2);
                scope.Remove<double>();
            }
            {
                ServiceScope scope = await provider.CreateScope("b");
                Assert.AreEqual("b", scope.Name);
                Assert.IsFalse(scope.TryGet<object>(out _));

                Assert.AreEqual("str-id", scope.Get<string>("id"));
                Assert.AreEqual(1, scope.Get<int>());
            }
        }
    }
}
