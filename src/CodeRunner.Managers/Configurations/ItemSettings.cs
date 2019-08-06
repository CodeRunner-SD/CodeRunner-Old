using System.Collections.Generic;

namespace CodeRunner.Managers.Configurations
{
    public abstract class ItemSettings<TItem>
    {
        public IDictionary<string, TItem> Items { get; set; } = new Dictionary<string, TItem>();
    }
}
