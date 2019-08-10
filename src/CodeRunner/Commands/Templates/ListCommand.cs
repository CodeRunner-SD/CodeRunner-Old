using CodeRunner.Managers;
using CodeRunner.Managers.Configurations;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;
using SettingItem = System.Tuple<string, CodeRunner.Managers.Configurations.TemplateItem, CodeRunner.Templates.BaseTemplate?>;

namespace CodeRunner.Commands.Templates
{
    public class ListCommand : BaseCommand<ListCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("list", "List all.");
            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            Workspace workspace = operation.Services.Get<Workspace>();
            TemplatesSettings? res = await workspace.Templates.Settings;
            if (res != null)
            {
                List<SettingItem> sources = new List<SettingItem>();
                foreach (string v in res.Items.Keys)
                {
                    TemplateItem value = (await workspace.Templates.Get(v))!;
                    sources.Add(new SettingItem(v, value, await value.Value));
                }
                terminal.OutputTable(sources,
                    new OutputTableColumnStringView<SettingItem>(x => x.Item1, "Name"),
                    new OutputTableColumnStringView<SettingItem>(x => x.Item2.FileName, "File"),
                    new OutputTableColumnStringView<SettingItem>(x => x.Item3?.Metadata?.Author ?? "N/A", nameof(TemplateMetadata.Author)),
                    new OutputTableColumnStringView<SettingItem>(x => x.Item3?.Metadata?.CreationTime.ToString() ?? "N/A", nameof(TemplateMetadata.CreationTime)),
                    new OutputTableColumnStringView<SettingItem>(x => x.Item3?.Metadata?.Version.ToString() ?? "N/A", nameof(TemplateMetadata.Version))
                );
            }
            return 0;
        }

        public class CArgument
        {
        }
    }
}
