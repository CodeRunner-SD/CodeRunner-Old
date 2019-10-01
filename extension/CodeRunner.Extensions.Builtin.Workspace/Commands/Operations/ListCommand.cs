using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Operations
{
    public class ListCommand : ItemManagers.ListCommand<IOperationManager, OperationSettings, Package<BaseOperation>>
    {
        public override Task<IOperationManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Operations);
        }

        public override async Task RenderItems(ITerminal terminal, IAsyncEnumerable<(string, Package<BaseOperation>?)> items, PipelineContext pipeline)
        {
            List<(string, Package<BaseOperation>?)> sources = new List<(string, Package<BaseOperation>?)>();
            await foreach ((string, Package<BaseOperation>?) v in items)
                sources.Add(v);
            terminal.OutputTable(sources,
                new OutputTableColumnStringView<(string, Package<BaseOperation>?)>(x => x.Item1, "Name"),
                new OutputTableColumnStringView<(string, Package<BaseOperation>?)>(x => x.Item2?.Metadata?.Name ?? "N/A", nameof(PackageMetadata.Name)),
                new OutputTableColumnStringView<(string, Package<BaseOperation>?)>(x => x.Item2?.Metadata?.Author ?? "N/A", nameof(PackageMetadata.Author)),
                new OutputTableColumnStringView<(string, Package<BaseOperation>?)>(x => x.Item2?.Metadata?.CreationTime.ToString() ?? "N/A", nameof(PackageMetadata.CreationTime)),
                new OutputTableColumnStringView<(string, Package<BaseOperation>?)>(x => x.Item2?.Metadata?.Version.ToString() ?? "N/A", nameof(PackageMetadata.Version))
            );
        }
    }
}
