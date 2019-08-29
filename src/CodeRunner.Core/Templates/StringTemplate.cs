using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class StringTemplate : BaseTemplate<string>
    {
        public static string GetVariableTemplate(string name) => $"{{{name}}}";

        public StringTemplate(string content = "", IList<Variable>? variables = null)
        {
            Content = content;
            UsedVariables = new List<Variable>(variables ?? Array.Empty<Variable>());
        }

        public StringTemplate() : this("", null)
        {
        }

        public string Content { get; set; }

        public IList<Variable> UsedVariables { get; }

        public override Task<string> Resolve(ResolveContext context)
        {
            StringBuilder sb = new StringBuilder(Content);
            foreach (Variable v in UsedVariables)
            {
                _ = sb.Replace(GetVariableTemplate(v.Name), context.GetVariable<string>(v));
            }
            return Task.FromResult(sb.ToString());
        }

        public static implicit operator StringTemplate(string content) => FromString(content);

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            foreach (Variable v in UsedVariables)
            {
                res.Add(v);
            }

            return res;
        }

        public static StringTemplate FromString(string content) => new StringTemplate(content, null);
    }
}
