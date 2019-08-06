using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using Newtonsoft.Json;
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
                Type = TemplateType.PackageFile,
            };

            Package.AddFile(item.FileName).Template = new TextFileTemplate(
                new StringTemplate(
                    JsonConvert.SerializeObject(
                        new PackageFileTemplate(
                            new StringTemplate(
                                StringTemplate.GetVariableTemplate("name") + $".{ext}",
                                new Variable[] { new Variable("name").Required() }
                            )
                        ).UseTemplate(new TextFileTemplate(new StringTemplate(source))),
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        }
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

            Package.AddFile(Workspace.P_Settings).Template = new TextFileTemplate(new StringTemplate(
                JsonConvert.SerializeObject(settings, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                })));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            return Package.ResolveTo(context, path);
        }
    }
}
