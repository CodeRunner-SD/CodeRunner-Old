using CodeRunner.Extensions.Commands;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.ItemManagers
{
    public abstract class BaseItemCommand<TArgument, TItemManager, TSettings, TItem> : BaseCommand<TArgument>
        where TItemManager : IItemManager<TSettings, TItem>
        where TItem : class
        where TSettings : class
    {
        public abstract Task<TItemManager> GetManager(PipelineContext pipeline);
    }
}
