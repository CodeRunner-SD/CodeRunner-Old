using CodeRunner;
using CodeRunner.Commands;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TDebugCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new DebugCommand().Build(),
                new string[] { "debug" },
                after: context =>
                 {
                     Assert.IsFalse(string.IsNullOrEmpty(context.Services.GetConsole().Out.ToString()));
                     return Task.FromResult<Wrapper<int>>(0);
                 });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
