using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased.Templates
{
    public class OperationsSpaceTemplate : DirectoryTemplate
    {
        public static Package<BaseOperation> Hello => new Package<BaseOperation>(new SimpleCommandLineOperation(
            new CommandLineTemplate[]{
                new CommandLineTemplate()
                .UseCommand("echo")
                .UseArgument(
                    new StringTemplate(
                        $"\"hello {StringTemplate.GetVariableTemplate("name")}!\"",
                        new Variable[] { new Variable("name").NotRequired("world") }
                    )
                )
            }))
        {
            Metadata = new PackageMetadata
            {
                Name = "hello",
                Author = nameof(CodeRunner),
                CreationTime = DateTimeOffset.Now,
                Version = Assembly.GetAssembly(typeof(OperationsSpaceTemplate))?.GetName().Version ?? new Version()
            }
        };

        public OperationsSpaceTemplate()
        {
            OperationSettings settings = new OperationSettings();

            _ = Package.AddFile(Workspace.PSettings)
                .UseTemplate(new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(settings))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
