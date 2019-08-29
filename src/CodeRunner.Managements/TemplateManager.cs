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
        public TemplateManager(DirectoryInfo pathRoot) : base(pathRoot, new System.Lazy<DirectoryTemplate>(() => new TemplatesSpaceTemplate())) => SettingsLoader = new JsonFileLoader<TemplatesSettings>(new FileInfo(Path.Join(PathRoot.FullName, Workspace.P_Settings)));

        private PackageFileLoaderPool<BaseTemplate> FileLoaderPool { get; } = new PackageFileLoaderPool<BaseTemplate>();

        public override async Task Install(string id, Package<BaseTemplate>? item)
        {
            if (item == null)
            {
                return;
            }

            await Set(id, new TemplateItem
            {
                FileName = $"{id}.tpl",
            });
            TemplateItem? wrap = await Get(id);
            if (wrap == null)
            {
                return;
            }

            await wrap.SetValue(item);
        }

        public override async Task Uninstall(string id)
        {
            TemplateItem? wrap = await Get(id);
            if (wrap == null)
            {
                return;
            }

            await wrap.SetValue(null);
            await Set(id, null);
        }

        protected override Task ConfigurateItem(TemplateItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
