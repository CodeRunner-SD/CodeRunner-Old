using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class Workspace
    {
        public const string P_CRRoot = ".cr";
        public const string P_Settings = "settings.json";
        public const string P_TemplatesRoot = "templates";
        public const string P_OperatorsRoot = "operators";

        private readonly JsonFileLoader<AppSettings> settingsLoader;

        public Workspace(DirectoryInfo pathRoot)
        {
            PathRoot = pathRoot;
            CRRoot = new DirectoryInfo(Path.Join(pathRoot.FullName, P_CRRoot));
            Templates = new TemplateManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_TemplatesRoot)));
            Operations = new OperationManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_OperatorsRoot)));
            settingsLoader = new JsonFileLoader<AppSettings>(new FileInfo(Path.Join(CRRoot.FullName, P_Settings)));
        }

        public DirectoryInfo PathRoot { get; }

        public Task<AppSettings?> Settings => settingsLoader.Data;

        public TemplateManager Templates { get; }

        public OperationManager Operations { get; }

        public bool HasInitialized
        {
            get
            {
                CRRoot.Refresh();
                return CRRoot.Exists;
            }
        }

        private DirectoryInfo CRRoot { get; set; }

        public Task<bool> CheckValid()
        {
            PathRoot.Refresh();
            CRRoot.Refresh();

            return Task.FromResult(PathRoot.Exists && CRRoot.Exists);
        }

        public async Task Initialize()
        {
            await new WorkspaceTemplate().ResolveTo(new CodeRunner.Templates.ResolveContext(), PathRoot.FullName);
            await Templates.Initialize();
            await Operations.Initialize();
        }
    }
}
