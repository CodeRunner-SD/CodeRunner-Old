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

        public static Workspace GetWorkspace(this ServiceScope scope) => scope.Get<Workspace>();

        public static WorkItem? GetWorkItem(this ServiceScope scope)
        {
            if (scope.TryGet<WorkItem>(out WorkItem? workItem))
            {
                return workItem;
            }
            return null;
        }

        public static IConsole GetConsole(this ServiceScope scope) => scope.Get<IConsole>();

        public static TextReader GetInput(this ServiceScope scope) => scope.Get<TextReader>();

        public static Command GetReplCommand(this ServiceScope scope) => scope.Get<Command>(ReplCommandId);

        public static Command GetCliCommand(this ServiceScope scope) => scope.Get<Command>(CliCommandId);

        public static ILogger GetLogger(this ServiceScope scope) => scope.Get<ILogger>();
    }
}