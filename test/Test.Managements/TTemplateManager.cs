using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Test.Managements
{
    [TestClass]
    public class TTemplateManager
    {
        [TestMethod]
        public async Task Basic()
        {
            using TempDirectory td = new TempDirectory();
            TemplateManager manager = new TemplateManager(td.Directory);
            await manager.Initialize();
            Assert.IsNotNull(await manager.Settings);
            Assert.IsTrue(await manager.Has("c"));
            CodeRunner.Managements.Configurations.TemplateItem c = await manager.Get("c");
            Assert.IsNotNull(c);
            CodeRunner.Templates.BaseTemplate vc = await c.Value;
            await c.Value;
            await c.Value;
            Assert.IsNotNull(vc);
            {
                string name = "tc";
                string newFile = "tc.tpl";
                File.Copy(Path.Join(td.Directory.FullName, c.FileName), Path.Join(td.Directory.FullName, newFile));
                await manager.Set(name, new CodeRunner.Managements.Configurations.TemplateItem
                {
                    FileName = newFile
                });
                Assert.IsTrue(await manager.Has(name));
                await manager.Set(name, new CodeRunner.Managements.Configurations.TemplateItem
                {
                    FileName = newFile
                });
                CodeRunner.Managements.Configurations.TemplateItem tc = await manager.Get(name);
                Assert.IsNotNull(tc);
                CodeRunner.Templates.BaseTemplate vtc = await tc.Value;
                Assert.IsNotNull(vtc);
                Assert.AreEqual(vc.Metadata.Author, vtc.Metadata.Author);

                await manager.Set(name, null);
                Assert.IsFalse(await manager.Has(name));
                Assert.IsNull(await manager.Get(name));
            }
        }
    }
}
