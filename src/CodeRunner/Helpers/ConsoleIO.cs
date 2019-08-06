using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Rendering;
using System.Text;

namespace CodeRunner.Helpers
{
    public static class ConsoleIO
    {
        public static string? InputVariableValue(this IConsole console, Variable variable)
        {
            console.Out.Write($"  {variable.Name}");
            console.Out.Write(variable.IsRequired ? "*" : $"({variable.Default?.ToString()})");
            console.Out.Write(": ");
            return console.InputLine();
        }

        public static string? InputLine(this IConsole console)
        {
            return Program.Input.ReadLine();
        }

        public static bool IsEndOfInput(this IConsole console)
        {
            return Program.Environment == EnvironmentType.Test && Program.Input.Peek() == -1;
        }

        public static bool FillVariables(this IConsole console, IEnumerable<Variable> variables, ResolveContext context)
        {
            bool isFirst = true;
            foreach (Variable v in variables)
            {
                if (context.HasVariable(v.Name))
                {
                    continue;
                }

                if (isFirst)
                {
                    console.Out.WriteLine("Please input variable value:");
                    isFirst = false;
                }

                string? line = console.InputVariableValue(v);
                if (string.IsNullOrEmpty(line))
                {
                    if (v.IsRequired)
                    {
                        return false;
                    }
                }
                else
                {
                    context.WithVariable(v.Name, line!);
                }
            }
            return true;
        }
    }
}
