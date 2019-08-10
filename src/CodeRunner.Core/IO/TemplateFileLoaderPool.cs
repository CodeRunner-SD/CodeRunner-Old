using CodeRunner.Templates;
using System.IO;

namespace CodeRunner.IO
{
    public class TemplateFileLoaderPool<T> : FileLoaderPool<TemplateFileLoader<T>, T> where T : BaseTemplate
    {
        protected override TemplateFileLoader<T> Create(FileInfo file)
        {
            return new TemplateFileLoader<T>(file);
        }
    }
}
