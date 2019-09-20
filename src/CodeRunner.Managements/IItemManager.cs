using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public interface IItemManager<TSettings, TItem> : IManager<TSettings> where TItem : class where TSettings : class
    {
        Task<bool> Has(string id);

        Task<TItem?> Get(string id);

        Task Set(string id, TItem? value);

        IAsyncEnumerable<string> GetKeys();

        IAsyncEnumerable<TItem?> GetValues();
    }
}
