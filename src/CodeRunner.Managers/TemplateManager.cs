using CodeRunner.IO;
using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class TemplateManager
    {
        public TemplateManager(DirectoryInfo pathRoot)
        {
            PathRoot = pathRoot;
            settingsLoader = new JsonFileLoader<TemplatesSettings>(new FileInfo(Path.Join(PathRoot.FullName, Workspace.P_Settings)));
        }

        public DirectoryInfo PathRoot { get; }

        private readonly JsonFileLoader<TemplatesSettings> settingsLoader;

        public Task<TemplatesSettings?> Settings => settingsLoader.Data;

        public async Task Initialize()
        {
            await new TemplatesSpaceTemplate().ResolveTo(new TemplateResolveContext(), PathRoot.FullName);
        }

        public async Task<T?> Get<T>(string id) where T : class
        {
            var settings = await Settings;
            if (settings == null) return null;
            if (settings.Items.TryGetValue(id, out var item))
            {
                switch (item.Type)
                {
                    case TemplateType.TextFile:
                        if (typeof(T).IsAssignableFrom(typeof(TextFileTemplate)))
                        {
                            var file = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
                            if (file.Exists)
                            {
                                using var ss = file.OpenRead();
                                return (await BaseTemplate.Load<TextFileTemplate>(ss)) as T;
                            }
                        }
                        return null;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
