using CodeRunner.IO;
using CodeRunner.Loggings;
using CodeRunner.Operations;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Operations
{
    [TestClass]
    public class TOperation
    {
        private const string C_HelloWorld = @"print(""Hello World!"")";

        [TestMethod]
        public async Task CommandLine()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_HelloWorld, Encoding.UTF8);

            StringTemplate source = new StringTemplate(
                    StringTemplate.GetVariableTemplate(OperationVariables.VarInputPath.Name),
                        new Variable[] {
                            OperationVariables.VarInputPath
                        }
                );

            SimpleCommandLineOperation op = new SimpleCommandLineOperation(new[]
            {
                new CommandLineTemplate()
                    .UseCommand("python")
                    .UseArgument(source)
            });

            ResolveContext context = new ResolveContext()
                .SetInputPath(tmp.File.FullName)
                .SetShell(Utils.GetShell());

            // op.CommandExecuting += Op_CommandExecuting;
            // op.CommandExecuted += Op_CommandExecuted;
            Pipeline<OperationWatcher, Wrapper<bool>> pipeline = await (await op.Resolve(context)).Build(new OperationWatcher(), new Logger());
            PipelineResult<Wrapper<bool>> res = await pipeline.Consume();
            Assert.IsTrue(res.IsOk && res.Result!);
        }

        /*private Task<bool> Op_CommandExecuting(BaseOperation sender, int index, CLIExecutorSettings settings, string[] commands)
        {
            settings.CollectOutput = true;
            return Task.FromResult(true);
        }

        private Task<bool> Op_CommandExecuted(BaseOperation sender, int index, CodeRunner.Executors.ExecutorResult result)
        {
            Assert.AreEqual(CodeRunner.Executors.ExecutorState.Ended, result.State);
            StringAssert.Contains(result.Output, "Hello");
            return Task.FromResult(true);
        }*/
    }
}
