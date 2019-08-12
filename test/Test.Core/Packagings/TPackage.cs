﻿using CodeRunner.IO;
using CodeRunner.Packagings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.Core.Packagings
{
    [TestClass]
    public class TPackage
    {
        [TestMethod]
        public async Task SaveLoad()
        {
            using TempFile temp = new TempFile();
            Package<string> tp = new Package<string>()
            {
                Data = "content",
                Metadata = new PackageMetadata
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
                Package<string> rt = await Package<string>.Load(st);
                Assert.AreEqual("content", rt.Data);
                Assert.AreEqual("author", rt.Metadata.Author);
            }
        }
    }
}