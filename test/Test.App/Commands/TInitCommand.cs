using CodeRunner.Commands;
using CodeRunner.Pipelines;
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
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new InitCommand().Build(),
                new string[] { "init" },
                after: context => Task.FromResult<Wrapper<int>>(0));

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }

        [TestMethod]
        public async Task Delete()
        {
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new InitCommand().Build(),
                new string[] { "init", "--delete" },
                after: context => Task.FromResult<Wrapper<int>>(0));

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
