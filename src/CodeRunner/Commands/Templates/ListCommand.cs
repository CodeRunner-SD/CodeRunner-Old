using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.Threading.Tasks;
using SettingItem = System.Tuple<string, CodeRunner.Managements.Configurations.TemplateItem, CodeRunner.Packagings.Package<CodeRunner.Templates.BaseTemplate>?>;

namespace CodeRunner.Commands.Templates
{
    public class ListCommand : ItemManagers.ListCommand<TemplateManager, TemplatesSettings, TemplateItem, Package<BaseTemplate>?>
    {
        public override Task<TemplateManager> GetManager(PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.Get<Workspace>();
            return Task.FromResult(workspace.Templates);
        }

        public override async Task RenderItems(ITerminal terminal, IDictionary<string, TemplateItem> items, PipelineContext pipeline)
        {
            Workspace workspace = pipeline.Services.Get<Workspace>();
            List<SettingItem> sources = new List<SettingItem>();
            foreach (string v in items.Keys)
            {
                TemplateItem value = (await workspace.Templates.Get(v))!;
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
