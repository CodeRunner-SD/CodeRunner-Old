using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Test.Core.IO
{
    [TestClass]
    public class TFileLoaderPool
    {
        [TestMethod]
        public void Template()
        {
            using TempFile tf = new TempFile();
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject(new StringTemplate("a")));
            TemplateFileLoaderPool<StringTemplate> pool = new TemplateFileLoaderPool<StringTemplate>();
            TemplateFileLoader<StringTemplate> loader = pool.Get(tf.File);
            Assert.AreEqual("a", loader.Data.Result?.Content);
            Assert.AreSame(loader, pool.Get(tf.File));
        }
    }
}
