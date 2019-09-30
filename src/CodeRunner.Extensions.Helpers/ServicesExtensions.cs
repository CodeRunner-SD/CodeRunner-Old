using CodeRunner.Diagnostics;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.IO;

namespace CodeRunner.Extensions.Helpers
{
    public static class ServicesExtensions
    {
        internal const string ReplCommandId = "repl";

        internal const string CliCommandId = "cli";

        public static IWorkspace GetWorkspace(this ServiceScope scope)
        {
            Assert.IsNotNull(scope);
            return scope.Get<IWorkspace>();
        }

        public static IWorkItem? GetWorkItem(this ServiceScope scope)
        {
            Assert.IsNotNull(scope);
            return scope.TryGet<IWorkItem>(out IWorkItem? workItem) ? workItem : null;
        }

        public static IConsole GetConsole(this ServiceScope scope)
        {
            Assert.IsNotNull(scope);
            return scope.Get<IConsole>();
        }

        public static IHost GetHost(this ServiceScope scope)
        {
            Assert.IsNotNull(scope);
            return scope.Get<IHost>();
        }

        public static TextReader GetInput(this ServiceScope scope)
        {
            Assert.IsNotNull(scope);
            return scope.Get<TextReader>();
        }

        public static ILogger GetLogger(this ServiceScope scope)
        {
            Assert.IsNotNull(scope);
            return scope.Get<ILogger>();
        }
    }
}