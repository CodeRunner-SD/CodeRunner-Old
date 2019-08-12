using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands.ItemManagers
{
    public static class RemoveCommand
    {
        public class CArgument
        {
            public string Name { get; set; } = "";
        }
    }

    public abstract class RemoveCommand<TItemManager, TSettings, TItem, TValue> : BaseItemCommand<RemoveCommand.CArgument, TItemManager, TSettings, TItem, TValue>
        where TSettings : ItemSettings<TItem>
        where TItem : ItemValue<TValue>
        where TItemManager : BaseItemManager<TSettings, TItem, TValue>
    {
        public override Command Configure()
        {
            Command res = new Command("remove", "Remove item.");
            res.AddAlias("rm");
            {
                Argument<string> arg = new Argument<string>(nameof(RemoveCommand.CArgument.Name))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                res.AddArgument(arg);
            }
            return res;
        }

        public override async Task<int> Handle(RemoveCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            await (await GetManager(operation)).Set(argument.Name, null);
            return 0;
        }
    }
}
