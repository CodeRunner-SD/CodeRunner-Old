using CodeRunner.Diagnostics;
using CodeRunner.IO;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Packagings
{
    public static class Package
    {
        public static Task<Package<T>> Load<T>(Stream stream) where T : class => JsonFormatter.Deserialize<Package<T>>(stream);
    }

    public class Package<T> where T : class
    {
        public Package() { }

        public Package(T data) : this()
        {
            Assert.IsNotNull(data);
            Data = data;
        }

        public T? Data { get; set; }

        public PackageMetadata? Metadata { get; set; }

        public Task Save(Stream stream) => JsonFormatter.Serialize(this, stream);
    }
}
