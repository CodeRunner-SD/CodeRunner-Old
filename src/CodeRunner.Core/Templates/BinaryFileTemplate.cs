using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class BinaryFileTemplate : FileTemplate
    {
        public BinaryFileTemplate(byte[]? content = null) : base(null)
        {
            Content = content ?? Array.Empty<byte>();
        }

        public byte[] Content { get; set; }

        public override Task<FileInfo> ResolveTo(TemplateResolveContext context, string path)
        {
            FileInfo res = new FileInfo(path);
            using (var fs = res.Open(FileMode.Create))
            {
                using var bw = new BinaryWriter(fs);
                bw.Write(Content);
            }
            return Task.FromResult(res);
        }
    }
}
