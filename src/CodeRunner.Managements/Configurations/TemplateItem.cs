using CodeRunner.IO;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.Configurations
{
    public class TemplateItem : ItemValue<Package<BaseTemplate>?>
    {
        public string FileName { get; set; } = "";

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TemplateManager? Parent { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public PackageFileLoaderPool<BaseTemplate>? FileLoaderPool { get; set; }

        private PackageFileLoader<BaseTemplate>? FileLoader { get; set; }

        protected override Task<Package<BaseTemplate>?> GetValue()
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
                    FileLoader = new PackageFileLoader<BaseTemplate>(file);
                }
                return FileLoader.Data;
            }
            return Task.FromResult<Package<BaseTemplate>?>(null);
        }
    }
}
