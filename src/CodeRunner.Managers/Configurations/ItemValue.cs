using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
    public abstract class ItemValue<T>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Task<T> Value => GetValue();

        protected abstract Task<T> GetValue();
    }
}
