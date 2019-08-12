using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Operations
{
    public class AddCommand : ItemManagers.AddCommand<OperationManager, OperationsSettings, OperationItem, Package<Operation>?>
    {
        public override Task<OperationItem> GetItem(FileInfo file)
        {
            return Task.FromResult(new OperationItem
            {
                FileName = file.FullName
            });
        }

        public override Task<OperationManager> GetManager(PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Operations);
        }
    }
}
