using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Core.IO
{
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
