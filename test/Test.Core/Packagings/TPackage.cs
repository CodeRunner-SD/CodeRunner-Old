using CodeRunner.IO;
using CodeRunner.Packagings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Test.Core.Packagings
{
    [TestClass]
    public class TPackage
    {
        [TestMethod]
        public async Task SaveLoad()
        {
            using TempFile temp = new TempFile();
            Package<string> tp = new Package<string>()
            {
                Data = "content",
                Metadata = new PackageMetadata
                {
                    Author = "author",
                    CreationTime = DateTimeOffset.Now,
                    Version = new Version()
                }
            };
            using (FileStream st = temp.File.Open(FileMode.Create, FileAccess.Write))
            {
                await tp.Save(st);
            }

            using (FileStream st = temp.File.OpenRead())
            {
                Package<string> rt = await Package.Load<string>(st);
                Assert.AreEqual("content", rt.Data);
                Assert.AreEqual("author", rt.Metadata!.Author);
            }
        }
    }
}
