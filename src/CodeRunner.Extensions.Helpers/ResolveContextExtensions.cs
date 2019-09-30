using CodeRunner.Diagnostics;
using CodeRunner.Templates;
using System.Collections.Generic;

namespace CodeRunner.Extensions.Helpers
{
    public static class ResolveContextExtensions
    {
        public static ResolveContext FromArgumentList(this ResolveContext context, IReadOnlyCollection<string> args)
        {
            Assert.IsNotNull(args);
            Assert.IsNotNull(context);

            foreach (string s in args)
            {
                int id = s.IndexOf('=', System.StringComparison.InvariantCulture);
                if (id == -1)
                {
                    continue;
                }

                string name = s.Substring(0, id);
                string value = s.Substring(id + 1);
                _ = context.WithVariable(name, value);
            }
            return context;
        }
    }
}
