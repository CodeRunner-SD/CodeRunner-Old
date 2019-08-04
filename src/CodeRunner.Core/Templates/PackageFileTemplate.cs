using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class PackageFileTemplate : DirectoryTemplate
    {
        public PackageFileTemplate(string name = "") : base(null)
        {
            Name = name;
        }

        public string Name { get; set; }

        public FileAttributes Attributes { get; set; }

        public FileTemplate? Template { get; set; }

        public override async Task<DirectoryInfo> ResolveTo(TemplateResolveContext context, string path)
        {
            var res = new DirectoryInfo(path);
            if (!res.Exists) res.Create();
            if (Template != null)
            {
                var file = await Template!.ResolveTo(context, Path.Join(res.FullName, Name));
                file.Attributes = Attributes;
            }
            return res;
        }

        public PackageFileTemplate UseName(string name = "")
        {
            Name = name;
            return this;
        }

        public PackageFileTemplate UseAttributes(FileAttributes attributes)
        {
            Attributes = attributes;
            return this;
        }

        public PackageFileTemplate WithAttributes(FileAttributes attributes)
        {
            Attributes |= attributes;
            return this;
        }

        public PackageFileTemplate WithoutAttributes(FileAttributes attributes)
        {
            Attributes &= ~attributes;
            return this;
        }

        public async Task FromBinary(FileInfo file)
        {
            UseName(file.Name).UseAttributes(file.Attributes);
            Template = new BinaryFileTemplate(await File.ReadAllBytesAsync(file.FullName));
        }

        public async Task FromText(FileInfo file)
        {
            UseName(file.Name).UseAttributes(file.Attributes);
            Template = new TextFileTemplate(await File.ReadAllTextAsync(file.FullName));
        }
    }
}
