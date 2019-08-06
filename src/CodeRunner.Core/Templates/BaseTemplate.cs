using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class BaseTemplate
    {
        public static async Task<T> Load<T>(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await sr.ReadToEndAsync(), new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
                });
            }
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

        public virtual async Task Save(Stream stream)
        {
            using StreamWriter sw = new StreamWriter(stream);
            await sw.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            }));
        }
    }
}
