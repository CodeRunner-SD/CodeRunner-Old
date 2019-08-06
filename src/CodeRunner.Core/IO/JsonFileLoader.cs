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
                using StreamReader sr = new StreamReader(st);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await sr.ReadToEndAsync(), new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
                });
            }
            catch
            {
                return null;
            }
        }
    }
}
