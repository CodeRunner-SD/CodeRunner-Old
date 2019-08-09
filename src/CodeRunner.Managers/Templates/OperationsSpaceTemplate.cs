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
                        $"\"hello {StringTemplate.GetVariableTemplate("name")}!\"",
                        new Variable[] { new Variable("name").NotRequired("world") }
                    )
                )
            );

            {
                var source = new StringTemplate(
                    StringTemplate.GetVariableTemplate(OperationVariables.InputPath.Name),
                        new Variable[] {
                            OperationVariables.InputPath
                        }
                );

                var output = new StringTemplate(
                    StringTemplate.GetVariableTemplate(OperationVariables.OutputPath.Name),
                        new Variable[] {
                            OperationVariables.OutputPath
                        }
                );

                AppendOperationTemplate("c", settings,
                    new CommandLineTemplate()
                        .UseCommand("gcc")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output)
                );

                AppendOperationTemplate("cpp", settings,
                    new CommandLineTemplate()
                        .UseCommand("g++")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output)
                );

                AppendOperationTemplate("csharp", settings,
                    new CommandLineTemplate()
                        .UseCommand("csc")
                        .UseArgument(source)
                        .UseArgument("-out")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output)
                );

                AppendOperationTemplate("python", settings,
                    new CommandLineTemplate()
                        .UseCommand("python")
                        .UseArgument(source)
                );

                AppendOperationTemplate("ruby", settings,
                    new CommandLineTemplate()
                        .UseCommand("ruby")
                        .UseArgument(source)
                );

                AppendOperationTemplate("go", settings,
                    new CommandLineTemplate()
                        .UseCommand("go")
                        .UseCommand("run")
                        .UseArgument(source)
                );

                AppendOperationTemplate("javascript", settings,
                    new CommandLineTemplate()
                        .UseCommand("node")
                        .UseArgument(source)
                );
            }

            Package.AddFile(Workspace.P_Settings).Template = new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(settings)));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            return Package.ResolveTo(context, path);
        }
    }
}
