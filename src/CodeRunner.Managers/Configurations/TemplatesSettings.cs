using System.Collections.Generic;

namespace CodeRunner.Managers.Configurations
{
    public class TemplatesSettings
    {
        public IDictionary<string, TemplateItem> Items { get; set; } = new Dictionary<string, TemplateItem>();
    }
}
