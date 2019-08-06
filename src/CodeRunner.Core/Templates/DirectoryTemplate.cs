using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class DirectoryTemplate : BaseTemplate<DirectoryInfo>
    {
        private const string VarDirectoryPath = "directoryPath";

        public static readonly Variable Var = new Variable(VarDirectoryPath).Required().ReadOnly();

        public override Task<DirectoryInfo> Resolve(ResolveContext context)
        {
            return ResolveTo(context, context.GetVariable<string>(Var));
        }

        public abstract Task<DirectoryInfo> ResolveTo(ResolveContext context, string path);

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(Var);
            return res;
        }
    }
}
