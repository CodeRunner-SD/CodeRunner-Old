using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.Reflection;

namespace CodeRunner.Resources.Programming
{
    public static class Templates
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
                    Version = Assembly.GetAssembly(typeof(Templates))?.GetName().Version ?? new Version()
                }
            };
        }

        public static Package<BaseTemplate> C => CreateSingleFile(nameof(C).ToLower(), "c", Properties.Resources.tpl_c);
        public static Package<BaseTemplate> Cpp => CreateSingleFile(nameof(Cpp).ToLower(), "cpp", Properties.Resources.tpl_cpp);
        public static Package<BaseTemplate> CSharp => CreateSingleFile(nameof(CSharp).ToLower(), "cs", Properties.Resources.tpl_csharp);
        public static Package<BaseTemplate> Python => CreateSingleFile(nameof(Python).ToLower(), "py", Properties.Resources.tpl_python);
        public static Package<BaseTemplate> FSharp => CreateSingleFile(nameof(FSharp).ToLower(), "fs", Properties.Resources.tpl_fsharp);
        public static Package<BaseTemplate> Go => CreateSingleFile(nameof(Go).ToLower(), "go", Properties.Resources.tpl_go);
        public static Package<BaseTemplate> Java => CreateSingleFile(nameof(Java).ToLower(), "java", Properties.Resources.tpl_java);
    }
}
