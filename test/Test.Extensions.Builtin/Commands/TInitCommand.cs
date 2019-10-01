using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Test.App.Mocks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TInitCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            TestWorkspace workspace = new TestWorkspace();
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new InitCommand().Build(),
                new string[] { "init" },
                after: context => Task.FromResult<Wrapper<int>>(0));
            workspace.AssertInvoked(nameof(IWorkspace.Initialize));
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }

        [TestMethod]
        public async Task Delete()
        {
            TestWorkspace workspace = new TestWorkspace();
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new InitCommand().Build(),
                new string[] { "init", "--delete" },
                after: context => Task.FromResult<Wrapper<int>>(0));

            workspace.AssertInvoked(nameof(IWorkspace.Clear));
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
