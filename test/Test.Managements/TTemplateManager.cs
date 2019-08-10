using CodeRunner.IO;
using CodeRunner.Managements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Managements
{
    [TestClass]
    public class TTemplateManager
    {
        [TestMethod]
        public void Basic()
        {
            using TempDirectory td = new TempDirectory();
            TemplateManager manager = new TemplateManager(td.Directory);
            manager.Initialize().Wait();
            Assert.IsNotNull(manager.Settings.Result);
            Assert.IsTrue(manager.Has("c").Result);
            CodeRunner.Managements.Configurations.TemplateItem c = manager.Get("c").Result;
            Assert.IsNotNull(c);
            CodeRunner.Templates.BaseTemplate vc = c.Value.Result;
            c.Value.Wait();
            c.Value.Wait();
            Assert.IsNotNull(vc);
            {
                string name = "tc";
                string newFile = "tc.tpl";
                File.Copy(Path.Join(td.Directory.FullName, c.FileName), Path.Join(td.Directory.FullName, newFile));
                manager.Set(name, new CodeRunner.Managements.Configurations.TemplateItem
                {
                    FileName = newFile
                }).Wait();
                Assert.IsTrue(manager.Has(name).Result);
                manager.Set(name, new CodeRunner.Managements.Configurations.TemplateItem
                {
                    FileName = newFile
                }).Wait();
                CodeRunner.Managements.Configurations.TemplateItem tc = manager.Get(name).Result;
                Assert.IsNotNull(tc);
                CodeRunner.Templates.BaseTemplate vtc = tc.Value.Result;
                Assert.IsNotNull(vtc);
                Assert.AreEqual(vc.Metadata.Author, vtc.Metadata.Author);

                manager.Set(name, null).Wait();
                Assert.IsFalse(manager.Has(name).Result);
                Assert.IsNull(manager.Get(name).Result);
            }
        }
    }
}
