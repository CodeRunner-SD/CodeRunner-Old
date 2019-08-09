using CodeRunner.Managers;
using System.CommandLine;

namespace CodeRunner.Helpers
{
    public class TestView
    {
        public static IConsole Console { get; internal set; }

        public static Workspace Workspace { get; internal set; }
    }
}
