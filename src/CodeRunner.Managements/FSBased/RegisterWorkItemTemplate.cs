using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased
{
    public class RegisterWorkItemTemplate : FunctionBasedTemplate<IWorkItem?>
    {
        public static Variable Target => new Variable("target").Required();

        public static Variable Workspace => new Variable("workspace").Required();

        public RegisterWorkItemTemplate() : base(context =>
        {
            if (context.TryGetVariable(Target, out FileSystemInfo? fs))
            {
                Workspace space = context.GetVariable<Workspace>(Workspace);
                switch (fs)
                {
                    case FileInfo f:
                        return Task.FromResult<IWorkItem?>(WorkItem.CreateByFile(space, f));
                    case DirectoryInfo d:
                        return Task.FromResult<IWorkItem?>(WorkItem.CreateByDirectory(space, d));
                }
            }
            return Task.FromResult<IWorkItem?>(null);
        }, null)
        {
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(Target);
            res.Add(Workspace);
            return res;
        }
    }
}
