using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Templates
{
    public class WorkspaceTemplate : DirectoryTemplate
    {
        public WorkspaceTemplate() : base(null)
        {
            var crRoot = Package.AddDirectory(Workspace.P_CRRoot).WithAttributes(FileAttributes.Hidden);
            crRoot.AddDirectory(Workspace.P_TemplatesRoot);
            crRoot.AddFile(Workspace.P_Settings).Template = new TextFileTemplate(new StringTemplate(JsonSerializer.Serialize<AppSettings>(new AppSettings
            {
                Version = new Version(0, 0, 1, 0)
            })));
        }

        PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(TemplateResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
