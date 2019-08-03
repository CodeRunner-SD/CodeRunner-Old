using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Core.Templates
{
    [TestClass]
    public class ToFile
    {
        [TestMethod]
        public void Text()
        {
            using TempFile temp = new TempFile();
            var context = new TemplateResolveContext().With("name", "lily").With(TextFileTemplate.VarFilePath, temp.File.FullName);
            context.With("rs", "abc").Without("rs");
            TextFileTemplate tf = new TextFileTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"), new string[] { "name", "home" }));
            var fi = tf.Resolve(context).Result;
            Assert.AreEqual("lily", File.ReadAllText(fi.FullName, tf.Encoding));
        }
    }
}
