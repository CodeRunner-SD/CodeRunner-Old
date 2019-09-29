using CodeRunner.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class BinaryFileTemplate : FileTemplate
    {
        private string content;

        public BinaryFileTemplate(byte[] content)
        {
            Assert.IsNotNull(content);
            this.content = Convert.ToBase64String(content);
        }

        public string Content
        {
            get => content;
            set
            {
                Assert.IsNotNull(value);
                content = value;
            }
        }

        public override Task<FileInfo> ResolveTo(ResolveContext context, string path)
        {
            Assert.IsNotNull(context);
            Assert.IsNotNull(path);

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
