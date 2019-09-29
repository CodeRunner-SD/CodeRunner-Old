using CodeRunner.IO;
using CodeRunner.Managements;
using CodeRunner.Managements.FSBased;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Managements
{
    [TestClass]
    public class TOperationManager
    {
        private async Task TestManager(IOperationManager manager)
        {
            await manager.Initialize();
            Assert.IsNotNull(await manager.Settings);
            {
                Package<BaseOperation> sample = new Package<BaseOperation>(
                    new SimpleCommandLineOperation()
                    .Use(new CommandLineTemplate()))
                {
                    Metadata = new PackageMetadata
                    {
                        Name = nameof(sample)
                    }
                };
                await manager.SetValue(nameof(sample), sample);
                Assert.IsTrue(await manager.HasKey(nameof(sample)));
                Package<BaseOperation>? reget = await manager.GetValue(nameof(sample));
                Assert.IsNotNull(reget);
                Assert.AreEqual(nameof(sample), reget?.Metadata?.Name);
                Assert.IsInstanceOfType(reget!.Data, typeof(SimpleCommandLineOperation));

                await manager.SetValue(nameof(sample), null);
                Assert.IsFalse(await manager.HasKey(nameof(sample)));
            }
        }

        [TestMethod]
        public async Task FSBased()
        {
            using TempDirectory td = new TempDirectory();
            IOperationManager manager = new OperationManager(td.Directory);
            await TestManager(manager);
        }
    }
}
