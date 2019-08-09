using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
    public class TemplateItem : ItemValue<BaseTemplate?, TemplateManager>
    {
        public string FileName { get; set; } = "";

        public TemplateType Type { get; set; }

        protected override async Task<BaseTemplate?> GetValue()
        {
            if (Parent != null)
            {
                switch (Type)
                {
                    case TemplateType.File:
                        {
                            FileInfo file = new FileInfo(Path.Join(Parent.PathRoot.FullName, FileName));
                            if (file.Exists)
                            {
                                using FileStream ss = file.OpenRead();
                                return await BaseTemplate.Load<PackageFileTemplate>(ss);
                            }
                            break;
                        }
                    case TemplateType.Directory:
                        {
                            FileInfo file = new FileInfo(Path.Join(Parent.PathRoot.FullName, FileName));
                            if (file.Exists)
                            {
                                using FileStream ss = file.OpenRead();
                                return await BaseTemplate.Load<PackageDirectoryTemplate>(ss);
                            }
                            break;
                        }
                }
            }
            return null;
        }
    }
}
