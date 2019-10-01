using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.ItemManagers
{
    public static class RemoveCommand
    {
        public class CArgument
        {
            public string Name { get; set; } = "";
        }
    }

    public abstract class RemoveCommand<TItemManager, TSettings, TItem> : BaseItemCommand<RemoveCommand.CArgument, TItemManager, TSettings, TItem>
        where TSettings : class
        where TItem : class
        where TItemManager : IItemManager<TSettings, TItem>
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

        protected override async Task<int> Handle(RemoveCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            await (await GetManager(operation)).SetValue(argument.Name, null);
            return 0;
        }
    }
}
