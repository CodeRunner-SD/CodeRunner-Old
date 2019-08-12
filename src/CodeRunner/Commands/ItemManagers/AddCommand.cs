using CodeRunner.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands.ItemManagers
{
    public static class AddCommand
    {
        public class CArgument
        {
            public string Name { get; set; } = "";

            public FileInfo? File { get; set; }
        }
    }

    public abstract class AddCommand<TItemManager, TSettings, TItem, TValue> : BaseItemCommand<AddCommand.CArgument, TItemManager, TSettings, TItem, TValue>
        where TSettings : ItemSettings<TItem>
        where TItem : ItemValue<TValue>
        where TItemManager : BaseItemManager<TSettings, TItem, TValue>
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

        public override async Task<int> Handle(AddCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken)
        {
            Workspace workspace = operation.Services.GetWorkspace();
            argument.File = CommandLines.ResolvePath(workspace, argument.File!);
            await (await GetManager(operation)).Set(
                argument.Name,
                await GetItem(
                    new FileInfo(
                        Path.GetRelativePath(workspace.Templates.PathRoot.FullName, argument.File!.FullName))));
            return 0;
        }
    }
}
