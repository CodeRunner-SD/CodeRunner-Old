using CodeRunner;
using CodeRunner.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TRunCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UseSampleCommandInvoker(
                new RunCommand().Build(),
                new string[] { "hello", "--", "name=a" },
                before: Utils.InitializeWorkspace,
                after: context =>
                {
                    StringAssert.Contains(context.Services.GetConsole().Out.ToString(), "hello");
                    return Task.FromResult(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }
    }
}
