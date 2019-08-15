using System.Threading.Tasks;

namespace CodeRunner.Managements.Configurations
{
    public abstract class ItemValue<T>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Task<T> Value => GetValue();

        protected abstract Task<T> GetValue();

        public abstract Task SetValue(T value);
    }
}
