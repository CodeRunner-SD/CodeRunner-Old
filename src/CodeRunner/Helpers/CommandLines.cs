using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;

namespace CodeRunner.Helpers
{
    public static class CommandLines
    {
        public static Parser CreateParser(Command command, OperationContext context)
        {
            CommandLineBuilder builder = new CommandLineBuilder(command);
            builder.UseMiddleware(inv =>
            {
                inv.BindingContext.AddService(typeof(OperationContext), () => context);
            });
            return builder.Build();
        }
    }
}
