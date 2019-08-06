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
            await new TemplatesSpaceTemplate().ResolveTo(new ResolveContext(), PathRoot.FullName);
        }

        public async Task<TemplateItem?> GetItem(string id)
        {
            TemplatesSettings? settings = await Settings;
            if (settings == null)
            {
                return null;
            }

            if (settings.Items.TryGetValue(id, out TemplateItem? item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public async Task<BaseTemplate?> Get(TemplateItem item)
        {
            switch (item.Type)
            {
                case TemplateType.PackageFile:
                    FileInfo file = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
                    if (file.Exists)
                    {
                        using (FileStream ss = file.OpenRead())
                        {
                            return await BaseTemplate.Load<PackageFileTemplate>(ss);
                        }
                    }
                    return null;
            }
            return null;
        }
    }
}
