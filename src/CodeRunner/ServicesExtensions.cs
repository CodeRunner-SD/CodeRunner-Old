﻿using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System.CommandLine;

namespace CodeRunner
{
    public static class ServicesExtensions
    {
        internal const string ReplCommandId = "repl";

        internal const string CliCommandId = "cli";

        public static ExtensionCollection GetExtensions(this ServiceScope scope) => scope.Get<ExtensionCollection>();

        public static CommandCollection GetCommands(this ServiceScope scope) => scope.Get<CommandCollection>();

        public static Command GetReplCommand(this ServiceScope scope) => scope.Get<Command>(ReplCommandId);

        public static Command GetCliCommand(this ServiceScope scope) => scope.Get<Command>(CliCommandId);
    }
}