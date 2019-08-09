using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using System.IO;

namespace CodeRunner.Managers
{
    public class OperationManager : BaseItemManager<OperationsSettings, OperationItem, Operation?, OperationManager>
    {
        public OperationManager(DirectoryInfo pathRoot) : base(pathRoot, new OperationsSpaceTemplate())
        {
        }

        protected override OperationManager ItemParent => this;
    }
}
