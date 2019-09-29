using CodeRunner.Diagnostics;
using CodeRunner.Packagings;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public class PackageFileLoader<T> : ObjectFileLoader<Package<T>> where T : class
    {
        public PackageFileLoader(FileInfo file) : base(file)
        {
        }

        protected override async Task<Package<T>?> OnLoading()
        {
            using FileStream st = File.OpenRead();
            return await Package.Load<T>(st).ConfigureAwait(false);
        }

        public override async Task Save(Package<T> value)
        {
            Assert.IsNotNull(value);

            using FileStream st = File.Open(FileMode.Create, FileAccess.Write);
            await value.Save(st).ConfigureAwait(false);
            File.LastWriteTime = DateTime.Now;
        }
    }
}
