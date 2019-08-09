using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
    public abstract class ItemValue<T, TParent> where TParent : class
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Task<T> Value => GetValue();

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TParent? Parent { get; set; }

        protected abstract Task<T> GetValue();
    }
}
