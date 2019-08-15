using CodeRunner;
using CodeRunner.Helpers;
using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Test.App
{
    [TestClass]
    public class Integrate
    {
        private async Task UsingInput(string content, Func<TextReader, Task> action)
        {
            using MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            using StreamReader sr = new StreamReader(ms);
            await action(sr);
        }

        [TestInitialize]
        public void Setup()
        {
            Program.Environment = EnvironmentType.Test;
        }

        [TestMethod]
        public async Task Basic()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput("--version", async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
            });
        }

        [TestMethod]
        public async Task Init()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "init"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
                Assert.IsTrue(TestView.Workspace!.HasInitialized);
            });
            await UsingInput(string.Join('\n', "init --delete"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
                Assert.IsFalse(TestView.Workspace!.HasInitialized);
            });
        }

        [TestMethod]
        public async Task NewNow()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "init", "new c", "a"), async input =>
             {
                 TestView.Input = input;
                 Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
                 Assert.IsTrue(File.Exists(Path.Join(td.Directory.FullName, "a.c")));
             });
            await UsingInput(string.Join('\n', "now -f a.c"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
            });
        }

        [TestMethod]
        public async Task Run()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "init", "run hello -- name=sun", '\n'), async input =>
             {
                 TestView.Input = input;
                 Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
                 StringAssert.Contains(TestView.Console!.Out.ToString(), "hello sun");
             });
        }

        [TestMethod]
        public async Task Debug()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "debug"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
            });
        }

        [TestMethod]
        public async Task Clear()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "clear"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
            });
        }

        [TestMethod]
        public async Task Template()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "init", "template list"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
                StringAssert.Contains(TestView.Console!.Out.ToString(), "python");
            });
        }

        [TestMethod]
        public async Task Operation()
        {
            using TempDirectory td = new TempDirectory();
            await UsingInput(string.Join('\n', "init", "operation list"), async input =>
            {
                TestView.Input = input;
                Assert.AreEqual(0, await Program.Main(new string[] { "-d", td.Directory.FullName }));
                StringAssert.Contains(TestView.Console!.Out.ToString(), "hello");
            });
        }
    }
}
