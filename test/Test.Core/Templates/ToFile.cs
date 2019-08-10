using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Test.Core.Templates
{
    [TestClass]
    public class TFile
    {
        [TestMethod]
        public void Text()
        {
            using TempFile temp = new TempFile();
            ResolveContext context = new ResolveContext().WithVariable("name", "lily").WithVariable(FileTemplate.Var.Name, temp.File.FullName);
            TextFileTemplate tf = new TextFileTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"), new Variable[] { new Variable("name").NotRequired("") }));
            FileInfo fi = tf.Resolve(context).Result;
            Assert.AreEqual("lily", File.ReadAllText(fi.FullName));
        }

        [TestMethod]
        public void Binary()
        {
            using TempFile temp = new TempFile();
            ResolveContext context = new ResolveContext().WithVariable(FileTemplate.Var.Name, temp.File.FullName);
            BinaryFileTemplate tf = new BinaryFileTemplate(Encoding.UTF8.GetBytes("hello"));
            FileInfo fi = tf.Resolve(context).Result;
            Assert.AreEqual("hello", File.ReadAllText(fi.FullName));
        }
    }
}
