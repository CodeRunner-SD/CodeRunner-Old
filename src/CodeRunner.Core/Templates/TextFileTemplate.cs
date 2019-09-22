using CodeRunner.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class TextFileTemplate : FileTemplate
    {
        public TextFileTemplate() : this("")
        {
        }

        public TextFileTemplate(StringTemplate content)
        {
            Assert.IsNotNull(content);

            Content = content;
        }

        public StringTemplate Content { get; set; }

        public override async Task<FileInfo> ResolveTo(ResolveContext context, string path)
        {
            Assert.IsNotNull(context);
            Assert.IsNotNull(path);

            string content = await Content.Resolve(context).ConfigureAwait(false);
            FileInfo res = new FileInfo(path);
            using (FileStream fs = res.Open(FileMode.Create))
            {
                using StreamWriter ss = new StreamWriter(fs);
                await ss.WriteAsync(content).ConfigureAwait(false);
            }
            return res;
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Collect(Content);
            return res;
        }
    }
}
