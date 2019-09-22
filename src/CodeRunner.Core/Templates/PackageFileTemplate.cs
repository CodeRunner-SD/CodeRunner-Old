using CodeRunner.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class PackageFileTemplate : DirectoryTemplate
    {
        public PackageFileTemplate(StringTemplate? name = null) => Name = name ?? new StringTemplate("");

        public PackageFileTemplate() : this(null)
        {
        }

        public StringTemplate Name { get; set; }

        public FileAttributes Attributes { get; set; }

        public FileTemplate? Template { get; set; }

        public override async Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            Assert.IsNotNull(context);
            Assert.IsNotNull(path);

            DirectoryInfo res = new DirectoryInfo(path);
            if (!res.Exists)
            {
                res.Create();
                res.Refresh();
            }

            if (Template != null)
            {
                string name = await Name.Resolve(context).ConfigureAwait(false);
                FileInfo file = await Template.ResolveTo(context, Path.Join(res.FullName, name)).ConfigureAwait(false);
                file.Refresh();
                file.Attributes = Attributes;
            }
            return res;
        }

        public PackageFileTemplate UseName(StringTemplate? name = null)
        {
            Name = name ?? new StringTemplate("");
            return this;
        }

        public PackageFileTemplate UseTemplate(FileTemplate? template = null)
        {
            Template = template;
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
            Assert.IsNotNull(file);

            _ = UseName(file.Name).UseAttributes(file.Attributes)
                .UseTemplate(new BinaryFileTemplate(await File.ReadAllBytesAsync(file.FullName).ConfigureAwait(false)));
        }

        public async Task FromText(FileInfo file)
        {
            Assert.IsNotNull(file);

            _ = UseName(file.Name).UseAttributes(file.Attributes)
                .UseTemplate(new TextFileTemplate(await File.ReadAllTextAsync(file.FullName).ConfigureAwait(false)));
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Collect(Name);
            if (Template != null)
            {
                res.Collect(Template);
                _ = res.Remove(FileTemplate.Var);
            }
            return res;
        }
    }
}
