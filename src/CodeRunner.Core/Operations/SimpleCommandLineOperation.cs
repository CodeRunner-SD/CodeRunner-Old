using CodeRunner.Diagnostics;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Operations
{
    public class SimpleCommandLineOperation : CommandLineOperation
    {
        public SimpleCommandLineOperation(IList<CommandLineTemplate>? items = null) => Items = items ?? new List<CommandLineTemplate>();

        public SimpleCommandLineOperation() : this(null)
        {
        }

        public IList<CommandLineTemplate> Items { get; }

        public BaseOperation Use(CommandLineTemplate command)
        {
            Assert.IsNotNull(command);

            Items.Add(command);
            return this;
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(OperationVariables.VarShell);
            res.Add(OperationVariables.VarWorkingDirectory);
            foreach (CommandLineTemplate v in Items)
            {
                res.Collect(v);
            }

            return res;
        }

        protected override Task<CommandLineOperationSettings> GetSettings(ResolveContext context)
        {
            CommandLineOperationSettings res = new CommandLineOperationSettings();
            foreach (CommandLineTemplate v in Items)
            {
                res.Scripts.Add(v);
            }
            return Task.FromResult(res);
        }
    }
}
