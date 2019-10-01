using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.ItemManagers
{
    public static class ListCommand
    {
        public class CArgument
        {
        }
    }

    public abstract class ListCommand<TItemManager, TSettings, TItem> : BaseItemCommand<ListCommand.CArgument, TItemManager, TSettings, TItem>
        where TSettings : class
        where TItem : class
        where TItemManager : IItemManager<TSettings, TItem>
    {
        public override Command Configure()
        {
            Command res = new Command("list", "List all.");
            return res;
        }

        public abstract Task RenderItems(ITerminal terminal, IAsyncEnumerable<(string, TItem?)> items, PipelineContext pipeline);

        protected override async Task<int> Handle(ListCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            static async IAsyncEnumerable<(string, TItem?)> GetAllItems(TItemManager manager)
            {
                await foreach (string key in manager.GetKeys())
                {
                    yield return (key, await manager.GetValue(key));
                }
            }

            ITerminal terminal = console.GetTerminal();
            TItemManager manager = await GetManager(pipeline);
            await RenderItems(terminal, GetAllItems(manager), pipeline);
            return 0;
        }
    }
}
