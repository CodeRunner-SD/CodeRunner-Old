using CodeRunner.IO;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class BaseTemplate
    {
        public static Task<T> Load<T>(Stream stream)
        {
            return JsonFormatter.Deserialize<T>(stream);
        }

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

        public virtual Task Save(Stream stream)
        {
            return JsonFormatter.Serialize(this, stream);
        }
    }
}
