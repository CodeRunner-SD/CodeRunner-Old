using CodeRunner.IO;
using CodeRunner.Templates;
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
            Assert.IsNull(await loader.Data);
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject("a"));
            Assert.AreEqual("a", await loader.Data);
            await loader.Save("b");
            Assert.AreEqual("b", await loader.Data);
        }

        [TestMethod]
        public async Task Template()
        {
            using TempFile tf = new TempFile();
            TemplateFileLoader<StringTemplate> loader = new TemplateFileLoader<StringTemplate>(tf.File);
            Assert.IsNull(await loader.Data);
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject(new StringTemplate("a")));
            Assert.AreEqual("a", (await loader.Data)?.Content);
            await loader.Save(new StringTemplate("b"));
            Assert.AreEqual("b", (await loader.Data)?.Content);
        }
    }
}
