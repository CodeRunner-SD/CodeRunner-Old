using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Managements
{
    [TestClass]
    public class TOperationManager
    {
        [TestMethod]
        public void Basic()
        {
            using TempDirectory td = new TempDirectory();
            OperationManager manager = new OperationManager(td.Directory);
            manager.Initialize().Wait();
            Assert.IsNotNull(manager.Settings.Result);
            Assert.IsTrue(manager.Has("hello").Result);
            CodeRunner.Managements.Configurations.OperationItem c = manager.Get("hello").Result;
            Assert.IsNotNull(c);
            CodeRunner.Operations.Operation vc = c.Value.Result;
            c.Value.Wait();
            c.Value.Wait();
            Assert.IsNotNull(vc);
            {
                string name = "tc";
                string newFile = "tc.tpl";
                File.Copy(Path.Join(td.Directory.FullName, c.FileName), Path.Join(td.Directory.FullName, newFile));
                manager.Set(name, new CodeRunner.Managements.Configurations.OperationItem
                {
                    FileName = newFile
                }).Wait();
                Assert.IsTrue(manager.Has(name).Result);
                manager.Set(name, new CodeRunner.Managements.Configurations.OperationItem
                {
                    FileName = newFile
                }).Wait();
                CodeRunner.Managements.Configurations.OperationItem tc = manager.Get(name).Result;
                Assert.IsNotNull(tc);
                CodeRunner.Operations.Operation vtc = tc.Value.Result;
                Assert.IsNotNull(vtc);
                Assert.AreEqual(vc.Metadata.Author, vtc.Metadata.Author);

                manager.Set(name, null).Wait();
                Assert.IsFalse(manager.Has(name).Result);
                Assert.IsNull(manager.Get(name).Result);
            }
        }
    }
}
