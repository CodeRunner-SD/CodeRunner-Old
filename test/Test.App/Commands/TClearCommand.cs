using CodeRunner.Commands;
using CodeRunner.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading.Tasks;

namespace Test.App.Commands
{
    [TestClass]
    public class TClearCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            TestTerminal terminal = new TestTerminal();
            CodeRunner.Pipelines.PipelineResult<int> result = await Utils.UsePipeline(async context =>
            {
                Parser parser = CommandLines.CreateParser(new ClearCommand().Build(), context);
                return await parser.InvokeAsync("clear", context.Services.Get<IConsole>());
            }, terminal);
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(0, result.Result);
        }
    }
}
