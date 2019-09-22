using CodeRunner;
using CodeRunner.Commands;
using CodeRunner.Pipelines;
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
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new RunCommand().Build(),
                new string[] { "hello", "--", "name=a" },
                before: Utils.InitializeWorkspace,
                after: context =>
                {
                    StringAssert.Contains(context.Services.GetConsole().Out.ToString(), "hello");
                    return Task.FromResult<Wrapper<int>>(0);
                });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
