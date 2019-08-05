using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public static class BaseTemplate
    {
        public static async Task<T> Load<T>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }

    public abstract class BaseTemplate<TResult>
    {
        protected BaseTemplate(string[]? variables)
        {
            if (variables != null)
                foreach(var v in variables)
                    Variables.Add(v);
        }

        public IList<string> Variables { get; set; } = new List<string>();

        public async Task<TResult> Resolve(IDictionary<string, object>? variables = null)
        {
            variables ??= new Dictionary<string, object>();
            return await Resolve(new TemplateResolveContext(variables!));
        }

        public abstract Task<TResult> Resolve(TemplateResolveContext context);

        public virtual async Task Save(Stream stream)
        {
            await JsonSerializer.SerializeAsync(stream, this, GetType());
        }
    }
}
