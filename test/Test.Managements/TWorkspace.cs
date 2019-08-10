using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Managements
{
    [TestClass]
    public class TWorkItem
    {
        [TestMethod]
        public void Basic()
        {
            using TempDirectory td = new TempDirectory();
            Workspace workspace = new Workspace(td.Directory);
            {
                WorkItem item = WorkItem.CreateByDirectory(workspace, td.Directory);
            }
        }
    }

    [TestClass]
    public class TWorkspace
    {
        [TestMethod]
        public void Basic()
        {
            using TempDirectory td = new TempDirectory();
            Workspace workspace = new Workspace(td.Directory);
            Assert.IsFalse(workspace.HasInitialized);
            workspace.Initialize().Wait();
            Assert.IsTrue(workspace.HasInitialized);
            Assert.IsTrue(Directory.Exists(Path.Join(td.Directory.FullName, ".cr")));
            Assert.IsNotNull(workspace.Settings.Result);
            workspace.Clear().Wait();
            Assert.IsFalse(Directory.Exists(Path.Join(td.Directory.FullName, ".cr")));
        }
    }
}
