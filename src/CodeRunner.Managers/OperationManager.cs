using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class OperationManager : BaseItemManager<OperationsSettings, OperationItem, Operation?>
    {
        public OperationManager(DirectoryInfo pathRoot) : base(pathRoot, new OperationsSpaceTemplate())
        {
        }

        private TemplateFileLoaderPool<Operation> FileLoaderPool { get; } = new TemplateFileLoaderPool<Operation>();

        protected override Task ConfigurateItem(OperationItem item)
        {
            item.Parent = this;
            item.FileLoaderPool = FileLoaderPool;
            return base.ConfigurateItem(item);
        }
    }
}
