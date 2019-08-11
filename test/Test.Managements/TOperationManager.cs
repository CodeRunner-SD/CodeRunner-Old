using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Test.Managements
{
    [TestClass]
    public class TOperationManager
    {
        [TestMethod]
        public async Task Basic()
        {
            using TempDirectory td = new TempDirectory();
            OperationManager manager = new OperationManager(td.Directory);
            await manager.Initialize();
            Assert.IsNotNull(await manager.Settings);
            Assert.IsTrue(await manager.Has("hello"));
            CodeRunner.Managements.Configurations.OperationItem c = await manager.Get("hello");
            Assert.IsNotNull(c);
            CodeRunner.Operations.Operation vc = await c.Value;
            await c.Value;
            await c.Value;
            Assert.IsNotNull(vc);
            {
                string name = "tc";
                string newFile = "tc.tpl";
                File.Copy(Path.Join(td.Directory.FullName, c.FileName), Path.Join(td.Directory.FullName, newFile));
                await manager.Set(name, new CodeRunner.Managements.Configurations.OperationItem
                {
                    FileName = newFile
                });
                Assert.IsTrue(await manager.Has(name));
                await manager.Set(name, new CodeRunner.Managements.Configurations.OperationItem
                {
                    FileName = newFile
                });
                CodeRunner.Managements.Configurations.OperationItem tc = await manager.Get(name);
                Assert.IsNotNull(tc);
                CodeRunner.Operations.Operation vtc = await tc.Value;
                Assert.IsNotNull(vtc);
                Assert.AreEqual(vc.Metadata.Author, vtc.Metadata.Author);

                await manager.Set(name, null);
                Assert.IsFalse(await manager.Has(name));
                Assert.IsNull(await manager.Get(name));
            }
        }
    }
}
