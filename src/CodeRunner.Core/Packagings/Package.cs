using CodeRunner.IO;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Packagings
{
    public class Package<T> where T : class
    {
        public Package() { }

        public Package(T data) : this()
        {
            Data = data;
        }

        public T? Data { get; set; }

        public PackageMetadata? Metadata { get; set; }

        public Task Save(Stream stream)
        {
            return JsonFormatter.Serialize(this, stream);
        }

        public static Task<Package<T>> Load(Stream stream)
        {
            return JsonFormatter.Deserialize<Package<T>>(stream);
        }
    }
}
