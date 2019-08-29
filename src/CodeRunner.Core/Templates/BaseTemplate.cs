using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class BaseTemplate
    {
        public virtual VariableCollection GetVariables() => new VariableCollection();

        public abstract Task DoResolve(ResolveContext context);
    }

    public abstract class BaseTemplate<TResult> : BaseTemplate
    {
        public override Task DoResolve(ResolveContext context) => Resolve(context);

        public abstract Task<TResult> Resolve(ResolveContext context);
    }
}
