using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers
{
    public class TemplateManager : BaseItemManager<TemplatesSettings, TemplateItem, BaseTemplate>
    {
        public TemplateManager(DirectoryInfo pathRoot) : base(pathRoot, new TemplatesSpaceTemplate())
        {
        }

        public override async Task<BaseTemplate?> Get(TemplateItem item)
        {
            switch (item.Type)
            {
                case TemplateType.File:
                    {
                        FileInfo file = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
                        if (file.Exists)
                        {
                            using FileStream ss = file.OpenRead();
                            return await BaseTemplate.Load<PackageFileTemplate>(ss);
                        }
                        return null;
                    }
                case TemplateType.Directory:
                    {
                        FileInfo file = new FileInfo(Path.Join(PathRoot.FullName, item.FileName));
                        if (file.Exists)
                        {
                            using FileStream ss = file.OpenRead();
                            return await BaseTemplate.Load<PackageDirectoryTemplate>(ss);
                        }
                        return null;
                    }
            }
            return null;
        }
    }
}
