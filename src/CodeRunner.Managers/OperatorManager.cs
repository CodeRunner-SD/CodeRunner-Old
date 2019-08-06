using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using CodeRunner.Templates;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class OperationManager
    {
        public OperationManager(DirectoryInfo pathRoot)
        {
            PathRoot = pathRoot;
            settingsLoader = new JsonFileLoader<OperationsSettings>(new FileInfo(Path.Join(PathRoot.FullName, Workspace.P_Settings)));
        }

        public DirectoryInfo PathRoot { get; }

        private readonly JsonFileLoader<OperationsSettings> settingsLoader;

        public Task<OperationsSettings?> Settings => settingsLoader.Data;

        public async Task Initialize()
        {
            await new OperationsSpaceTemplate().ResolveTo(new ResolveContext(), PathRoot.FullName);
        }

        public async Task<OperationItem?> GetItem(string id)
        {
            OperationsSettings? settings = await Settings;
            if (settings == null)
            {
                return null;
            }

            if (settings.Items.TryGetValue(id, out OperationItem? item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public async Task<Operation?> Get(OperationItem item)
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
