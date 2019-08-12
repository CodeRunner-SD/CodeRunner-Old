using CodeRunner;
using CodeRunner.Commands;
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
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new NowCommand().Build(),
                new string[] { "now", "-f", "a.c" },
                before: Utils.InitializeWorkspace,
                after: context =>
                {
                    CodeRunner.Managements.WorkItem item = context.Services.GetWorkItem();
                    Assert.IsNotNull(item);
                    Assert.AreEqual(CodeRunner.Managements.WorkItemType.File, item!.Type);
                    Assert.AreSame(item!.File, item!.Target);
                    Assert.AreEqual("a.c", item!.RelativePath);
                    return Task.FromResult(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }

        [TestMethod]
        public async Task Directory()
        {
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new NowCommand().Build(),
                new string[] { "now", "-d", "a" },
                before: Utils.InitializeWorkspace,
                after: context =>
                {
                    CodeRunner.Managements.WorkItem item = context.Services.GetWorkItem();
                    Assert.IsNotNull(item);
                    Assert.AreEqual(CodeRunner.Managements.WorkItemType.Directory, item!.Type);
                    Assert.AreSame(item!.Directory, item!.Target);
                    Assert.AreEqual("a", item!.RelativePath);
                    return Task.FromResult(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }
    }
}
