using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Pipelines;
using System.Threading.Tasks;

namespace CodeRunner.Commands.ItemManagers
{
    public abstract class BaseItemCommand<TArgument, TItemManager, TSettings, TItem, TValue> : BaseCommand<TArgument>
        where TSettings : ItemSettings<TItem>
        where TItem : ItemValue<TValue>
        where TItemManager : BaseItemManager<TSettings, TItem, TValue>
    {
        public abstract Task<TItemManager> GetManager(PipelineContext pipeline);
    }
}
