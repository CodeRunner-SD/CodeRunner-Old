using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased.Templates
{
    public class TemplatesSpaceTemplate : DirectoryTemplate
    {
        public TemplatesSpaceTemplate()
        {
            TemplateSettings settings = new TemplateSettings();

            _ = Package.AddFile(Workspace.PSettings).UseTemplate(new TextFileTemplate(new StringTemplate(
                JsonFormatter.Serialize(settings))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
