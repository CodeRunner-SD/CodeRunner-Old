using CodeRunner.IO;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Test.Core.Templates
{
    [TestClass]
    public class TPackage
    {
        [TestMethod]
        public async Task Files()
        {
            using TempFile f = new TempFile();
            {
                PackageFileTemplate pf = new PackageFileTemplate()
                    .UseName(f.File.Name)
                    .UseAttributes(FileAttributes.Normal)
                    .WithAttributes(FileAttributes.Archive)
                    .WithoutAttributes(FileAttributes.System)
                    .UseTemplate(new TextFileTemplate("hello"));
                using (TempDirectory dir = new TempDirectory())
                {
                    dir.Directory.Delete();
                    await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, dir.Directory.FullName));
                }
                await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, f.File.DirectoryName));
                f.File.Refresh();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Assert.AreEqual(FileAttributes.Archive, f.File.Attributes & FileAttributes.Archive);
                }

                Assert.AreEqual("hello", File.ReadAllText(f.File.FullName));
            }
            {
                PackageFileTemplate pf = new PackageFileTemplate();
                await pf.FromText(f.File);
                pf.WithoutAttributes(FileAttributes.Archive);
                await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, f.File.DirectoryName));
                f.File.Refresh();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Assert.AreNotEqual(FileAttributes.Archive, f.File.Attributes & FileAttributes.Archive);
                }

                Assert.AreEqual("hello", File.ReadAllText(f.File.FullName));
            }
            {
                PackageFileTemplate pf = new PackageFileTemplate();
                await pf.FromBinary(f.File);
                await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, f.File.DirectoryName));
                f.File.Refresh();
                Assert.AreEqual("hello", File.ReadAllText(f.File.FullName));
            }
        }

        [TestMethod]
        public async Task Directories()
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
                await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, d.Directory.FullName));
                d.Directory.Refresh();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Assert.AreEqual(FileAttributes.Archive, d.Directory.Attributes & FileAttributes.Archive);
                }
            }
            {
                PackageDirectoryTemplate pf = new PackageDirectoryTemplate();
                await pf.From(d.Directory, true);
                pf.UseName().WithoutAttributes(FileAttributes.Archive);
                await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, d.Directory.FullName));
                d.Directory.Refresh();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Assert.AreNotEqual(FileAttributes.Archive, d.Directory.Attributes & FileAttributes.Archive);
                }

                Assert.AreEqual("hello", File.ReadAllText(Path.Join(d.Directory.FullName, "a.txt")));
                Assert.IsTrue(Directory.Exists(Path.Join(d.Directory.FullName, "subdir")));
            }
            {
                PackageDirectoryTemplate pf = new PackageDirectoryTemplate();
                await pf.From(d.Directory, false);
                pf.UseName().WithoutAttributes(FileAttributes.Archive);
                await pf.Resolve(new ResolveContext().WithVariable(DirectoryTemplate.Var, d.Directory.FullName));
                d.Directory.Refresh();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Assert.AreNotEqual(FileAttributes.Archive, d.Directory.Attributes & FileAttributes.Archive);
                }

                Assert.AreEqual("hello", File.ReadAllText(Path.Join(d.Directory.FullName, "a.txt")));
                Assert.IsTrue(Directory.Exists(Path.Join(d.Directory.FullName, "subdir")));
            }
        }
    }
}
