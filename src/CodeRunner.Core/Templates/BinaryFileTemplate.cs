using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class BinaryFileTemplate : FileTemplate
    {
        public BinaryFileTemplate(byte[] content) => Content = Convert.ToBase64String(content);

        public string Content { get; set; }

        public override Task<FileInfo> ResolveTo(ResolveContext context, string path)
        {
            FileInfo res = new FileInfo(path);
            using (FileStream fs = res.Open(FileMode.Create))
            {
                using BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(Convert.FromBase64String(Content));
            }
            return Task.FromResult(res);
        }
    }
}
