using CodeRunner.Managers;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace CodeRunner.Helpers
{
    public class TestView
    {
        public static IConsole Console { get; internal set; }

        public static Workspace Workspace { get; internal set; }
    }
}
