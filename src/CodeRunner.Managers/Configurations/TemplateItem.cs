using CodeRunner.IO;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
    public class TemplateItem : ItemValue<BaseTemplate?>
    {
        public string FileName { get; set; } = "";

        public TemplateManager? Parent { get; set; }

        public TemplateFileLoaderPool<BaseTemplate>? FileLoaderPool { get; set; }

        private TemplateFileLoader<BaseTemplate>? FileLoader { get; set; }

        protected override Task<BaseTemplate?> GetValue()
        {
            if (FileLoader != null)
            {
                return FileLoader.Data;
            }
            if (Parent != null)
            {
                FileInfo file = new FileInfo(Path.Join(Parent.PathRoot.FullName, FileName));
                if (FileLoaderPool != null)
                {
                    FileLoader = FileLoaderPool.Get(file);
                }
                else
                {
                    FileLoader = new TemplateFileLoader<BaseTemplate>(file);
                }
                return FileLoader.Data;
            }
            return Task.FromResult<BaseTemplate?>(null);
        }
    }
}
