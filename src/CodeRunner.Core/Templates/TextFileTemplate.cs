using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class TextFileTemplate : BaseTemplate<FileInfo>
    {
        public const string VarFilePath = "filePath";

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public TextFileTemplate(StringTemplate content) : base(new string[] { VarFilePath })
        {
            Content = content;
        }

        public StringTemplate Content { get; set; }

        public override async Task<FileInfo> Resolve(TemplateResolveContext context)
        {
            string content = await Content.Resolve(context);
            FileInfo res = new FileInfo(context.GetVariable<string>(VarFilePath));
            using (var fs = res.Open(FileMode.Create))
            {
                using var ss = new StreamWriter(fs, Encoding);
                await ss.WriteAsync(content);
            }
            return res;
        }
    }
}
