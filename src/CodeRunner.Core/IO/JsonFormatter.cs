using CodeRunner.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public static class JsonFormatter
    {
        public static string Serialize(object value, JsonSerializerSettings? settings = null)
        {
            Assert.IsNotNull(value);

            return JsonConvert.SerializeObject(value, Formatting.Indented, settings ?? new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            });
        }

        public static T Deserialize<T>(string json)
        {
            Assert.IsNotNull(json);

            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public static async Task Serialize(object value, Stream stream)
        {
            using StreamWriter sw = new StreamWriter(stream);
            await sw.WriteAsync(Serialize(value)).ConfigureAwait(false);
        }

        public static async Task<T> Deserialize<T>(Stream stream)
        {
            using StreamReader sr = new StreamReader(stream);
            return Deserialize<T>(await sr.ReadToEndAsync().ConfigureAwait(false));
        }
    }
}
