using CodeRunner.Executors;
using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
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

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Func<ExecutorResult, Task<bool>>? Handler { get; set; }

        public Operation Use(CommandLineTemplate command)
        {
            Items.Add(command);
            return this;
        }

        public override async Task<bool> Resolve(ResolveContext context)
        {
            string shell = context.GetVariable<string>(VarShell);
            foreach (var v in Items)
            {
                var cmd = await v.Resolve(context);
                ProcessStartInfo res = new ProcessStartInfo
                {
                    FileName = shell,
                    Arguments = $"-c {cmd}"
                };
                using (CLIExecutor exe = new CLIExecutor(res))
                {
                    var result = await exe.Run();
                    if (Handler != null)
                        if (!await Handler.Invoke(result))
                        {
                            break;
                        }
                }
            }
            return true;
        }

        public override VariableCollection GetVariables()
        {
            var res = base.GetVariables();
            res.Add(VarShell);
            foreach (var v in Items)
                res.Collect(v);
            return res;
        }
    }
}
