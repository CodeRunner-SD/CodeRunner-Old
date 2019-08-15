using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.Templates;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public class OperationManager : BaseItemManager<OperationsSettings, OperationItem, Package<Operation>?>
    {
        public OperationManager(DirectoryInfo pathRoot) : base(pathRoot, new OperationsSpaceTemplate())
        {
        }

        public override async Task Install(string id, Package<Operation>? item)
        {
            if (item == null)
            {
                return;
            }

            await Set(id, new OperationItem
            {
                FileName = $"{id}.tpl",
            });
            OperationItem wrap = await Get(id);
            if (wrap == null)
            {
                return;
            }

            await wrap.SetValue(item);
        }

        public override async Task Uninstall(string id)
        {
            OperationItem wrap = await Get(id);
            if (wrap == null)
            {
                return;
            }

            await wrap.SetValue(null);
            await Set(id, null);
        }

        private PackageFileLoaderPool<Operation> FileLoaderPool { get; } = new PackageFileLoaderPool<Operation>();

        protected override Task ConfigurateItem(OperationItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
