using CodeRunner.Executors;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
    public delegate Task<bool> OperationCommandExecutingHandler(Operation sender, int index, ProcessStartInfo process, string command);

    public delegate Task<bool> OperationCommandExecutedHandler(Operation sender, int index, ExecutorResult result);

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

        public event OperationCommandExecutingHandler CommandExecuting;

        public event OperationCommandExecutedHandler CommandExecuted;

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
                string cmd = await v.Resolve(context);
                ProcessStartInfo res = new ProcessStartInfo
                {
                    FileName = shell,
                    Arguments = $"-c {cmd}"
                };

                if (CommandExecuting != null)
                {
                    if (!await CommandExecuting.Invoke(this, index, res, cmd))
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
