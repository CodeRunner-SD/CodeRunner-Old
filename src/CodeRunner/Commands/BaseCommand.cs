using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public abstract class BaseCommand<T>
    {
        public abstract Command Configure();

        public abstract Task<int> Handle(T argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken);

        public virtual Command Build()
        {
            Command command = Configure();
            command.Handler = CommandHandler.Create(async (T argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken) =>
            {
                operation.Logs.Debug($"Command {GetType().FullName} invoking.");
                var res = await Handle(argument, console, context, operation, cancellationToken);
                operation.Logs.Debug($"Command {GetType().FullName} invoked with {res}.");
                return res;
            });
            return command;
        }
    }
}
