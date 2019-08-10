using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.Templates;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
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
