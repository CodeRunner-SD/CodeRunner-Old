using CodeRunner.Managements;
using System.CommandLine;
using System.IO;

namespace CodeRunner.Helpers
{
    public class TestView
    {
        public static IConsole Console { get; internal set; }

        public static Workspace Workspace { get; internal set; }

        public static TextReader? Input { get; set; }
    }
}
