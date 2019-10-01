using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Operations
{
    public class RemoveCommand : ItemManagers.RemoveCommand<IOperationManager, OperationSettings, Package<BaseOperation>>
    {
        public override Task<IOperationManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Operations);
        }
    }
}
