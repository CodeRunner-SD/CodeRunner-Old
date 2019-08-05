using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
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
                using (var st = File.OpenRead())
                    return await JsonSerializer.DeserializeAsync<T>(st);
            }
            catch
            {
                return null;
            }
        }
    }
}
