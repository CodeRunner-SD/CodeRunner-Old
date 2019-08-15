using CodeRunner.IO;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public abstract class BaseManager<TSettings> : IHasPathRoot where TSettings : class
    {
        protected BaseManager(DirectoryInfo pathRoot, DirectoryTemplate? directoryTemplate)
        {
            PathRoot = pathRoot;
            DirectoryTemplate = directoryTemplate;
            SettingsLoader = new JsonFileLoader<TSettings>(new FileInfo(Path.Join(PathRoot.FullName, Workspace.P_Settings)));
        }

        protected JsonFileLoader<TSettings> SettingsLoader { get; set; }

        protected DirectoryTemplate? DirectoryTemplate { get; }

        public DirectoryInfo PathRoot { get; }

        public Task<TSettings?> Settings => SettingsLoader.Data;

        public virtual async Task Initialize()
        {
            if (DirectoryTemplate != null)
            {
                await DirectoryTemplate.ResolveTo(new ResolveContext(), PathRoot.FullName);
            }
        }
    }
}
