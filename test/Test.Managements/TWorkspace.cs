using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Test.Managements
{
    [TestClass]
    public class TWorkspace
    {
        [TestMethod]
        public async Task Basic()
        {
            using TempDirectory td = new TempDirectory();
            Workspace workspace = new Workspace(td.Directory);
            Assert.IsFalse(workspace.HasInitialized);
            await workspace.Initialize();
            Assert.IsTrue(workspace.HasInitialized);
            Assert.IsTrue(Directory.Exists(Path.Join(td.Directory.FullName, ".cr")));
            Assert.IsNotNull(await workspace.Settings);
            await workspace.Clear();
            Assert.IsFalse(Directory.Exists(Path.Join(td.Directory.FullName, ".cr")));
        }
    }
}
