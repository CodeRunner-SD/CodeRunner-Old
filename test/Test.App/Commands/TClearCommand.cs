using CodeRunner.Extensions.Builtin.Console.Commands;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Test.App.Mocks;

namespace Test.App.Commands
{
    [TestClass]
    public class TClearCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            TestWorkspace workspace = new TestWorkspace();
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new ClearCommand().Build(),
                new string[] { "clear" });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
