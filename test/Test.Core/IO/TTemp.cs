using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Core.IO
{
    [TestClass]
    public class TTemp
    {
        [TestMethod]
        public void Files()
        {
            FileInfo file;
            using (TempFile tmp = new TempFile("tmp"))
            {
                file = tmp.File;
                Assert.IsTrue(file.Exists);
            }
            file.Refresh();
            Assert.IsFalse(file.Exists);
        }

        [TestMethod]
        public void Directories()
        {
            DirectoryInfo file;
            using (TempDirectory tmp = new TempDirectory(Path.GetTempPath()))
            {
                file = tmp.Directory;
                Assert.IsTrue(file.Exists);
            }
            file.Refresh();
            // Assert.IsFalse(file.Exists);
        }
    }
}
