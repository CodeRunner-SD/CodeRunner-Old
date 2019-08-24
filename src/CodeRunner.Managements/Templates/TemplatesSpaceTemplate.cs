using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.Templates
{
    public class TemplatesSpaceTemplate : DirectoryTemplate
    {
        public TemplatesSpaceTemplate()
        {
            TemplatesSettings settings = new TemplatesSettings();

            Package.AddFile(Workspace.P_Settings).UseTemplate(new TextFileTemplate(new StringTemplate(
                JsonFormatter.Serialize(settings))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            return Package.ResolveTo(context, path);
        }
    }
}
