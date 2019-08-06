using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class PackageDirectoryTemplate : DirectoryTemplate
    {
        public PackageDirectoryTemplate(StringTemplate? name = null)
        {
            Name = name ?? new StringTemplate("");
        }

        public PackageDirectoryTemplate() : this(null)
        {
        }

        public StringTemplate Name { get; set; }

        public FileAttributes Attributes { get; set; } = FileAttributes.Directory;

        public IList<PackageDirectoryTemplate> Directories { get; set; } = new List<PackageDirectoryTemplate>();

        public IList<PackageFileTemplate> Files { get; set; } = new List<PackageFileTemplate>();

        public override async Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            string realPath;
            string name = await Name.Resolve(context);
            if (string.IsNullOrEmpty(name))
            {
                realPath = path;
            }
            else
            {
                realPath = Path.Join(path, name);
            }

            DirectoryInfo res = new DirectoryInfo(realPath);
            if (!res.Exists)
            {
                res.Create();
            }

            res.Attributes = Attributes;
            foreach (PackageFileTemplate f in Files)
            {
                await f.ResolveTo(context, res.FullName);
            }

            foreach (PackageDirectoryTemplate f in Directories)
            {
                await f.ResolveTo(context, res.FullName);
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
            UseName(dir.Name).UseAttributes(dir.Attributes);
            foreach (FileInfo file in dir.GetFiles())
            {
                PackageFileTemplate f = AddFile();
                if (asText)
                {
                    await f.FromText(file);
                }
                else
                {
                    await f.FromBinary(file);
                }
            }
            foreach (DirectoryInfo file in dir.GetDirectories())
            {
                await AddDirectory().From(file, asText);
            }
        }
    }
}
