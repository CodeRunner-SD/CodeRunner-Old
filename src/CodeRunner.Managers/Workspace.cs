using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class Workspace : BaseManager<AppSettings>
    {
        public const string P_CRRoot = ".cr";
        public const string P_Settings = "settings.json";
        public const string P_TemplatesRoot = "templates";
        public const string P_OperatorsRoot = "operations";

        public Workspace(DirectoryInfo pathRoot) : base(pathRoot, new WorkspaceTemplate())
        {
            CRRoot = new DirectoryInfo(Path.Join(pathRoot.FullName, P_CRRoot));
            SettingsLoader = new JsonFileLoader<AppSettings>(new FileInfo(Path.Join(CRRoot.FullName, P_Settings)));
            Templates = new TemplateManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_TemplatesRoot)));
            Operations = new OperationManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_OperatorsRoot)));
        }

        private DirectoryInfo CRRoot { get; set; }

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

        public Task<bool> CheckValid()
        {
            PathRoot.Refresh();
            CRRoot.Refresh();

            return Task.FromResult(PathRoot.Exists && CRRoot.Exists);
        }

        public Task Clear()
        {
            if (CRRoot.Exists)
                CRRoot.Delete(true);
            return Task.CompletedTask;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await Templates.Initialize();
            await Operations.Initialize();
        }
    }
}
