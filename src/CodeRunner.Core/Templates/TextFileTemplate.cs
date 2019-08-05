﻿using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class TextFileTemplate : FileTemplate
    {
        public TextFileTemplate() : this("")
        {
        }

        public TextFileTemplate(StringTemplate content) : base(null)
        {
            Content = content;
        }

        public StringTemplate Content { get; set; }

        public override async Task<FileInfo> ResolveTo(TemplateResolveContext context, string path)
        {
            string content = await Content.Resolve(context);
            FileInfo res = new FileInfo(path);
            using (var fs = res.Open(FileMode.Create))
            {
                using var ss = new StreamWriter(fs);
                await ss.WriteAsync(content);
            }
            return res;
        }
    }
}