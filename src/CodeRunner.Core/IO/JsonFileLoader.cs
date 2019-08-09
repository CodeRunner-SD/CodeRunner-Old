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
            using FileStream st = File.OpenWrite();
            await JsonFormatter.Serialize(value, st);
            File.LastWriteTimeUtc = DateTime.UtcNow;
        }

        protected override async Task<T?> OnLoading()
        {
            try
            {
                using FileStream st = File.OpenRead();
                return await JsonFormatter.Deserialize<T>(st);
            }
            catch
            {
                return null;
            }
        }
    }
}
