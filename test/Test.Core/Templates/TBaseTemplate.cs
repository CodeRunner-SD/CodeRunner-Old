using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.Core.Templates
{
    [TestClass]
    public class TBaseTemplate
    {
        [TestMethod]
        public async Task SaveLoad()
        {
            using TempFile temp = new TempFile();
            StringTemplate tp = new StringTemplate("content", new Variable[] { new Variable("var") })
            {
                Metadata = new TemplateMetadata
                {
                    Author = "author",
                    CreationTime = DateTimeOffset.Now,
                    Version = new Version()
                }
            };
            using (System.IO.FileStream st = temp.File.OpenWrite())
            {
                await tp.Save(st);
            }

            using (System.IO.FileStream st = temp.File.OpenRead())
            {
                BaseTemplate rt = await BaseTemplate.Load(st);
                Assert.AreEqual("author", rt.Metadata.Author);
                Assert.IsTrue(((StringTemplate)rt).Variables.Contains(new Variable("var")));
            }
        }

        [TestMethod]
        public void Variable()
        {
            TextFileTemplate tp = new TextFileTemplate(new StringTemplate("content", new Variable[] { new Variable("var") })
            {
                Metadata = new TemplateMetadata
                {
                    Author = "author",
                    CreationTime = DateTimeOffset.Now,
                    Version = new Version()
                }
            });
            Assert.IsTrue(tp.GetVariables().Contains(new Variable("var")));
        }
    }
}
