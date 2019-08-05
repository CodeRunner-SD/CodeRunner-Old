using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public abstract class FileTemplate : BaseTemplate<FileInfo>
    {
        public const string VarFilePath = "filePath";

        public override Task<FileInfo> Resolve(TemplateResolveContext context) => ResolveTo(context, context.GetVariable<string>(VarFilePath));

        protected FileTemplate(string[]? variables = null) : base(null)
        {
            List<string> list = new List<string>(variables ?? Array.Empty<string>())
            {
                VarFilePath
            };
            base.Variables = list;
        }

        public abstract Task<FileInfo> ResolveTo(TemplateResolveContext context, string path);
    }
}
