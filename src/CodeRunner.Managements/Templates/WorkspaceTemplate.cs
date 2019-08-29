using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CodeRunner.Managements.Templates
{
    public class WorkspaceTemplate : DirectoryTemplate
    {
        public static readonly PackageMetadata BuiltinTemplateMetadata = new PackageMetadata
        {
            Author = "CodeRunner",
            CreationTime = DateTimeOffset.Now,
            Version = Assembly.GetAssembly(typeof(Workspace))?.GetName().Version ?? new Version()
        };

        public WorkspaceTemplate()
        {
            PackageDirectoryTemplate crRoot = Package.AddDirectory(Workspace.P_CRRoot).WithAttributes(FileAttributes.Hidden);
            _ = crRoot.AddDirectory(Workspace.P_TemplatesRoot);
            _ = crRoot.AddFile(Workspace.P_Settings).UseTemplate(new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(new WorkspaceSettings
            {
                Version = new Version(0, 0, 1, 0)
            }))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
