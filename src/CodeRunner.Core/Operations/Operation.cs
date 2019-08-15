using CodeRunner.Executors;
using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Operations
{
    public delegate Task<bool> OperationCommandExecutingHandler(Operation sender, int index, CLIExecutorSettings settings, string[] commands);

    public delegate Task<bool> OperationCommandExecutedHandler(Operation sender, int index, ExecutorResult result);

    public static class OperationVariables
    {
        public static readonly Variable InputPath = new Variable("inputPath").Required();

        public static readonly Variable OutputPath = new Variable("outputPath").Required();
    }

    public class Operation : BaseTemplate<bool>
    {
        public static readonly Variable VarShell = new Variable("shell").Required().ReadOnly();

        public Operation(IList<CommandLineTemplate>? items = null)
        {
            Items = items ?? new List<CommandLineTemplate>();
        }

        public Operation() : this(null)
        {
        }

        public IList<CommandLineTemplate> Items { get; }

        public event OperationCommandExecutingHandler? CommandExecuting;

        public event OperationCommandExecutedHandler? CommandExecuted;

        public Operation Use(CommandLineTemplate command)
        {
            Items.Add(command);
            return this;
        }

        public override async Task<bool> Resolve(ResolveContext context)
        {
            string shell = context.GetVariable<string>(VarShell);
            for (int index = 0; index < Items.Count; index++)
            {
                CommandLineTemplate v = Items[index];
                string[] cmds = await v.Resolve(context);
                CLIExecutorSettings res = new CLIExecutorSettings(shell, new string[]
                {
                    "-c",
                    string.Join(' ', cmds)
                })
                {
                    TimeLimit = TimeSpan.FromSeconds(10)
                };
                if (CommandExecuting != null)
                {
                    if (!await CommandExecuting.Invoke(this, index, res, cmds))
                    {
                        return false;
                    }
                }

                using CLIExecutor exe = new CLIExecutor(res);
                ExecutorResult result = await exe.Run();
                if (CommandExecuted != null)
                {
                    if (!await CommandExecuted.Invoke(this, index, result))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(VarShell);
            foreach (CommandLineTemplate v in Items)
            {
                res.Collect(v);
            }

            return res;
        }
    }
}
