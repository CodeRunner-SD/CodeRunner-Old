using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Templates
{
    public class AddCommand : ItemManagers.AddCommand<ITemplateManager, TemplateSettings, Package<BaseTemplate>>
    {
        public override async Task<Package<BaseTemplate>> GetItem(FileInfo file)
        {
            using FileStream st = file.OpenRead();
            return await Package.Load<BaseTemplate>(st);
        }

        public override Task<ITemplateManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }
    }
}
