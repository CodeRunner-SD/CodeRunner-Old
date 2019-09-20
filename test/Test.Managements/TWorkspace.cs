using CodeRunner.IO;
using CodeRunner.Managements;
using CodeRunner.Managements.FSBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Managements
{
    [TestClass]
    public class TWorkspace
    {
        private async Task TestManager(IWorkspace manager)
        {
            await manager.Initialize();
            await manager.Clear();
        }

        [TestMethod]
        public async Task FSBased()
        {
            using TempDirectory td = new TempDirectory();
            IWorkspace workspace = new Workspace(td.Directory);
            await TestManager(workspace);
        }
    }
}
