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
            TextFileTemplate tf = new TextFileTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"), new string[] { "name" }));
            var fi = tf.Resolve(new TemplateResolveContext().With("name", "lily").With(TextFileTemplate.VarFilePath, Path.GetTempFileName())).Result;
            Assert.AreEqual("lily", File.ReadAllText(fi.FullName, tf.Encoding));
            fi.Delete();
        }
    }
}
