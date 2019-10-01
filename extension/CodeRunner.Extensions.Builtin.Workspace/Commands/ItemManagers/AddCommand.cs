using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.ItemManagers
{
    public static class AddCommand
    {
        public class CArgument
        {
            public string Name { get; set; } = "";

            public FileInfo? File { get; set; }
        }
    }

    public abstract class AddCommand<TItemManager, TSettings, TItem> : BaseItemCommand<AddCommand.CArgument, TItemManager, TSettings, TItem>
        where TSettings : class
        where TItem : class
        where TItemManager : IItemManager<TSettings, TItem>
    {
        public override Command Configure()
        {
            Command res = new Command("add", "Add new item.");
            {
                Argument<string> arg = new Argument<string>(nameof(AddCommand.CArgument.Name))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                res.AddArgument(arg);
            }
            {
                Argument<FileInfo> arg = new Argument<FileInfo>(nameof(AddCommand.CArgument.File))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                res.AddArgument(arg);
            }
            return res;
        }

        public abstract Task<TItem> GetItem(FileInfo file);

        protected override async Task<int> Handle(AddCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            if (argument.File == null)
                return -1;

            TItem item = await GetItem(argument.File);

            try
            {
                await (await GetManager(operation)).SetValue(
                    argument.Name, item);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
    }
}
