using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Resources.Programming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Test.App.Mocks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TNewCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            TestWorkspace workspace = new TestWorkspace();
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new NewCommand().Build(),
                new string[] { "new", "c", "a" },
                before: async context =>
                {
                    _ = await Utils.InitializeWorkspace(context);
                    await context.Services.GetWorkspace().Templates.SetValue("c", FileTemplates.C);
                    return 0;
                },
                after: context =>
                {
                    workspace.AssertInvoked(nameof(IWorkspace.Create));
                    return Task.FromResult<Wrapper<int>>(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
