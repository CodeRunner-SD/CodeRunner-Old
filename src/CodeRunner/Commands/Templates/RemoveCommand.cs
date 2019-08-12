using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Templates
{
    public class RemoveCommand : ItemManagers.RemoveCommand<TemplateManager, TemplatesSettings, TemplateItem, Package<BaseTemplate>?>
    {
        public override Task<TemplateManager> GetManager(PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.Get<Workspace>();
            return Task.FromResult(workspace.Templates);
        }
    }
}
