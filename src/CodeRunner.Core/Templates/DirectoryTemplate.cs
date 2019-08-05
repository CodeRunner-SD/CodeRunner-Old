using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class DirectoryTemplate : BaseTemplate<DirectoryInfo>
    {
        public const string VarDirectoryPath = "directoryPath";

        public override Task<DirectoryInfo> Resolve(TemplateResolveContext context) => ResolveTo(context, context.GetVariable<string>(VarDirectoryPath));

        protected DirectoryTemplate(string[]? variables = null) : base(null)
        {
            var list = new List<string>(variables ?? Array.Empty<string>())
            {
                VarDirectoryPath
            };
            Variables = list;
        }

        public abstract Task<DirectoryInfo> ResolveTo(TemplateResolveContext context, string path);
    }
}
