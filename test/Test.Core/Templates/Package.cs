using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Core.Templates
{
    [TestClass]
    public class Package
    {
        [TestMethod]
        public void Files()
        {
            using TempFile f = new TempFile();
            {
                PackageFileTemplate pf = new PackageFileTemplate()
                    .UseName(f.File.Name)
                    .UseAttributes(FileAttributes.Normal)
                    .WithAttributes(FileAttributes.Archive)
                    .WithoutAttributes(FileAttributes.System)
                    .UseTemplate(new TextFileTemplate("hello"));
                pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var.Name, f.File.DirectoryName)).Wait();
                f.File.Refresh();
                Assert.AreEqual(FileAttributes.Archive, f.File.Attributes & FileAttributes.Archive);
                Assert.AreEqual("hello", File.ReadAllText(f.File.FullName));
            }
            {
                PackageFileTemplate pf = new PackageFileTemplate();
                pf.FromText(f.File).Wait();
                pf.WithoutAttributes(FileAttributes.Archive);
                pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var.Name, f.File.DirectoryName)).Wait();
                f.File.Refresh();
                Assert.AreNotEqual(FileAttributes.Archive, f.File.Attributes & FileAttributes.Archive);
                Assert.AreEqual("hello", File.ReadAllText(f.File.FullName));
            }
            {
                PackageFileTemplate pf = new PackageFileTemplate();
                pf.FromBinary(f.File).Wait();
                pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var.Name, f.File.DirectoryName)).Wait();
                f.File.Refresh();
                Assert.AreEqual("hello", File.ReadAllText(f.File.FullName));
            }
        }

        [TestMethod]
        public void Directories()
        {
            using TempDirectory d = new TempDirectory();
            {
                PackageDirectoryTemplate pf = new PackageDirectoryTemplate()
                    .UseName("")
                    .UseAttributes(FileAttributes.Directory)
                    .WithAttributes(FileAttributes.Archive)
                    .WithoutAttributes(FileAttributes.System);
                pf.AddFile("a.txt").UseTemplate(new TextFileTemplate("hello"));
                pf.AddDirectory("subdir");
                pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var.Name, d.Directory.FullName)).Wait();
                d.Directory.Refresh();
                Assert.AreEqual(FileAttributes.Archive, d.Directory.Attributes & FileAttributes.Archive);
            }
            {
                PackageDirectoryTemplate pf = new PackageDirectoryTemplate();
                pf.From(d.Directory, true).Wait();
                pf.UseName().WithoutAttributes(FileAttributes.Archive);
                pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var.Name, d.Directory.FullName)).Wait();
                d.Directory.Refresh();
                Assert.AreNotEqual(FileAttributes.Archive, d.Directory.Attributes & FileAttributes.Archive);
                Assert.AreEqual("hello", File.ReadAllText(Path.Join(d.Directory.FullName, "a.txt")));
                Assert.IsTrue(Directory.Exists(Path.Join(d.Directory.FullName, "subdir")));
            }
            {
                PackageDirectoryTemplate pf = new PackageDirectoryTemplate();
                pf.From(d.Directory, false).Wait();
                pf.UseName().WithoutAttributes(FileAttributes.Archive);
                pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var.Name, d.Directory.FullName)).Wait();
                d.Directory.Refresh();
                Assert.AreNotEqual(FileAttributes.Archive, d.Directory.Attributes & FileAttributes.Archive);
                Assert.AreEqual("hello", File.ReadAllText(Path.Join(d.Directory.FullName, "a.txt")));
                Assert.IsTrue(Directory.Exists(Path.Join(d.Directory.FullName, "subdir")));
            }
        }
    }
}
