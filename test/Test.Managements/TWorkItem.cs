using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
}
