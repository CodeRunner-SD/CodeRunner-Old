using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Operations
{
    public class AddCommand : ItemManagers.AddCommand<IOperationManager, OperationSettings, Package<BaseOperation>>
    {
        public override async Task<Package<BaseOperation>> GetItem(FileInfo file)
        {
            using FileStream st = file.OpenRead();
            return await Package.Load<BaseOperation>(st);
        }

        public override Task<IOperationManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Operations);
        }
    }
}
