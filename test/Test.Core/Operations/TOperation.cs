using CodeRunner.IO;
using CodeRunner.Operations;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        public async Task Basic()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_HelloWorld, Encoding.UTF8);

            StringTemplate source = new StringTemplate(
                    StringTemplate.GetVariableTemplate(OperationVariables.InputPath.Name),
                        new Variable[] {
                            OperationVariables.InputPath
                        }
                );

            Operation op = new Operation(new[]
            {
                new CommandLineTemplate()
                    .UseCommand(Utils.GetPythonFile())
                    .UseArgument(source)
            });

            ResolveContext context = new ResolveContext()
                .WithVariable(OperationVariables.InputPath.Name, tmp.File.FullName)
                .WithVariable(Operation.VarShell.Name, Utils.GetShell());

            op.CommandExecuting += Op_CommandExecuting;
            op.CommandExecuted += Op_CommandExecuted;

            Assert.IsTrue(await op.Resolve(context));
        }

        private Task<bool> Op_CommandExecuting(Operation sender, int index, System.Diagnostics.ProcessStartInfo process, string[] command)
        {
            return Task.FromResult(true);
        }

        private Task<bool> Op_CommandExecuted(Operation sender, int index, CodeRunner.Executors.ExecutorResult result)
        {
            Assert.AreEqual(CodeRunner.Executors.ExecutorState.Ended, result.State);
            StringAssert.Contains(result.Output, "Hello");
            return Task.FromResult(true);
        }
    }
}
