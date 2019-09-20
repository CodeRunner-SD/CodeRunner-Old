using CodeRunner.IO;
using CodeRunner.Packagings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Test.Core.IO
{
    [TestClass]
    public class TFileLoaderPool
    {
        [TestMethod]
        public async Task Package()
        {
            using TempFile tf = new TempFile();
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject(new Package<string>("a")));
            PackageFileLoaderPool<string> pool = new PackageFileLoaderPool<string>();
            PackageFileLoader<string> loader = pool.Get(tf.File);
            Assert.AreEqual("a", (await loader.GetData())?.Data);
            Assert.AreSame(loader, pool.Get(tf.File));
        }
    }
}
