using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class OperationManager : BaseItemManager<OperationsSettings, OperationItem, Operation>
    {
        public OperationManager(DirectoryInfo pathRoot) : base(pathRoot, new OperationsSpaceTemplate())
        {
        }

        public override async Task<Operation?> Get(OperationItem item)
        {
            FileInfo file = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
            if (file.Exists)
            {
                using FileStream ss = file.OpenRead();
                return await BaseTemplate.Load<Operation>(ss);
            }
            return null;
        }
    }
}
