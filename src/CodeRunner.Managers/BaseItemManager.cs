using CodeRunner.Managers.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public abstract class BaseItemManager<TSettings, TItem, TValue> : BaseManager<TSettings> where TSettings : ItemSettings<TItem> where TItem : class where TValue : class
    {
        protected BaseItemManager(DirectoryInfo pathRoot, DirectoryTemplate directoryTemplate) : base(pathRoot, directoryTemplate)
        {
        }

        public virtual async Task<TItem?> GetItem(string id)
        {
            TSettings? settings = await Settings;
            if (settings == null)
            {
                return null;
            }

            if (settings.Items.TryGetValue(id, out TItem? item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public abstract Task<TValue?> Get(TItem item);
    }
}
