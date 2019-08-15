using CodeRunner.IO;
using CodeRunner.Packagings;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.Configurations
{
    public class PackageFileItem<T, TParent> : ItemValue<Package<T>?> where TParent : class, IHasPathRoot where T : class
    {
        private PackageFileLoader<T>? fileLoader;

        public string FileName { get; set; } = "";

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TParent? Parent { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public PackageFileLoaderPool<T>? FileLoaderPool { get; set; }

        private PackageFileLoader<T>? FileLoader
        {
            get
            {
                if (fileLoader != null)
                {
                    return fileLoader;
                }
                if (Parent != null)
                {
                    FileInfo file = new FileInfo(Path.Join(Parent.PathRoot.FullName, FileName));
                    if (FileLoaderPool != null)
                    {
                        fileLoader = FileLoaderPool.Get(file);
                    }
                    else
                    {
                        fileLoader = new PackageFileLoader<T>(file);
                    }
                    return fileLoader;
                }
                return null;
            }
        }

        public override async Task SetValue(Package<T>? value)
        {
            if (FileLoader != null)
            {
                if (value == null)
                {
                    if (FileLoader.File.Exists)
                    {
                        FileLoader.File.Delete();
                    }
                }
                else
                {
                    await FileLoader.Save(value);
                }
            }
        }

        protected override Task<Package<T>?> GetValue()
        {
            if (FileLoader != null)
            {
                return FileLoader.Data;
            }
            return Task.FromResult<Package<T>?>(null);
        }
    }
}
