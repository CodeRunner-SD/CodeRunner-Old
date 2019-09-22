using CodeRunner;
using CodeRunner.Commands;
using CodeRunner.Managements.FSBased;
using CodeRunner.Pipelines;
using CodeRunner.Resources.Programming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TNewCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new NewCommand().Build(),
                new string[] { "new", "c", "a" },
                before: async context =>
                {
                    _ = await Utils.InitializeWorkspace(context);
                    await context.Services.GetWorkspace().Templates.Set("c", Templates.C);
                    return 0;
                },
                after: context =>
                {
                    Workspace workspace = (Workspace)context.Services.GetWorkspace();
                    Assert.IsTrue(File.Exists(Path.Join(workspace.PathRoot.FullName, "a.c")));
                    return Task.FromResult<Wrapper<int>>(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
