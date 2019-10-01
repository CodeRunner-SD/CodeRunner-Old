using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    internal static class Utils
    {
        public static Task ResolveCallback(VariableCollection variables, ResolveContext resolveContext, InvocationContext context, PipelineContext pipeline)
        {
            _ = resolveContext.FromArgumentList(context.ParseResult.UnparsedTokens);

            TextReader input = pipeline.Services.GetInput();
            ITerminal terminal = pipeline.Services.GetConsole().GetTerminal();

            if (!terminal.FillVariables(input, variables, resolveContext))
                throw new ArgumentException();
            return Task.CompletedTask;
        }
    }
}
