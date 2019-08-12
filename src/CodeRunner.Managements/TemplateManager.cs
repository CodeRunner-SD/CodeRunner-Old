using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.Templates;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public class TemplateManager : BaseItemManager<TemplatesSettings, TemplateItem, Package<BaseTemplate>?>
    {
        public TemplateManager(DirectoryInfo pathRoot) : base(pathRoot, new TemplatesSpaceTemplate())
        {
        }

        private PackageFileLoaderPool<BaseTemplate> FileLoaderPool { get; } = new PackageFileLoaderPool<BaseTemplate>();

        protected override Task ConfigurateItem(TemplateItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
