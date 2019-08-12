using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.Threading.Tasks;
using SettingItem = System.Tuple<string, CodeRunner.Managements.Configurations.OperationItem, CodeRunner.Packagings.Package<CodeRunner.Operations.Operation>?>;

namespace CodeRunner.Commands.Operations
{
    public class ListCommand : ItemManagers.ListCommand<OperationManager, OperationsSettings, OperationItem, Package<Operation>?>
    {
        public override Task<OperationManager> GetManager(PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.Get<Workspace>();
            return Task.FromResult(workspace.Operations);
        }

        public override async Task RenderItems(ITerminal terminal, IDictionary<string, OperationItem> items, PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.Get<Workspace>();
            List<SettingItem> sources = new List<SettingItem>();
            foreach (string v in items.Keys)
            {
                OperationItem value = (await workspace.Operations.Get(v))!;
                sources.Add(new SettingItem(v, value, await value.Value));
            }
            terminal.OutputTable(sources,
                new OutputTableColumnStringView<SettingItem>(x => x.Item1, "Name"),
                new OutputTableColumnStringView<SettingItem>(x => x.Item2.FileName, "File"),
                new OutputTableColumnStringView<SettingItem>(x => x.Item3?.Metadata?.Author ?? "N/A", nameof(PackageMetadata.Author)),
                new OutputTableColumnStringView<SettingItem>(x => x.Item3?.Metadata?.CreationTime.ToString() ?? "N/A", nameof(PackageMetadata.CreationTime)),
                new OutputTableColumnStringView<SettingItem>(x => x.Item3?.Metadata?.Version.ToString() ?? "N/A", nameof(PackageMetadata.Version))
            );
        }
    }
}
