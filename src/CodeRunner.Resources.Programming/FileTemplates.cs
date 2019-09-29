using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.Reflection;

namespace CodeRunner.Resources.Programming
{
    public static class FileTemplates
    {
        private static Package<BaseTemplate> CreateSingleFile(string name, string ext, string source)
        {
            return new Package<BaseTemplate>(new PackageFileTemplate(
                new StringTemplate(
                    StringTemplate.GetVariableTemplate("name") + $".{ext}",
                    new Variable[] { new Variable("name").Required() }
                )
            ).UseTemplate(new TextFileTemplate(new StringTemplate(source))))
            {
                Metadata = new PackageMetadata
                {
                    Name = name,
                    Author = nameof(CodeRunner),
                    CreationTime = DateTimeOffset.Now,
                    Version = Assembly.GetAssembly(typeof(FileTemplates))?.GetName().Version ?? new Version()
                }
            };
        }

        public static Package<BaseTemplate> C => CreateSingleFile("c", "c", Properties.Resources.tpl_c);
        public static Package<BaseTemplate> Cpp => CreateSingleFile("cpp", "cpp", Properties.Resources.tpl_cpp);
        public static Package<BaseTemplate> CSharp => CreateSingleFile("csharp", "cs", Properties.Resources.tpl_csharp);
        public static Package<BaseTemplate> Python => CreateSingleFile("python", "py", Properties.Resources.tpl_python);
        public static Package<BaseTemplate> FSharp => CreateSingleFile("fsharp", "fs", Properties.Resources.tpl_fsharp);
        public static Package<BaseTemplate> Go => CreateSingleFile("go", "go", Properties.Resources.tpl_go);
        public static Package<BaseTemplate> Java => CreateSingleFile("java", "java", Properties.Resources.tpl_java);
    }
}
