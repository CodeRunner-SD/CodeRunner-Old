using CodeRunner.IO;
using CodeRunner.Templates;
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
        public async Task Template()
        {
            using TempFile tf = new TempFile();
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject(new StringTemplate("a")));
            TemplateFileLoaderPool<StringTemplate> pool = new TemplateFileLoaderPool<StringTemplate>();
            TemplateFileLoader<StringTemplate> loader = pool.Get(tf.File);
            Assert.AreEqual("a", (await loader.Data)?.Content);
            Assert.AreSame(loader, pool.Get(tf.File));
        }
    }
}
