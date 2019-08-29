using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.IO;

namespace CodeRunner.Rendering
{
    public static class ConsoleIO
    {
        public static string? InputVariableValue(this ITerminal terminal, TextReader input, Variable variable)
        {
            terminal.Output("  ");
            if (variable.IsRequired)
            {
                terminal.OutputEmphasize(variable.Name);
            }
            else
            {
                terminal.OutputBold(variable.Name);
                terminal.Output($"({ variable.GetDefault<object>().ToString()})");
            }
            terminal.Output(": ");
            return input.InputLine();
        }

        public static string? InputLine(this TextReader input) => input.ReadLine();

        public static bool IsEndOfInput(this TextReader input) => Program.Environment == EnvironmentType.Test && input.Peek() == -1;

        public static bool FillVariables(this ITerminal terminal, TextReader input, IEnumerable<Variable> variables, ResolveContext context)
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

                string? line = terminal.InputVariableValue(input, v);
                if (string.IsNullOrEmpty(line))
                {
                    if (v.IsRequired)
                    {
                        return false;
                    }
                }
                else
                {
                    _ = context.WithVariable(v.Name, line!);
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

        public static void Output(this ITerminal terminal, string content) => terminal.Out.Write(content);

        public static void OutputBlink(this ITerminal terminal, string content)
        {
            terminal.Render(StyleSpan.BlinkOn());
            terminal.Output(content);
            terminal.Render(StyleSpan.BlinkOff());
        }

        public static void OutputColor(this ITerminal terminal, ForegroundColorSpan color, string content)
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

        public static void OutputError(this ITerminal terminal, string content) => terminal.OutputColor(ForegroundColorSpan.Red(), content);

        public static void OutputWarning(this ITerminal terminal, string content) => terminal.OutputColor(ForegroundColorSpan.Yellow(), content);

        public static void OutputInformation(this ITerminal terminal, string content) => terminal.OutputColor(ForegroundColorSpan.Cyan(), content);

        public static void OutputDebug(this ITerminal terminal, string content) => terminal.OutputColor(ForegroundColorSpan.Green(), content);

        public static void OutputFatal(this ITerminal terminal, string content) => terminal.OutputColor(BackgroundColorSpan.Red(), content);

        public static void EnsureAtLeft(this ITerminal terminal)
        {
            if (terminal.CursorLeft != 0)
            {
                terminal.OutputLine();
            }
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
            terminal.OutputError(content);
            terminal.OutputLine();
        }

        public static void OutputWarningLine(this ITerminal terminal, string content)
        {
            terminal.OutputWarning(content);
            terminal.OutputLine();
        }

        public static void OutputInformationLine(this ITerminal terminal, string content)
        {
            terminal.OutputInformation(content);
            terminal.OutputLine();
        }

        public static void OutputFatalLine(this ITerminal terminal, string content)
        {
            terminal.OutputFatal(content);
            terminal.OutputLine();
        }

        public static void OutputDebugLine(this ITerminal terminal, string content)
        {
            terminal.OutputDebug(content);
            terminal.OutputLine();
        }

        public static void OutputTable<TSource>(this ITerminal terminal, IEnumerable<TSource> sources, params IOutputTableColumnView<TSource>[] columns)
        {
            int[] length = new int[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                length[i] = Math.Max(length[i], columns[i].MeasureHeader());
                foreach (TSource v in sources)
                {
                    length[i] = Math.Max(length[i], columns[i].Measure(v));
                }
            }
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i].RenderHeader(terminal, length[i]);
                terminal.Output(" ");
            }
            terminal.OutputLine();
            foreach (TSource v in sources)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    columns[i].Render(terminal, v, length[i]);
                    terminal.Output(" ");
                }
                terminal.OutputLine();
            }
        }
    }
}
