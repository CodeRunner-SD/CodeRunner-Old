using CodeRunner;
using CodeRunner.Helpers;
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
                Program.Input = input;
                Assert.AreEqual(0, Program.Main(new string[] { "-d", td.Directory.FullName }).Result);
            });
        }

        [TestMethod]
        public void Init()
        {
            using TempDirectory td = new TempDirectory();
            UsingInput("init", input =>
            {
                Program.Input = input;
                Assert.AreEqual(0, Program.Main(new string[] { "-d", td.Directory.FullName }).Result);
                Assert.IsTrue(TestView.Workspace.CheckValid().Result);
            });
        }

        [TestMethod]
        public void New()
        {
            using TempDirectory td = new TempDirectory();
            UsingInput(string.Join('\n', "init", "new c", "a"), input =>
            {
                Program.Input = input;
                Assert.AreEqual(0, Program.Main(new string[] { "-d", td.Directory.FullName }).Result);
                Assert.IsTrue(File.Exists(Path.Join(td.Directory.FullName, "a.c")));
            });
        }

        [TestMethod]
        public void Run()
        {
            using TempDirectory td = new TempDirectory();
            UsingInput(string.Join('\n', "init", "run hello name=sun", '\n'), input =>
             {
                 Program.Input = input;
                 Assert.AreEqual(0, Program.Main(new string[] { "-d", td.Directory.FullName }).Result);
                 StringAssert.Contains(TestView.Console.Out.ToString(), "hello");
             });
        }
    }
}
