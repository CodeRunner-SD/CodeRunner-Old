using CodeRunner.Executors;
using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace Test.Core.Executors
{
    [TestClass]
    public class TCLIExecutor
    {
        private const string C_HelloWorld = @"print(""Hello World!"")";
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

        private string GetPythonFile()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return "C:/Python37/python.exe";
            }
            else
            {
                return "/usr/bin/python3";
            }
        }

        [TestMethod]
        public void Basic()
        {
            using (TempFile tmp = new TempFile())
            {
                File.WriteAllText(tmp.File.FullName, C_HelloWorld, Encoding.UTF8);
                using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName));
                ExecutorResult res = cli.Run().Result;
                Assert.AreEqual(0, res.ExitCode);
                StringAssert.Contains(res.Output, "Hello World!");
                Assert.ThrowsExceptionAsync<Exception>(async () =>
                {
                    await cli.Run();
                });
            }
            using (TempFile tmp = new TempFile())
            {
                File.WriteAllText(tmp.File.FullName, C_Exit1, Encoding.UTF8);
                using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName));
                ExecutorResult res = cli.Run().Result;
                Assert.AreEqual(1, res.ExitCode);
            }
        }

        [TestMethod]
        public void Input()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_Input, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName))
            {
                Input = "hello"
            };
            ExecutorResult res = cli.Run().Result;
            Assert.AreEqual(ExecutorState.Ended, res.State);
            StringAssert.Contains(res.Output, "hello");
        }

        [TestMethod]
        public void TimeOut()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_DeadCycle, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName))
            {
                TimeLimit = TimeSpan.FromSeconds(0.2)
            };
            ExecutorResult res = cli.Run().Result;
            Assert.AreEqual(ExecutorState.OutOfTime, res.State);
            Assert.IsTrue(res.RunningTime.TotalSeconds >= 0.1);
        }

        /*[TestMethod]
        public void Error()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_DeadCycle, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName))
            {
                TimeLimit = TimeSpan.FromSeconds(1)
            };
            System.Threading.Tasks.Task<ExecutorResult> task = cli.Run();
            Assert.ThrowsException<Exception>(() => cli.Initialize());
            cli.Process.Kill(true);
            ExecutorResult res = task.Result;
            Assert.AreEqual(ExecutorState.Ended, res.State);
            Assert.AreNotEqual(0, res.ExitCode);
        }*/

        [TestMethod]
        public void MemoryOut()
        {
            using TempFile tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_3MB, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName))
            {
                MemoryLimit = 1024
            };
            ExecutorResult res = cli.Run().Result;
            Assert.AreEqual(ExecutorState.OutOfMemory, res.State);
        }
    }
}
