using CodeRunner.Executors;
using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Executors
{
    [TestClass]
    public class TCLIExecutor
    {
        private const string C_HelloWorld = @"print(""Hello World!"",end='')";
        private const string C_DeadCycle = @"import time
while(True):
    time.sleep(1)";
        private const string C_3MB = @"l = []
for i in range(0, 10**3):
    l.append(i)
print(l)";
        private const string C_Input = @"s = input()
print(s)";
        private const string C_Exit1 = @"exit(1)";

        [TestMethod]
        public async Task Basic()
        {
            using (TempFile tmp = new TempFile())
            {
                File.WriteAllText(tmp.File.FullName, C_HelloWorld, Encoding.UTF8);
                using CLIExecutor cli = new CLIExecutor(new CLIExecutorSettings(Utils.GetShell(), new string[] { "-c", $"python {tmp.File.FullName}" })
                {
                    CollectOutput = true
                });
                ExecutorResult res = await cli.Run();
                Assert.AreEqual(0, res.ExitCode);
                StringAssert.Contains(res.Output, "Hello World!");
                _ = await Assert.ThrowsExceptionAsync<Exception>(async () => await cli.Run());
            }
            using (TempFile tmp = new TempFile())
            {
                File.WriteAllText(tmp.File.FullName, C_Exit1, Encoding.UTF8);
                using CLIExecutor cli = new CLIExecutor(new CLIExecutorSettings(Utils.GetShell(), new string[] { "-c", $"python {tmp.File.FullName}" }));
                ExecutorResult res = await cli.Run();
                Assert.AreEqual(1, res.ExitCode);
            }
        }

        [TestMethod]
        public async Task Input()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_Input, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new CLIExecutorSettings(Utils.GetShell(), new string[] { "-c", $"python {tmp.File.FullName}" })
            {
                Input = "hello",
                CollectOutput = true
            });
            ExecutorResult res = await cli.Run();
            Assert.AreEqual(ExecutorState.Ended, res.State);
            StringAssert.Contains(res.Output, "hello");
        }

        [TestMethod]
        public async Task TimeOut()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_DeadCycle, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new CLIExecutorSettings(Utils.GetShell(), new string[] { "-c", $"python {tmp.File.FullName}" })
            {
                TimeLimit = TimeSpan.FromSeconds(0.2)
            });
            ExecutorResult res = await cli.Run();
            Assert.AreEqual(ExecutorState.OutOfTime, res.State);
            Assert.IsTrue(res.RunningTime.TotalSeconds >= 0.1);
        }

        [TestMethod]
        public async Task Error()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_DeadCycle, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new CLIExecutorSettings(Utils.GetShell(), new string[] { "-c", $"python {tmp.File.FullName}" })
            {
                TimeLimit = TimeSpan.FromSeconds(0.5)
            });
            Task<ExecutorResult> task = cli.Run();
            await cli.Kill();
            ExecutorResult res = await task;
            Assert.AreEqual(ExecutorState.Ended, res.State);
            Assert.AreNotEqual(0, res.ExitCode);
        }

        [TestMethod]
        public async Task MemoryOut()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_3MB, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new CLIExecutorSettings(Utils.GetShell(), new string[] { "-c", $"python {tmp.File.FullName}" })
            {
                MemoryLimit = 1024
            });
            ExecutorResult res = await cli.Run();
            Assert.AreEqual(ExecutorState.OutOfMemory, res.State);
        }
    }
}
