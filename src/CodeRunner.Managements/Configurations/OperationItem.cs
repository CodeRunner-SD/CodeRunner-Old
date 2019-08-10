using CodeRunner.IO;
using CodeRunner.Operations;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.Configurations
{
    public class OperationItem : ItemValue<Operation?>
    {
        public string FileName { get; set; } = "";

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public OperationManager? Parent { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TemplateFileLoaderPool<Operation>? FileLoaderPool { get; set; }

        private TemplateFileLoader<Operation>? FileLoader { get; set; }

        protected override Task<Operation?> GetValue()
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
                    FileLoader = new TemplateFileLoader<Operation>(file);
                }
                return FileLoader.Data;
            }
            return Task.FromResult<Operation?>(null);
        }
    }
}
