using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class BaseTemplate
    {
        public virtual VariableCollection GetVariables()
        {
            return new VariableCollection();
        }

        public abstract Task DoResolve(ResolveContext context);
    }

    public abstract class BaseTemplate<TResult> : BaseTemplate
    {
        public override async Task DoResolve(ResolveContext context)
        {
            await Resolve(context);
        }

        public abstract Task<TResult> Resolve(ResolveContext context);
    }
}
