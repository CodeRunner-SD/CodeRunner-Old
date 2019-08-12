using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Pipelines;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands.ItemManagers
{
    public static class ListCommand
    {
        public class CArgument
        {
        }
    }

    public abstract class ListCommand<TItemManager, TSettings, TItem, TValue> : BaseItemCommand<ListCommand.CArgument, TItemManager, TSettings, TItem, TValue>
        where TSettings : ItemSettings<TItem>
        where TItem : ItemValue<TValue>
        where TItemManager : BaseItemManager<TSettings, TItem, TValue>
    {
        public override Command Configure()
        {
            Command res = new Command("list", "List all.");
            return res;
        }

        public abstract Task RenderItems(ITerminal terminal, IDictionary<string, TItem> items, PipelineContext pipeline);

        public override async Task<int> Handle(ListCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            TSettings? res = await (await GetManager(pipeline)).Settings;
            if (res != null)
            {
                await RenderItems(terminal, res.Items, pipeline);
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
