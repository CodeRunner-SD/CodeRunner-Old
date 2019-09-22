using CodeRunner.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class FunctionBasedTemplate : BaseTemplate
    {
        public FunctionBasedTemplate(Func<ResolveContext, Task> func, IList<Variable>? variables = null)
        {
            Assert.IsNotNull(func);
            Func = func;
            UsedVariables = new List<Variable>(variables ?? Array.Empty<Variable>());
        }

        protected Func<ResolveContext, Task> Func { get; set; }

        protected List<Variable> UsedVariables { get; }

        public override Task DoResolve(ResolveContext context)
        {
            Assert.IsNotNull(context);
            return Func(context);
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            foreach (Variable v in UsedVariables)
            {
                res.Add(v);
            }

            return res;
        }
    }

    public class FunctionBasedTemplate<TResult> : BaseTemplate<TResult>
    {
        public FunctionBasedTemplate(Func<ResolveContext, Task<TResult>> func, IList<Variable>? variables = null)
        {
            Func = func;
            UsedVariables = new List<Variable>(variables ?? Array.Empty<Variable>());
        }

        protected Func<ResolveContext, Task<TResult>> Func { get; set; }

        protected List<Variable> UsedVariables { get; }

        public override Task<TResult> Resolve(ResolveContext context) => Func(context);

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            foreach (Variable v in UsedVariables)
            {
                res.Add(v);
            }

            return res;
        }
    }
}
