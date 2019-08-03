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

        [TestMethod]
        public void Basic()
        {
            using var tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_HelloWorld, Encoding.UTF8);
            string pythonFile;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                pythonFile = "C:/Python37/python.exe";
            }
            else
            {
                pythonFile = "/usr/bin/python";
            }
            CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(pythonFile, tmp.File.FullName));
            var res = cli.Run().Result;
            Assert.AreEqual(0, res.ExitCode);
            Assert.AreEqual("Hello World!", res.Output.FirstOrDefault());
        }

        [TestMethod]
        public void TimeOut()
        {
            using var tmp = new TempFile();
            File.WriteAllText(tmp.File.FullName, C_DeadCycle, Encoding.UTF8);
            string pythonFile;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                pythonFile = "C:/Python37/python.exe";
            }
            else
            {
                pythonFile = "/usr/bin/python";
            }
            CLIExecutor cli = new CLIExecutor(new System.Diagnostics.ProcessStartInfo(pythonFile, tmp.File.FullName))
            {
                TimeLimit = TimeSpan.FromSeconds(0.2)
            };
            var res = cli.Run().Result;
            Assert.AreEqual(ExecutorState.OutOfTime, res.State);
        }
    }
}
