using CodeRunner.IO;
using CodeRunner.Packagings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Test.Core.IO
{
    [TestClass]
    public class TObjectFileLoader
    {
        [TestMethod]
        public async Task Json()
        {
            using TempFile tf = new TempFile();
            JsonFileLoader<string> loader = new JsonFileLoader<string>(tf.File);
            Assert.IsNull(await loader.GetData());
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject("a"));
            Assert.AreEqual("a", await loader.GetData());
            await loader.Save("b");
            Assert.AreEqual("b", await loader.GetData());
        }

        [TestMethod]
        public async Task Package()
        {
            using TempFile tf = new TempFile();
            PackageFileLoader<string> loader = new PackageFileLoader<string>(tf.File);
            Assert.IsNull(await loader.GetData());
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject(new Package<string>("a")));
            Assert.AreEqual("a", (await loader.GetData())?.Data);
            await loader.Save(new Package<string>("b"));
            Assert.AreEqual("b", (await loader.GetData())?.Data);
        }
    }
}
