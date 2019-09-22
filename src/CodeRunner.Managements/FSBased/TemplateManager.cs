using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.FSBased.Templates;
using CodeRunner.Templates;
using System.IO;

namespace CodeRunner.Managements.FSBased
{
    public class TemplateManager : ItemManager<TemplateSettings, BaseTemplate>, ITemplateManager
    {
        public TemplateManager(DirectoryInfo pathRoot) : base(pathRoot, new System.Lazy<DirectoryTemplate>(() => new TemplatesSpaceTemplate()))
        {
        }
    }
}
