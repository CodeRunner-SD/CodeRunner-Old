using CodeRunner.IO;
using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public class Workspace : BaseManager<WorkspaceSettings>
    {
        public const string P_CRRoot = ".cr";
        public const string P_Settings = "settings.json";
        public const string P_TemplatesRoot = "templates";
        public const string P_OperatorsRoot = "operations";

        public Workspace(DirectoryInfo pathRoot) : base(pathRoot, new System.Lazy<CodeRunner.Templates.DirectoryTemplate>(() => new WorkspaceTemplate()))
        {
            CRRoot = new DirectoryInfo(Path.Join(pathRoot.FullName, P_CRRoot));
            Templates = new TemplateManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_TemplatesRoot)));
            Operations = new OperationManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_OperatorsRoot)));
            SettingsLoader = new JsonFileLoader<WorkspaceSettings>(new FileInfo(Path.Join(CRRoot.FullName, P_Settings)));
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

        public Task Clear()
        {
            if (CRRoot.Exists)
            {
                CRRoot.Delete(true);
            }

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
