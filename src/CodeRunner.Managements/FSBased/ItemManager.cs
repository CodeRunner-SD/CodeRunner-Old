using CodeRunner.IO;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased
{
    public abstract class ItemManager<TSettings, TValue> : Manager<TSettings>, IItemManager<TSettings, Package<TValue>>
        where TSettings : class where TValue : class
    {
        public ItemManager(DirectoryInfo pathRoot, Lazy<DirectoryTemplate>? directoryTemplate = null) : base(pathRoot, directoryTemplate)
        {
            SettingsLoader = new JsonFileLoader<TSettings>(
                new FileInfo(Path.Join(PathRoot.FullName, Workspace.P_Settings)));
            ListLoader = new JsonFileLoader<Dictionary<string, FileItemValue>>(
                new FileInfo(Path.Join(PathRoot.FullName, "list.json")));
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await ListLoader.Save(new Dictionary<string, FileItemValue>());
        }

        protected IObjectLoader<Dictionary<string, FileItemValue>> ListLoader { get; set; }

        private PackageFileLoaderPool<TValue> FileLoaderPool { get; } = new PackageFileLoaderPool<TValue>();

        private Task<Package<TValue>?> Load(FileItemValue item)
        {
            FileInfo fi = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
            return FileLoaderPool.Get(fi).GetData();
        }

        private Task Set(FileItemValue item, Package<TValue>? value)
        {
            FileInfo fi = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
            if (value == null)
            {
                fi.Delete();
                return Task.CompletedTask;
            }
            else
            {
                return FileLoaderPool.Get(fi).Save(value);
            }
        }

        public async Task<Package<TValue>?> Get(string id)
        {
            Dictionary<string, FileItemValue>? list = await ListLoader.GetData();
            if (list == null)
                return null;
            if (list.TryGetValue(id, out FileItemValue? item))
            {
                return await Load(item);
            }
            else
            {
                return null;
            }
        }

        public async IAsyncEnumerable<string> GetKeys()
        {
            Dictionary<string, FileItemValue>? list = await ListLoader.GetData();
            if (list != null)
            {
                foreach (string v in list.Keys)
                    yield return v;
            }
        }

        public async IAsyncEnumerable<Package<TValue>?> GetValues()
        {
            Dictionary<string, FileItemValue>? list = await ListLoader.GetData();
            if (list != null)
            {
                foreach (FileItemValue v in list.Values)
                    yield return await Load(v);
            }
        }

        public async Task<bool> Has(string id)
        {
            Dictionary<string, FileItemValue>? list = await ListLoader.GetData();
            if (list == null)
                return false;
            return list.ContainsKey(id);
        }

        public async Task Set(string id, Package<TValue>? value)
        {
            Dictionary<string, FileItemValue>? list = await ListLoader.GetData();
            if (list == null)
                return;
            if (list.TryGetValue(id, out FileItemValue? item))
            {
                await Set(item, value);
                if (value == null)
                {
                    _ = list.Remove(id);
                    await ListLoader.Save(list);
                }
            }
            else if (value != null)
            {
                FileItemValue nitem = new FileItemValue { FileName = id };
                await Set(nitem, value);
                list.Add(id, nitem);
                await ListLoader.Save(list);
            }
        }
    }
}
