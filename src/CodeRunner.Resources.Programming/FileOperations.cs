using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.Reflection;

namespace CodeRunner.Resources.Programming
{
    public static class FileOperations
    {
        private static Package<BaseOperation> Create(string name, params CommandLineTemplate[] items) => new Package<BaseOperation>(new SimpleCommandLineOperation(items))
        {
            Metadata = new PackageMetadata
            {
                Name = name,
                Author = nameof(CodeRunner),
                CreationTime = DateTimeOffset.Now,
                Version = Assembly.GetAssembly(typeof(FileOperations))?.GetName().Version ?? new Version()
            }
        };

        private static readonly StringTemplate source = new StringTemplate(
            StringTemplate.GetVariableTemplate(OperationVariables.VarInputPath.Name),
                new Variable[] {
                    OperationVariables.VarInputPath
                }
        );

        private static readonly StringTemplate output = new StringTemplate(
            StringTemplate.GetVariableTemplate(OperationVariables.VarOutputPath.Name),
                new Variable[] {
                    OperationVariables.VarOutputPath
                }
        );

        public static Package<BaseOperation> C => Create("c",
                    new CommandLineTemplate()
                        .UseCommand("gcc")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));

        public static Package<BaseOperation> Cpp => Create("cpp",
                    new CommandLineTemplate()
                        .UseCommand("g++")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));

        public static Package<BaseOperation> CSharp => Create("csharp",
                   new CommandLineTemplate()
                       .UseCommand("csc")
                       .UseArgument(source)
                       .UseArgument("-out")
                       .UseArgument(output),
                   new CommandLineTemplate()
                       .UseCommand(output));

        public static Package<BaseOperation> Python => Create("python",
                    new CommandLineTemplate()
                        .UseCommand("python")
                        .UseArgument(source));

        public static Package<BaseOperation> Ruby => Create("ruby",
                    new CommandLineTemplate()
                        .UseCommand("ruby")
                        .UseArgument(source));

        public static Package<BaseOperation> Go => Create("go",
                    new CommandLineTemplate()
                        .UseCommand("go")
                        .UseCommand("run")
                        .UseArgument(source));

        public static Package<BaseOperation> JavaScript => Create("javascript",
                    new CommandLineTemplate()
                        .UseCommand("node")
                        .UseArgument(source));
    }
}
