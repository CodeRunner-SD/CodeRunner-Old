using CodeRunner.Packagings;
using System.IO;

namespace CodeRunner.IO
{
    public class PackageFileLoaderPool<T> : FileLoaderPool<PackageFileLoader<T>, Package<T>> where T : class
    {
        protected override PackageFileLoader<T> Create(FileInfo file) => new PackageFileLoader<T>(file);
    }
}
