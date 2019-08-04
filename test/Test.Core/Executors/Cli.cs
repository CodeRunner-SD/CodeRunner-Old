using CodeRunner.Executors;
using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test.Core.Executors
{
    [TestClass]
    public class Cli
    {
        const string C_HelloWorld = @"print(""Hello World!"")";
        const string C_DeadCycle = @"import time
while(True):
    time.sleep(1)";
        const string C_3MB = @"l = []
for i in range(0, 10**5):
    l.append(i)";

        const string C_Exit1 = @"exit(1)";

        string GetPythonFile()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return "C:/Python37/python.exe";
            }
            else
            {
                return "/usr/bin/python";
            }
        }

        [TestMethod]
        public void Basic()
        {
            using (var tmp = new TempFile())
            {
                File.WriteAllText(tmp.File.FullName, C_HelloWorld, Encoding.UTF8);
                using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName));
                var res = cli.Run().Result;
                Assert.AreEqual(0, res.ExitCode);
                Assert.AreEqual("Hello World!", res.Output.FirstOrDefault());
            }
            using (var tmp = new TempFile())
            {
                File.WriteAllText(tmp.File.FullName, C_Exit1, Encoding.UTF8);
                using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName));
                var res = cli.Run().Result;
                Assert.AreEqual(1, res.ExitCode);
            }
        }

        [TestMethod]
        public void TimeOut()
        {
            using var tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_DeadCycle, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName))
            {
                TimeLimit = TimeSpan.FromSeconds(0.2)
            };
            var res = cli.Run().Result;
            Assert.AreEqual(ExecutorState.OutOfTime, res.State);
            Assert.IsTrue(res.RunningTime.TotalSeconds >= 0.2);
        }

        [TestMethod]
        public void MemoryOut()
        {
            using var tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_3MB, Encoding.UTF8);
            using CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(GetPythonFile(), tmp.File.FullName))
            {
                MemoryLimit = 1024
            };
            var res = cli.Run().Result;
            Assert.AreEqual(ExecutorState.OutOfMemory, res.State);
        }
    }
}
