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
            ResolveContext context = new ResolveContext().WithVariable("name", "lily").WithVariable(FileTemplate.Var.Name, temp.File.FullName);
            context.WithVariable("rs", "abc").WithoutVariable("rs");
            TextFileTemplate tf = new TextFileTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"), new Variable[] { new Variable("name").NotRequired("") }));
            FileInfo fi = tf.Resolve(context).Result;
            Assert.AreEqual("lily", File.ReadAllText(fi.FullName));
            using (FileStream st = temp.File.OpenWrite())
            {
                tf.Save(st).Wait();
            }

            using (FileStream st = temp.File.OpenRead())
            {
                TextFileTemplate ctf = BaseTemplate.Load<TextFileTemplate>(st).Result;
                Assert.IsTrue(ctf.Content.Variables.Contains(new Variable("name")));
            }
        }
    }

    [TestClass]
    public class Base
    {
        [TestMethod]
        public void Type()
        {
            StringTemplate st = new StringTemplate("content", new Variable[] { new Variable("var") });
            byte[] res;
            using (MemoryStream ms = new MemoryStream())
            {
                st.Save(ms).Wait();
                res = ms.ToArray();
            }
            using (MemoryStream ms = new MemoryStream(res))
            {
                BaseTemplate item = BaseTemplate.Load<BaseTemplate>(ms).Result;
                Assert.IsInstanceOfType(item, typeof(StringTemplate));
            }
        }
    }
}
