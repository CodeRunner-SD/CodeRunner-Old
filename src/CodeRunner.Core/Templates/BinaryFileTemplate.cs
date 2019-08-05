using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class BinaryFileTemplate : FileTemplate
    {
        public BinaryFileTemplate() : this(null)
        {
        }

        public BinaryFileTemplate(byte[]? content = null) : base(null)
        {
            if(content == null)
            {
                Content = "";
            }
            else
            {
                Content = Convert.ToBase64String(content!);
            }
        }

        public string Content { get; set; }

        public override Task<FileInfo> ResolveTo(TemplateResolveContext context, string path)
        {
            FileInfo res = new FileInfo(path);
            using (var fs = res.Open(FileMode.Create))
            {
                using var bw = new BinaryWriter(fs);
                bw.Write(Convert.FromBase64String(Content));
            }
            return Task.FromResult(res);
        }
    }
}
