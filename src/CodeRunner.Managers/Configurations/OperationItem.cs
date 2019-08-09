using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managers.Configurations
{
    public class OperationItem : ItemValue<Operation?, OperationManager>
    {
        public string FileName { get; set; } = "";

        protected override async Task<Operation?> GetValue()
        {
            if (Parent != null)
            {
                FileInfo file = new FileInfo(Path.Join(Parent.PathRoot.FullName, FileName));
                if (file.Exists)
                {
                    using FileStream ss = file.OpenRead();
                    return await BaseTemplate.Load<Operation>(ss);
                }
                return null;
            }
            return null;
        }
    }
}
