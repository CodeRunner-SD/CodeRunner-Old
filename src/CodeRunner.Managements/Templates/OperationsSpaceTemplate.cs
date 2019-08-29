using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.Templates
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
                        new Package<BaseOperation>(new SimpleCommandLineOperation(items))
                        {
                            Metadata = WorkspaceTemplate.BuiltinTemplateMetadata
                        }
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
                        $"\"hello {StringTemplate.GetVariableTemplate("name")}!\"",
                        new Variable[] { new Variable("name").NotRequired("world") }
                    )
                )
            );

            _ = Package.AddFile(Workspace.P_Settings)
                .UseTemplate(new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(settings))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
