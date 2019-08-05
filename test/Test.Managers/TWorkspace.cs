using CodeRunner.IO;
using CodeRunner.Managers;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Managers
{
    [TestClass]
    public class TWorkspace
    {
        [TestMethod]
        public void Basic()
        {
            using (TempDirectory td = new TempDirectory())
            {
                Workspace workspace = new Workspace(td.Directory);
                Assert.IsFalse(workspace.HasInitialized);
                Assert.IsFalse(workspace.CheckValid().Result);
                workspace.Initialize().Wait();
                Assert.IsTrue(workspace.HasInitialized);
                Assert.IsTrue(workspace.CheckValid().Result);
                Assert.IsNotNull(workspace.Settings.Result);
            }
        }

        [TestMethod]
        public void Templates()
        {
            using (TempDirectory td = new TempDirectory())
            {
                Workspace workspace = new Workspace(td.Directory);
                workspace.Initialize().Wait();
                Assert.IsNotNull(workspace.Templates.Get<TextFileTemplate>("c").Result);
            }
        }
    }
}
