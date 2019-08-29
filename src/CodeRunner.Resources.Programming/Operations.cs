using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.Reflection;

namespace CodeRunner.Resources.Programming
{
    public static class Operations
    {
        private static Package<BaseOperation> Create(string name, params CommandLineTemplate[] items) => new Package<BaseOperation>(new SimpleCommandLineOperation(items))
        {
            Metadata = new PackageMetadata
            {
                Name = name,
                Author = nameof(CodeRunner),
                CreationTime = DateTimeOffset.Now,
                Version = Assembly.GetAssembly(typeof(Operations))?.GetName().Version ?? new Version()
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

        public static Package<BaseOperation> C => Create(nameof(C).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("gcc")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));

        public static Package<BaseOperation> Cpp => Create(nameof(Cpp).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("g++")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));

        public static Package<BaseOperation> CSharp => Create(nameof(CSharp).ToLower(),
                   new CommandLineTemplate()
                       .UseCommand("csc")
                       .UseArgument(source)
                       .UseArgument("-out")
                       .UseArgument(output),
                   new CommandLineTemplate()
                       .UseCommand(output));

        public static Package<BaseOperation> Python => Create(nameof(Python).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("python")
                        .UseArgument(source));

        public static Package<BaseOperation> Ruby => Create(nameof(Ruby).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("ruby")
                        .UseArgument(source));

        public static Package<BaseOperation> Go => Create(nameof(Go).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("go")
                        .UseCommand("run")
                        .UseArgument(source));

        public static Package<BaseOperation> JavaScript => Create(nameof(JavaScript).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("node")
                        .UseArgument(source));
    }
}
