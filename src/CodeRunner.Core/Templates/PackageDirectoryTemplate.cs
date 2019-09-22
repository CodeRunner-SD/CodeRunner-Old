using CodeRunner.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class PackageDirectoryTemplate : DirectoryTemplate
    {
        public PackageDirectoryTemplate(StringTemplate? name = null) => Name = name ?? new StringTemplate("");

        public PackageDirectoryTemplate() : this(null)
        {
        }

        public StringTemplate Name { get; set; }

        public FileAttributes Attributes { get; set; } = FileAttributes.Directory;

        public IList<PackageDirectoryTemplate> Directories { get; } = new List<PackageDirectoryTemplate>();

        public IList<PackageFileTemplate> Files { get; } = new List<PackageFileTemplate>();

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Collect(Name);
            foreach (PackageDirectoryTemplate item in Directories)
            {
                res.Collect(item);
            }

            foreach (PackageFileTemplate item in Files)
            {
                res.Collect(item);
            }

            return res;
        }

        public override async Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            Assert.IsNotNull(context);
            Assert.IsNotNull(path);

            string realPath;
            string name = await Name.Resolve(context).ConfigureAwait(false);
            realPath = string.IsNullOrEmpty(name) ? path : Path.Join(path, name);

            DirectoryInfo res = new DirectoryInfo(realPath);
            if (!res.Exists)
            {
                res.Create();
                res.Refresh();
            }


            res.Attributes = Attributes;

            foreach (PackageFileTemplate f in Files)
            {
                _ = await f.ResolveTo(context, res.FullName).ConfigureAwait(false);
            }

            foreach (PackageDirectoryTemplate f in Directories)
            {
                _ = await f.ResolveTo(context, res.FullName).ConfigureAwait(false);
            }

            return res;
        }

        public PackageDirectoryTemplate UseAttributes(FileAttributes attributes)
        {
            Attributes = attributes;
            return this;
        }

        public PackageDirectoryTemplate WithAttributes(FileAttributes attributes)
        {
            Attributes |= attributes;
            return this;
        }

        public PackageDirectoryTemplate WithoutAttributes(FileAttributes attributes)
        {
            Attributes &= ~attributes;
            return this;
        }

        public PackageFileTemplate AddFile(StringTemplate? name = null)
        {
            PackageFileTemplate res = new PackageFileTemplate(name);
            Files.Add(res);
            return res;
        }

        public PackageDirectoryTemplate AddDirectory(StringTemplate? name = null)
        {
            PackageDirectoryTemplate res = new PackageDirectoryTemplate(name);
            Directories.Add(res);
            return res;
        }

        public PackageDirectoryTemplate UseName(StringTemplate? name = null)
        {
            Name = name ?? new StringTemplate("");
            return this;
        }

        public async Task From(DirectoryInfo dir, bool asText = false)
        {
            Assert.IsNotNull(dir);

            _ = UseName(dir.Name).UseAttributes(dir.Attributes);
            foreach (FileInfo file in dir.GetFiles())
            {
                PackageFileTemplate f = AddFile();
                if (asText)
                {
                    await f.FromText(file).ConfigureAwait(false);
                }
                else
                {
                    await f.FromBinary(file).ConfigureAwait(false);
                }
            }
            foreach (DirectoryInfo file in dir.GetDirectories())
            {
                await AddDirectory().From(file, asText).ConfigureAwait(false);
            }
        }
    }
}
