using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased
{
    public class RegisterWorkItemTemplate : FunctionBasedTemplate<IWorkItem?>
    {
        public static Variable Target => new Variable("target").Required();

        public static Variable Type => new Variable("type").NotRequired("file");

        public static Variable Workspace => new Variable("workspace").Required();

        public RegisterWorkItemTemplate() : base(context =>
        {
            if (context.TryGetVariable(Type, out string? type))
            {
                if (context.TryGetVariable(Target, out string? target))
                {
                    Workspace space = context.GetVariable<Workspace>(Workspace);
                    switch (type)
                    {
                        case "file":
                        case "f":
                            return Task.FromResult<IWorkItem?>(WorkItem.CreateByFile(space, new FileInfo(target)));
                        case "directory":
                        case "d":
                        case "dir":
                            return Task.FromResult<IWorkItem?>(WorkItem.CreateByDirectory(space, new DirectoryInfo(target)));
                    }
                }
            }
            return Task.FromResult<IWorkItem?>(null);
        }, null)
        {
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(Type);
            res.Add(Target);
            res.Add(Workspace);
            return res;
        }
    }
}
