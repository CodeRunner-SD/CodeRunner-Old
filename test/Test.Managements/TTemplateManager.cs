using CodeRunner.IO;
using CodeRunner.Managements;
using CodeRunner.Resources.Programming;
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
            await manager.Install("c", CodeRunner.Resources.Programming.Templates.C);
            Assert.IsNotNull(await manager.Settings);
            Assert.IsTrue(await manager.Has("c"));
            CodeRunner.Managements.Configurations.TemplateItem c = await manager.Get("c");
            Assert.IsNotNull(c);
            CodeRunner.Packagings.Package<CodeRunner.Templates.BaseTemplate> vc = await c.Value;
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
                CodeRunner.Packagings.Package<CodeRunner.Templates.BaseTemplate> vtc = await tc.Value;
                Assert.IsNotNull(vtc);
                Assert.AreEqual(vc.Metadata.Author, vtc.Metadata.Author);

                await manager.Set(name, null);
                Assert.IsFalse(await manager.Has(name));
                Assert.IsNull(await manager.Get(name));
            }
            {
                await manager.Install("c", Templates.C);
                CodeRunner.Managements.Configurations.TemplateItem item = await manager.Get("c");
                Assert.IsNotNull(item);
                string path = Path.Join(td.Directory.FullName, item.FileName);
                Assert.IsTrue(File.Exists(path));
                await manager.Uninstall("c");
                Assert.IsFalse(File.Exists(path));
            }
        }
    }
}
