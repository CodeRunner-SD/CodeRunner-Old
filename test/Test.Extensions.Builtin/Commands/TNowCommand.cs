using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Test.App.Mocks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TNowCommand
    {
        [TestMethod]
        public async Task File()
        {
            TestWorkspace workspace = new TestWorkspace(
                onCreate: (name, tem, callback) => Task.FromResult<IWorkItem?>(new TestWorkItem()));

            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new NowCommand().Build(),
                new string[] { "now", "--", "type=f", "target=a.c" },
                before: Utils.InitializeWorkspace,
                after: context => Task.FromResult<Wrapper<int>>(0));

            workspace.AssertInvoked(nameof(IWorkspace.Create));
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }

        [TestMethod]
        public async Task Directory()
        {
            TestWorkspace workspace = new TestWorkspace(
                onCreate: (name, tem, callback) => Task.FromResult<IWorkItem?>(new TestWorkItem()));

            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new NowCommand().Build(),
                new string[] { "now", "--", "type=d", "target=a" },
                before: Utils.InitializeWorkspace,
                after: context => Task.FromResult<Wrapper<int>>(0));

            workspace.AssertInvoked(nameof(IWorkspace.Create));

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
