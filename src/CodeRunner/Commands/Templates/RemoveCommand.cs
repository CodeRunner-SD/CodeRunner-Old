using CodeRunner.Managements;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Templates
{
    public class RemoveCommand : ItemManagers.RemoveCommand<ITemplateManager, TemplateSettings, Package<BaseTemplate>>
    {
        public override Task<ITemplateManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }
    }
}
