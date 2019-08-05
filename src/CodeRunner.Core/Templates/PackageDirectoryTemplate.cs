using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class PackageDirectoryTemplate : DirectoryTemplate
    {
        public PackageDirectoryTemplate(string name = "", string[]? variables = null) : base(variables)
        {
            Name = name;
        }

        public PackageDirectoryTemplate() : this("", null)
        {
        }

        public string Name { get; set; }

        public FileAttributes Attributes { get; set; } = FileAttributes.Directory;

        public IList<PackageDirectoryTemplate> Directories { get; set; } = new List<PackageDirectoryTemplate>();

        public IList<PackageFileTemplate> Files { get; set; } = new List<PackageFileTemplate>();

        public override async Task<DirectoryInfo> ResolveTo(TemplateResolveContext context, string path)
        {
            string realPath;
            if (string.IsNullOrEmpty(Name)) realPath = path;
            else realPath = Path.Join(path, Name);
            var res = new DirectoryInfo(realPath);
            if (!res.Exists) res.Create();
            res.Attributes = Attributes;
            foreach (var f in Files)
                await f.ResolveTo(context, res.FullName);
            foreach (var f in Directories)
                await f.ResolveTo(context, res.FullName);
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

        public PackageFileTemplate AddFile(string name = "")
        {
            var res = new PackageFileTemplate(name);
            Files.Add(res);
            return res;
        }

        public PackageDirectoryTemplate AddDirectory(string name = "")
        {
            var res = new PackageDirectoryTemplate(name);
            Directories.Add(res);
            return res;
        }

        public PackageDirectoryTemplate UseName(string name = "")
        {
            Name = name;
            return this;
        }

        public async Task From(DirectoryInfo dir, bool asText = false)
        {
            UseName(dir.Name).UseAttributes(dir.Attributes);
            foreach (var file in dir.GetFiles())
            {
                var f = AddFile();
                if (asText) await f.FromText(file);
                else await f.FromBinary(file);
            }
            foreach (var file in dir.GetDirectories())
            {
                await AddDirectory().From(file, asText);
            }
        }
    }
}
