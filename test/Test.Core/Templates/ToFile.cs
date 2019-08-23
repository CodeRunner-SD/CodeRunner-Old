using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Templates
{
    [TestClass]
    public class TFile
    {
        [TestMethod]
        public async Task Text()
        {
            using TempFile temp = new TempFile();
            ResolveContext context = new ResolveContext().WithVariable("name", "lily").WithVariable(FileTemplate.Var, temp.File.FullName);
            TextFileTemplate tf = new TextFileTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"), new Variable[] { new Variable("name").NotRequired("") }));
            FileInfo fi = await tf.Resolve(context);
            Assert.AreEqual("lily", File.ReadAllText(fi.FullName));
        }

        [TestMethod]
        public async Task Binary()
        {
            using TempFile temp = new TempFile();
            ResolveContext context = new ResolveContext().WithVariable(FileTemplate.Var, temp.File.FullName);
            BinaryFileTemplate tf = new BinaryFileTemplate(Encoding.UTF8.GetBytes("hello"));
            FileInfo fi = await tf.Resolve(context);
            Assert.AreEqual("hello", File.ReadAllText(fi.FullName));
        }
    }
}
