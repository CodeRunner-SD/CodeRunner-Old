using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public class JsonFileLoader<T> : ObjectFileLoader<T> where T : class
    {
        public JsonFileLoader(FileInfo file) : base(file)
        {
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
