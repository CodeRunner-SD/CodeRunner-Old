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

        private PackageFileLoaderPool<Operation> FileLoaderPool { get; } = new PackageFileLoaderPool<Operation>();

        protected override Task ConfigurateItem(OperationItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
