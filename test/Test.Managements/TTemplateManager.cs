using CodeRunner.IO;
using CodeRunner.Managements;
using CodeRunner.Managements.FSBased;
using CodeRunner.Packagings;
using CodeRunner.Resources.Programming;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Managements
{
    [TestClass]
    public class TTemplateManager
    {
        private async Task TestManager(ITemplateManager manager)
        {
            await manager.Initialize();
            Assert.IsNotNull(await manager.Settings);
            {
                Package<BaseTemplate> c = Templates.C;
                await manager.Set(nameof(c), c);
                Assert.IsTrue(await manager.Has(nameof(c)));
                Package<BaseTemplate>? reget = await manager.Get(nameof(c));
                Assert.IsNotNull(reget);
                Assert.AreEqual(nameof(c), reget?.Metadata?.Name);
                Assert.IsInstanceOfType(reget!.Data, typeof(BaseTemplate));

                await manager.Set(nameof(c), null);
                Assert.IsFalse(await manager.Has(nameof(c)));
            }
        }

        [TestMethod]
        public async Task FSBased()
        {
            using TempDirectory td = new TempDirectory();
            ITemplateManager manager = new TemplateManager(td.Directory);
            await TestManager(manager);
        }
    }
}
