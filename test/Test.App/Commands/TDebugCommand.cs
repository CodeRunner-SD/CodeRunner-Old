using CodeRunner;
using CodeRunner.Commands;
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
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new DebugCommand().Build(),
                new string[] { "debug" },
                after: context =>
                 {
                     Assert.IsFalse(string.IsNullOrEmpty(context.Services.GetConsole().Out.ToString()));
                     return Task.FromResult(0);
                 });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }
    }
}
