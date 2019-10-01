using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Templates
{
    public class ListCommand : ItemManagers.ListCommand<ITemplateManager, TemplateSettings, Package<BaseTemplate>>
    {
        public override Task<ITemplateManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }

        public override async Task RenderItems(ITerminal terminal, IAsyncEnumerable<(string, Package<BaseTemplate>?)> items, PipelineContext pipeline)
        {
            List<(string, Package<BaseTemplate>?)> sources = new List<(string, Package<BaseTemplate>?)>();
            await foreach ((string, Package<BaseTemplate>?) v in items)
                sources.Add(v);
            terminal.OutputTable(sources,
                new OutputTableColumnStringView<(string, Package<BaseTemplate>?)>(x => x.Item1, "Name"),
                new OutputTableColumnStringView<(string, Package<BaseTemplate>?)>(x => x.Item2?.Metadata?.Name ?? "N/A", nameof(PackageMetadata.Name)),
                new OutputTableColumnStringView<(string, Package<BaseTemplate>?)>(x => x.Item2?.Metadata?.Author ?? "N/A", nameof(PackageMetadata.Author)),
                new OutputTableColumnStringView<(string, Package<BaseTemplate>?)>(x => x.Item2?.Metadata?.CreationTime.ToString() ?? "N/A", nameof(PackageMetadata.CreationTime)),
                new OutputTableColumnStringView<(string, Package<BaseTemplate>?)>(x => x.Item2?.Metadata?.Version.ToString() ?? "N/A", nameof(PackageMetadata.Version))
            );
        }
    }
}
