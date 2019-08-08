using CodeRunner.Templates;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Rendering;

namespace CodeRunner.Helpers
{
    public static class ConsoleIO
    {
        public static string? InputVariableValue(this ITerminal terminal, Variable variable)
        {
            terminal.Output("  ");
            if (variable.IsRequired)
            {
                terminal.OutputEmphasize(variable.Name);
            }
            else
            {
                terminal.OutputBold(variable.Name);
                terminal.Output($"({ variable.Default?.ToString()})");
            }
            terminal.Output(": ");
            return terminal.InputLine();
        }

        public static string? InputLine(this IConsole _)
        {
            return Program.Input.ReadLine();
        }

        public static bool IsEndOfInput(this IConsole _)
        {
            return Program.Environment == EnvironmentType.Test && Program.Input.Peek() == -1;
        }

        public static bool FillVariables(this ITerminal terminal, IEnumerable<Variable> variables, ResolveContext context)
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
                    terminal.OutputLine("Please input variable value:");
                    isFirst = false;
                }

                string? line = terminal.InputVariableValue(v);
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

        public static void OutputEmphasize(this ITerminal terminal, string content)
        {
            terminal.Render(StyleSpan.BoldOn());
            terminal.Render(StyleSpan.UnderlinedOn());
            terminal.Output(content);
            terminal.Render(StyleSpan.BoldOff());
            terminal.Render(StyleSpan.UnderlinedOff());
        }

        public static void OutputStandout(this ITerminal terminal, string content)
        {
            terminal.Render(StyleSpan.StandoutOn());
            terminal.Out.Write(content);
            terminal.Render(StyleSpan.StandoutOff());
        }

        public static void OutputBold(this ITerminal terminal, string content)
        {
            terminal.Render(StyleSpan.BoldOn());
            terminal.Output(content);
            terminal.Render(StyleSpan.BoldOff());
        }

        public static void OutputUnderline(this ITerminal terminal, string content)
        {
            terminal.Render(StyleSpan.UnderlinedOn());
            terminal.Output(content);
            terminal.Render(StyleSpan.UnderlinedOff());
        }

        public static void Output(this ITerminal terminal, string content)
        {
            terminal.Out.Write(content);
        }

        public static void OutputBlink(this ITerminal terminal, string content)
        {
            terminal.Render(StyleSpan.BlinkOn());
            terminal.Output(content);
            terminal.Render(StyleSpan.BlinkOff());
        }

        public static void OutputColor(this ITerminal terminal, ForegroundColorSpan color,string content)
        {
            terminal.Render(color);
            terminal.Output(content);
            terminal.Render(ForegroundColorSpan.Reset());
        }

        public static void OutputColor(this ITerminal terminal, BackgroundColorSpan color, string content)
        {
            terminal.Render(color);
            terminal.Output(content);
            terminal.Render(BackgroundColorSpan.Reset());
        }

        public static void OutputError(this ITerminal terminal, string content)
        {
            terminal.OutputColor(ForegroundColorSpan.Red(), content);
        }

        public static void OutputWarning(this ITerminal terminal, string content)
        {
            terminal.OutputColor(ForegroundColorSpan.Yellow(), content);
        }

        public static void OutputInformation(this ITerminal terminal, string content)
        {
            terminal.OutputColor(ForegroundColorSpan.Cyan(), content);
        }

        public static void OutputDebug(this ITerminal terminal, string content)
        {
            terminal.OutputColor(ForegroundColorSpan.Magenta(), content);
        }

        public static void OutputFatal(this ITerminal terminal, string content)
        {
            terminal.OutputColor(BackgroundColorSpan.Red(), content);
        }

        public static void OutputLine(this ITerminal terminal, string content)
        {
            terminal.Output(content);
            terminal.OutputLine();
        }

        public static void OutputLine(this ITerminal terminal)
        {
            terminal.Out.Write("\n");
            terminal.SetCursorPosition(0, terminal.CursorTop);
        }

        public static void OutputErrorLine(this ITerminal terminal, string content)
        {
            OutputError(terminal, content);
            terminal.OutputLine();
        }

        public static void OutputWarningLine(this ITerminal terminal, string content)
        {
            OutputWarning(terminal, content);
            terminal.OutputLine();
        }

        public static void OutputInformationLine(this ITerminal terminal, string content)
        {
            OutputInformation(terminal, content);
            terminal.OutputLine();
        }
    }
}
