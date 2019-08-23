using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.Reflection;

namespace CodeRunner.Resources.Programming
{
    public static class Operations
    {
        private static Package<BaseOperation> Create(string name, params CommandLineTemplate[] items)
        {
            return new Package<BaseOperation>(new SimpleCommandLineOperation(items))
            {
                Metadata = new PackageMetadata
                {
                    Name = name,
                    Author = nameof(CodeRunner),
                    CreationTime = DateTimeOffset.Now,
                    Version = Assembly.GetAssembly(typeof(Operations))?.GetName().Version ?? new Version()
                }
            };
        }

        private static readonly StringTemplate source = new StringTemplate(
            StringTemplate.GetVariableTemplate(OperationVariables.InputPath.Name),
                new Variable[] {
                    OperationVariables.InputPath
                }
        );

        private static readonly StringTemplate output = new StringTemplate(
            StringTemplate.GetVariableTemplate(OperationVariables.OutputPath.Name),
                new Variable[] {
                    OperationVariables.OutputPath
                }
        );

        public static Package<BaseOperation> C
        {
            get => Create(nameof(C).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("gcc")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));
        }

        public static Package<BaseOperation> Cpp
        {
            get => Create(nameof(Cpp).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("g++")
                        .UseArgument(source)
                        .UseArgument("-Wall")
                        .UseArgument("-o")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));
        }

        public static Package<BaseOperation> CSharp
        {
            get => Create(nameof(CSharp).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("csc")
                        .UseArgument(source)
                        .UseArgument("-out")
                        .UseArgument(output),
                    new CommandLineTemplate()
                        .UseCommand(output));
        }

        public static Package<BaseOperation> Python
        {
            get => Create(nameof(Python).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("python")
                        .UseArgument(source));
        }

        public static Package<BaseOperation> Ruby
        {
            get => Create(nameof(Ruby).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("ruby")
                        .UseArgument(source));
        }

        public static Package<BaseOperation> Go
        {
            get => Create(nameof(Go).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("go")
                        .UseCommand("run")
                        .UseArgument(source));
        }

        public static Package<BaseOperation> JavaScript
        {
            get => Create(nameof(JavaScript).ToLower(),
                    new CommandLineTemplate()
                        .UseCommand("node")
                        .UseArgument(source));
        }


    }
}
