using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class StringTemplate : BaseTemplate<string>
    {
        public static string GetVariableTemplate(string name)
        {
            return $"{{{name}}}";
        }

        public StringTemplate(string content = "", IList<Variable>? variables = null)
        {
            Content = content;
            Variables = new List<Variable>(variables ?? Array.Empty<Variable>());
        }

        public StringTemplate() : this("", null)
        {
        }

        public string Content { get; set; }

        public IList<Variable> Variables { get; set; }

        public override Task<string> Resolve(ResolveContext context)
        {
            StringBuilder sb = new StringBuilder(Content);
            foreach (Variable v in Variables)
            {
                sb.Replace(GetVariableTemplate(v.Name), context.GetVariable<string>(v));
            }
            return Task.FromResult(sb.ToString());
        }

        public static implicit operator StringTemplate(string content)
        {
            return new StringTemplate(content, null);
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            foreach (Variable v in Variables)
            {
                res.Add(v);
            }

            return res;
        }
    }
}
