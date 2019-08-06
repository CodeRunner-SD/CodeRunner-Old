using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class FileTemplate : BaseTemplate<FileInfo>
    {
        private const string VarFilePath = "filePath";

        public static readonly Variable Var = new Variable(VarFilePath).Required().ReadOnly();

        public override Task<FileInfo> Resolve(ResolveContext context)
        {
            return ResolveTo(context, context.GetVariable<string>(Var));
        }

        public abstract Task<FileInfo> ResolveTo(ResolveContext context, string path);

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(Var);
            return res;
        }
    }
}
