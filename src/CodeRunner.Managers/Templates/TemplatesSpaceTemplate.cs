using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Templates
{
    public class TemplatesSpaceTemplate : DirectoryTemplate
    {
        TextFileTemplate BuildTextFilePackage(TextFileTemplate template)
        {
            return new TextFileTemplate(JsonSerializer.Serialize(template));
        }

        public TemplatesSpaceTemplate() : base(null)
        {
            var settings = new TemplatesSettings();
            {
                var item = new TemplateItem
                {
                    FileName = "c.tpl",
                    Type = TemplateType.TextFile,
                };
                Package.AddFile(item.FileName).Template = BuildTextFilePackage(new TextFileTemplate(new StringTemplate(
                    Properties.Resources.tpl_c
                )));
                settings.Items.Add("c", item);
            }
            {
                var item = new TemplateItem
                {
                    FileName = "cpp.tpl",
                    Type = TemplateType.TextFile,
                };
                Package.AddFile(item.FileName).Template = BuildTextFilePackage(new TextFileTemplate(new StringTemplate(
                    Properties.Resources.tpl_cpp
                )));
                settings.Items.Add("cpp", item);
            }
            {
                var item = new TemplateItem
                {
                    FileName = "csharp.tpl",
                    Type = TemplateType.TextFile,
                };
                Package.AddFile(item.FileName).Template = BuildTextFilePackage(new TextFileTemplate(new StringTemplate(
                    Properties.Resources.tpl_csharp
                )));
                settings.Items.Add("csharp", item);
            }

            Package.AddFile(Workspace.P_Settings).Template = new TextFileTemplate(new StringTemplate(JsonSerializer.Serialize<TemplatesSettings>(settings)));
        }

        PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(TemplateResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
