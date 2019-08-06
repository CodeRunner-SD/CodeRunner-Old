using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Templates
{
    public class OperationsSpaceTemplate : DirectoryTemplate
    {
        private void AppendOperationTemplate(string name, OperationsSettings settings, params CommandLineTemplate[] items)
        {
            OperationItem item = new OperationItem
            {
                FileName = $"{name}.tpl",
            };

            Package.AddFile(item.FileName).Template = new TextFileTemplate(
                new StringTemplate(
                    JsonFormatter.Serialize(
                        new Operation(items)
                    )
                )
            );

            settings.Items.Add(name, item);
        }

        public OperationsSpaceTemplate()
        {
            OperationsSettings settings = new OperationsSettings();

            AppendOperationTemplate("hello", settings, new CommandLineTemplate()
                .UseCommand("echo")
                .UseArgument(
                    new StringTemplate(
                        $"'hello {StringTemplate.GetVariableTemplate("name")}!'",
                        new Variable[] { new Variable("name").NotRequired("world") }
                    )
                )
            );

            Package.AddFile(Workspace.P_Settings).Template = new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(settings)));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            return Package.ResolveTo(context, path);
        }
    }
}
