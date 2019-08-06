using System.Collections.Generic;

namespace CodeRunner.Managers.Configurations
{
    public class OperationsSettings
    {
        public IDictionary<string, OperationItem> Items { get; set; } = new Dictionary<string, OperationItem>();
    }
}
