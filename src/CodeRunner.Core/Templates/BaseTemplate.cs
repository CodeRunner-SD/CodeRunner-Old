using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class BaseTemplate<TResult>
    {
        protected BaseTemplate(string[]? variables)
        {
            Variables = variables ?? Array.Empty<string>();
        }

        public string[] Variables { get; protected set; }



        public async Task<TResult> Resolve(IDictionary<string, object>? variables = null)
        {
            variables ??= new Dictionary<string, object>();
            return await Resolve(new TemplateResolveContext(variables!));
        }

        public abstract Task<TResult> Resolve(TemplateResolveContext context);
    }
}
