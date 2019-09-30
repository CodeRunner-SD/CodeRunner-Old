using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Commands
{
    public abstract class BaseCommand<T> : ICommandBuilder
    {
        public abstract Command Configure();

        protected abstract Task<int> Handle(T argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken);

        public virtual Command Build()
        {
            Command command = Configure();
            command.Handler = CommandHandler.Create(async (T argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken) =>
            {
                int res = await Handle(argument, console, context, pipeline, cancellationToken).ConfigureAwait(true);
                return res;
            });
            return command;
        }
    }
}
