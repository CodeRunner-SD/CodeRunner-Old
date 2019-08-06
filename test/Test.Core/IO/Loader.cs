using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Test.Core.IO
{
    [TestClass]
    public class Loader
    {
        [TestMethod]
        public void JsonFile()
        {
            using TempFile tf = new TempFile();
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject("a"));
            JsonFileLoader<string> loader = new JsonFileLoader<string>(tf.File);
            Assert.AreEqual("a", loader.Data.Result);
            File.WriteAllText(tf.File.FullName, JsonConvert.SerializeObject("b"));
            Assert.AreEqual("b", loader.Data.Result);
        }
    }
}
