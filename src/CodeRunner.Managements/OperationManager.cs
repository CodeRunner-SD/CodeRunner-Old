using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.Templates;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public class OperationManager : BaseItemManager<OperationsSettings, OperationItem, Package<BaseOperation>?>
    {
        public OperationManager(DirectoryInfo pathRoot) : base(pathRoot, new System.Lazy<DirectoryTemplate>(() => new OperationsSpaceTemplate()))
        {
            SettingsLoader = new JsonFileLoader<OperationsSettings>(
                new FileInfo(Path.Join(PathRoot.FullName, Workspace.P_Settings)));
        }

        public override async Task Install(string id, Package<BaseOperation>? item)
        {
            if (item == null)
            {
                return;
            }

            await Set(id, new OperationItem
            {
                FileName = $"{id}.tpl",
            });
            OperationItem? wrap = await Get(id);
            if (wrap == null)
            {
                return;
            }

            await wrap.SetValue(item);
        }

        public override async Task Uninstall(string id)
        {
            OperationItem? wrap = await Get(id);
            if (wrap == null)
            {
                return;
            }

            await wrap.SetValue(null);
            await Set(id, null);
        }

        private PackageFileLoaderPool<BaseOperation> FileLoaderPool { get; } = new PackageFileLoaderPool<BaseOperation>();

        protected override Task ConfigurateItem(OperationItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
