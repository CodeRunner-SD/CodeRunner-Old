using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.IO;

namespace CodeRunner
{
    public static class ServicesExtensions
    {
        internal const string ReplCommandId = "repl";

        internal const string CliCommandId = "cli";

        public static IWorkspace GetWorkspace(this ServiceScope scope) => scope.Get<IWorkspace>();

        public static IWorkItem? GetWorkItem(this ServiceScope scope) => scope.TryGet<IWorkItem>(out IWorkItem? workItem) ? workItem : null;

        public static IConsole GetConsole(this ServiceScope scope) => scope.Get<IConsole>();

        public static TextReader GetInput(this ServiceScope scope) => scope.Get<TextReader>();

        public static Command GetReplCommand(this ServiceScope scope) => scope.Get<Command>(ReplCommandId);

        public static Command GetCliCommand(this ServiceScope scope) => scope.Get<Command>(CliCommandId);

        public static ILogger GetLogger(this ServiceScope scope) => scope.Get<ILogger>();
    }
}