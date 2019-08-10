using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class TemplateManager : BaseItemManager<TemplatesSettings, TemplateItem, BaseTemplate?>
    {
        public TemplateManager(DirectoryInfo pathRoot) : base(pathRoot, new TemplatesSpaceTemplate())
        {
        }

        private TemplateFileLoaderPool<BaseTemplate> FileLoaderPool { get; } = new TemplateFileLoaderPool<BaseTemplate>();

        protected override Task ConfigurateItem(TemplateItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
