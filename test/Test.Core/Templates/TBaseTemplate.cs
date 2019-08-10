using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Core.Templates
{
    [TestClass]
    public class TBaseTemplate
    {
        [TestMethod]
        public void SaveLoad()
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
                tp.Save(st).Wait();
            }

            using (System.IO.FileStream st = temp.File.OpenRead())
            {
                BaseTemplate rt = BaseTemplate.Load(st).Result;
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
