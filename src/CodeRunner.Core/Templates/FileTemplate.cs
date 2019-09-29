using CodeRunner.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class FileTemplate : BaseTemplate<FileInfo>
    {
        private const string VarFilePath = "filePath";

        public static Variable Var => new Variable(VarFilePath).Required();

        public override Task<FileInfo> Resolve(ResolveContext context)
        {
            Assert.IsNotNull(context);
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
