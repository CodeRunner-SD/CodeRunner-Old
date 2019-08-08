using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Templates
{
    public class TemplatesSpaceTemplate : DirectoryTemplate
    {
        private void AppendCodeFileTemplate(string name, string ext, string source, TemplatesSettings settings)
        {
            TemplateItem item = new TemplateItem
            {
                FileName = $"{name}.tpl",
                Type = TemplateType.File,
            };

            Package.AddFile(item.FileName).Template = new TextFileTemplate(
                new StringTemplate(
                    JsonFormatter.Serialize(
                        new PackageFileTemplate(
                            new StringTemplate(
                                StringTemplate.GetVariableTemplate("name") + $".{ext}",
                                new Variable[] { new Variable("name").Required() }
                            )
                        ).UseTemplate(new TextFileTemplate(new StringTemplate(source)))
                    )
                )
            );

            settings.Items.Add(name, item);
        }

        public TemplatesSpaceTemplate()
        {
            TemplatesSettings settings = new TemplatesSettings();

            AppendCodeFileTemplate("c", "c", Properties.Resources.tpl_c, settings);
            AppendCodeFileTemplate("cpp", "cpp", Properties.Resources.tpl_cpp, settings);
            AppendCodeFileTemplate("csharp", "cs", Properties.Resources.tpl_csharp, settings);
            AppendCodeFileTemplate("python", "py", Properties.Resources.tpl_python, settings);
            AppendCodeFileTemplate("fsharp", "fs", Properties.Resources.tpl_fsharp, settings);
            AppendCodeFileTemplate("go", "go", Properties.Resources.tpl_go, settings);
            AppendCodeFileTemplate("java", "java", Properties.Resources.tpl_java, settings);

            Package.AddFile(Workspace.P_Settings).Template = new TextFileTemplate(new StringTemplate(
                JsonFormatter.Serialize(settings)));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            return Package.ResolveTo(context, path);
        }
    }
}
