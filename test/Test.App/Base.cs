using CodeRunner;
using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Test.App
{
    [TestClass]
    public class Base
    {
        private void UsingInput(string content, Action<TextReader> action)
        {
            using MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            using StreamReader sr = new StreamReader(ms);
            action(sr);
        }

        [TestInitialize]
        public void Setup()
        {
            Program.Environment = EnvironmentType.Test;
        }

        [TestMethod]
        public void Basic()
        {
            using TempDirectory td = new TempDirectory();
            UsingInput("--version", input =>
            {
                Program.Workspace = new CodeRunner.Managers.Workspace(td.Directory);
                Program.Input = input;
                Assert.AreEqual(0, Program.Main(Array.Empty<string>()).Result);
            });
        }

        [TestMethod]
        public void Init()
        {
            using TempDirectory td = new TempDirectory();
            UsingInput("init", input =>
            {
                Program.Workspace = new CodeRunner.Managers.Workspace(td.Directory);
                Program.Input = input;
                Assert.AreEqual(0, Program.Main(Array.Empty<string>()).Result);
                Assert.IsTrue(Program.Workspace.CheckValid().Result);
            });
        }

        [TestMethod]
        public void New()
        {
            using TempDirectory td = new TempDirectory();
            UsingInput(string.Join('\n', "init", "new c", "a"), input =>
            {
                Program.Workspace = new CodeRunner.Managers.Workspace(td.Directory);
                Program.Input = input;
                Assert.AreEqual(0, Program.Main(Array.Empty<string>()).Result);
                Assert.IsTrue(File.Exists(Path.Join(td.Directory.FullName, "a.c")));
            });
        }
    }
}
