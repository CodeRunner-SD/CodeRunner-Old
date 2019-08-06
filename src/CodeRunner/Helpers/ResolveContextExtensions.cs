using CodeRunner.Templates;
using System.Collections.Generic;

namespace CodeRunner.Helpers
{
    public static class ResolveContextExtensions
    {
        public static ResolveContext FromArgumentList(this ResolveContext context, IReadOnlyCollection<string> args)
        {
            foreach (string s in args)
            {
                int id = s.IndexOf('=');
                string name = s.Substring(0, id);
                string value = s.Substring(id + 1);
                context.WithVariable(name, value);
            }
            return context;
        }
    }
}
