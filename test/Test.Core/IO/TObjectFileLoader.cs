using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Test.Core.IO
{
    [TestClass]
    public class TObjectFileLoader
    {
        [TestMethod]
        public void Json()
        {
            using TempFile tf = new TempFile();
            JsonFileLoader<string> loader = new JsonFileLoader<string>(tf.File);
            Assert.IsNull(loader.Data.Result);
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject("a"));
            Assert.AreEqual("a", loader.Data.Result);
            loader.Save("b").Wait();
            Assert.AreEqual("b", loader.Data.Result);
        }

        [TestMethod]
        public void Template()
        {
            using TempFile tf = new TempFile();
            TemplateFileLoader<StringTemplate> loader = new TemplateFileLoader<StringTemplate>(tf.File);
            Assert.IsNull(loader.Data.Result);
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject(new StringTemplate("a")));
            Assert.AreEqual("a", loader.Data.Result?.Content);
            loader.Save(new StringTemplate("b")).Wait();
            Assert.AreEqual("b", loader.Data.Result?.Content);
        }
    }

    [TestClass]
    public class TJsonFormatter
    {
        [TestMethod]
        public void TypeRetain()
        {
            StringTemplate st = new StringTemplate("content", new Variable[] { new Variable("var") });
            object item = JsonFormatter.Deserialize<object>(JsonFormatter.Serialize(st));
            Assert.IsInstanceOfType(item, typeof(StringTemplate));
        }
    }
}
