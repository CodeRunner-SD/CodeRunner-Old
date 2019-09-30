using CodeRunner.Extensions;
using System;

[assembly: EntryExtension(typeof(CodeRunner.Extensions.Builtin.Console.ConsoleExtension))]

namespace CodeRunner.Extensions.Builtin.Console
{
    public class ConsoleExtension : IExtension
    {
        public string Name => "Console";

        public string Publisher => "CodeRunner";

        public string Description => "Add basic console and terminal commands.";

        public Version Version => new Version(1, 0);
    }
}
