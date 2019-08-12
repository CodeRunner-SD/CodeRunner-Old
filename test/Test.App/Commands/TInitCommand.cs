using CodeRunner;
using CodeRunner.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TInitCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new InitCommand().Build(),
                new string[] { "init" },
                after: context =>
                {
                    Assert.IsTrue(context.Services.GetWorkspace().HasInitialized);
                    return Task.FromResult(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }

        [TestMethod]
        public async Task Delete()
        {
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new InitCommand().Build(),
                new string[] { "init", "--delete" },
                after: context =>
                {
                    Assert.IsFalse(context.Services.GetWorkspace().HasInitialized);
                    return Task.FromResult(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }
    }
}
