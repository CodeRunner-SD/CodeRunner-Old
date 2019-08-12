using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Operations
{
    public class RemoveCommand : ItemManagers.RemoveCommand<OperationManager, OperationsSettings, OperationItem, Package<Operation>?>
    {
        public override Task<OperationManager> GetManager(PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Operations);
        }
    }
}
