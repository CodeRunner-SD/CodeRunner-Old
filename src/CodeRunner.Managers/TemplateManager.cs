using CodeRunner.Managers.Configurations;
using CodeRunner.Managers.Templates;
using CodeRunner.Templates;
using System.IO;

namespace CodeRunner.Managers
{
    public class TemplateManager : BaseItemManager<TemplatesSettings, TemplateItem, BaseTemplate?, TemplateManager>
    {
        public TemplateManager(DirectoryInfo pathRoot) : base(pathRoot, new TemplatesSpaceTemplate())
        {
        }

        protected override TemplateManager ItemParent => this;
    }
}
