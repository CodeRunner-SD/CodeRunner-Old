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

        public StringTemplate(string content = "", string[]? variables = null) : base(variables)
        {
            Content = content;
        }

        public StringTemplate() : this("", null)
        {
        }

        public string Content { get; set; }

        public override Task<string> Resolve(TemplateResolveContext context)
        {
            StringBuilder sb = new StringBuilder(Content);
            foreach (var name in Variables)
            {
                if (context.TryGetVariable<string>(name, out var value))
                {
                    sb.Replace(GetVariableTemplate(name), value);
                }
            }
            return Task.FromResult(sb.ToString());
        }

        public static implicit operator StringTemplate(string content)
        {
            return new StringTemplate(content, Array.Empty<string>());
        }
    }
}
