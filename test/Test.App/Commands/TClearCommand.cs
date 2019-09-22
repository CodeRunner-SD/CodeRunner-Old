using CodeRunner.Commands;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TClearCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(
                new ClearCommand().Build(),
                new string[] { "clear" });

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
