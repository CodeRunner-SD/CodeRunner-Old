using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public static class JsonFormatter
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            });
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public static async Task Serialize(object obj, Stream stream)
        {
            using StreamWriter sw = new StreamWriter(stream);
            await sw.WriteAsync(Serialize(obj));
        }

        public static async Task<T> Deserialize<T>(Stream stream)
        {
            using StreamReader sr = new StreamReader(stream);
            return Deserialize<T>(await sr.ReadToEndAsync());
        }
    }
}
