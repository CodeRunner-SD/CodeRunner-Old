using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Templates
{
    public class WorkspaceTemplate : DirectoryTemplate
    {
        public WorkspaceTemplate()
        {
            PackageDirectoryTemplate crRoot = Package.AddDirectory(Workspace.P_CRRoot).WithAttributes(FileAttributes.Hidden);
            crRoot.AddDirectory(Workspace.P_TemplatesRoot);
            crRoot.AddFile(Workspace.P_Settings).UseTemplate(new TextFileTemplate(new StringTemplate(JsonConvert.SerializeObject(new AppSettings
            {
                Version = new Version(0, 0, 1, 0)
            }, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path)
        {
            return Package.ResolveTo(context, path);
        }
    }
}
