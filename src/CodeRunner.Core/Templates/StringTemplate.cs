using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class StringTemplate : BaseTemplate<string>
    {
        public static string GetVariableTemplate(string name) => $"{{name}}";

        public StringTemplate(string content, string[]? variables = null) : base(variables)
        {
            Content = content;
        }

        public string Content { get; set; }

        public override Task<string> Resolve(TemplateResolveContext context)
        {
            StringBuilder sb = new StringBuilder(Content);
            foreach (var name in Variables)
            {
                sb.Replace(GetVariableTemplate(name), context.GetVariable<string>(name));
            }
            return Task.FromResult(sb.ToString());
        }

        public static implicit operator StringTemplate(string content)
        {
            return new StringTemplate(content, Array.Empty<string>());
        }
    }
}
