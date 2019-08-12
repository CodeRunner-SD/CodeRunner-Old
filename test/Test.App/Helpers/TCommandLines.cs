using CodeRunner.Helpers;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.App.Helpers
{
    [TestClass]
    public class TCommandLines
    {
        [TestMethod]
        public void ResolvePath()
        {
            Workspace workspace = new Workspace(new DirectoryInfo(Path.GetTempPath()));
            {
                FileInfo file = new FileInfo("a.txt");
                Assert.AreEqual(Path.TrimEndingDirectorySeparator(workspace.PathRoot.FullName), Path.GetDirectoryName(CommandLines.ResolvePath(workspace, file).FullName));
            }
            {
                DirectoryInfo file = new DirectoryInfo("a");
                Assert.AreEqual(Path.TrimEndingDirectorySeparator(workspace.PathRoot.FullName), Path.GetDirectoryName(CommandLines.ResolvePath(workspace, file).FullName));
            }
        }
    }
}
