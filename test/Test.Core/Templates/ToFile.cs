using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

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
            Assert.AreEqual("lily", File.ReadAllText(fi.FullName));
            using (var st = temp.File.OpenWrite())
                tf.Save(st).Wait();
            using (var st = temp.File.OpenRead())
            {
                var ctf = BaseTemplate.Load<TextFileTemplate>(st).Result;
                Assert.IsTrue(ctf.Content.Variables.Contains("name"));
            }
        }
    }
}
