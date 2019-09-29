using CodeRunner.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public class JsonFileLoader<T> : ObjectFileLoader<T> where T : class
    {
        public JsonFileLoader(FileInfo file) : base(file)
        {
        }

        public override async Task Save(T value)
        {
            Assert.IsNotNull(value);

            using FileStream st = File.Open(FileMode.Create, FileAccess.Write);
            await JsonFormatter.Serialize(value, st).ConfigureAwait(false);
            File.LastWriteTime = DateTime.Now;
        }

        protected override async Task<T?> OnLoading()
        {
            using FileStream st = File.OpenRead();
            return await JsonFormatter.Deserialize<T>(st).ConfigureAwait(false);
        }
    }
}
