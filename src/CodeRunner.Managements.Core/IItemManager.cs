using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public interface IItemManager<TSettings, TItem> : IManager<TSettings> where TItem : class where TSettings : class
    {
        Task<bool> HasKey(string id);

        Task<TItem?> GetValue(string id);

        Task SetValue(string id, TItem? value);

        IAsyncEnumerable<string> GetKeys();

        IAsyncEnumerable<TItem?> GetValues();
    }
}
