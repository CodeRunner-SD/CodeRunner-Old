using CodeRunner.Managements.Configurations;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public abstract class BaseItemManager<TSettings, TItem, TValue> : BaseManager<TSettings>
        where TSettings : ItemSettings<TItem>
        where TItem : ItemValue<TValue>
    {
        protected BaseItemManager(DirectoryInfo pathRoot, DirectoryTemplate directoryTemplate) : base(pathRoot, directoryTemplate)
        {
        }

        public virtual async Task<bool> Has(string id)
        {
            TSettings? settings = await Settings;
            if (settings == null)
            {
                return false;
            }

            return settings.Items.ContainsKey(id);
        }

        public virtual async Task<TItem?> Get(string id)
        {
            TSettings? settings = await Settings;
            if (settings == null)
            {
                return null;
            }

            if (settings.Items.TryGetValue(id, out TItem? item))
            {
                await ConfigurateItem(item);
                return item;
            }
            else
            {
                return null;
            }
        }

        public virtual async Task Set(string id, TItem? value)
        {
            TSettings? settings = await Settings;
            if (settings == null)
            {
                throw new System.Exception("No settings");
            }

            if (value == null)
            {
                settings.Items.Remove(id);
            }
            else
            {
                if (settings.Items.ContainsKey(id))
                {
                    settings.Items[id] = value;
                }
                else
                {
                    settings.Items.Add(id, value);
                }
            }
            await SettingsLoader.Save(settings);
        }

        public abstract Task Install(string id, TValue item);

        public abstract Task Uninstall(string id);

        protected virtual Task ConfigurateItem(TItem item)
        {
            return Task.CompletedTask;
        }
    }
}
