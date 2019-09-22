using CodeRunner;
using CodeRunner.Commands;
using CodeRunner.Managements.FSBased;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TNowCommand
    {
        [TestMethod]
        public async Task File()
        {
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new NowCommand().Build(),
                new string[] { "now", "-f", "a.c" },
                before: Utils.InitializeWorkspace,
                after: context =>
                {
                    WorkItem? item = context.Services.GetWorkItem() as WorkItem;
                    Assert.IsNotNull(item);
                    Assert.AreEqual(CodeRunner.Managements.WorkItemType.File, item!.Type);
                    Assert.AreSame(item!.File, item!.Target);
                    Assert.AreEqual("a.c", item!.Name);
                    return Task.FromResult<Wrapper<int>>(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }

        [TestMethod]
        public async Task Directory()
        {
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new NowCommand().Build(),
                new string[] { "now", "-d", "a" },
                before: Utils.InitializeWorkspace,
                after: context =>
                {
                    WorkItem? item = context.Services.GetWorkItem() as WorkItem;
                    Assert.IsNotNull(item);
                    Assert.AreEqual(CodeRunner.Managements.WorkItemType.Directory, item!.Type);
                    Assert.AreSame(item!.Directory, item!.Target);
                    Assert.AreEqual("a", item!.Name);
                    return Task.FromResult<Wrapper<int>>(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
