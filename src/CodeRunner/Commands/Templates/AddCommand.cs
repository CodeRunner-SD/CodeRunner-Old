using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Templates
{
    public class AddCommand : ItemManagers.AddCommand<TemplateManager, TemplatesSettings, TemplateItem, Package<BaseTemplate>?>
    {
        public override Task<TemplateItem> GetItem(FileInfo file)
        {
            return Task.FromResult(new TemplateItem
            {
                FileName = file.FullName
            });
        }

        public override Task<TemplateManager> GetManager(PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }
    }
}
