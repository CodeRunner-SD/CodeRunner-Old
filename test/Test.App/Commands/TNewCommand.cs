using CodeRunner;
using CodeRunner.Commands;
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
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new NewCommand().Build(),
                new string[] { "new", "c", "--", "name=a" },
                before: async context =>
                {
                    _ = await Utils.InitializeWorkspace(context);
                    await context.Services.GetWorkspace().Templates.Install("c", Templates.C);
                    return 0;
                },
                after: context =>
                {
                    CodeRunner.Managements.Workspace workspace = context.Services.GetWorkspace();
                    Assert.IsTrue(File.Exists(Path.Join(workspace.PathRoot.FullName, "a.c")));
                    return Task.FromResult(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }
    }
}
