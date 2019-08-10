using CodeRunner.Templates;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public class TemplateFileLoader<T> : ObjectFileLoader<T> where T : BaseTemplate
    {
        public TemplateFileLoader(FileInfo file) : base(file)
        {
        }

        protected override async Task<T?> OnLoading()
        {
            try
            {
                using FileStream st = File.OpenRead();
                return await BaseTemplate.Load<T>(st);
            }
            catch
            {
                return null;
            }
        }

        public override async Task Save(T value)
        {
            using FileStream st = File.OpenWrite();
            await value.Save(st);
            File.LastWriteTimeUtc = DateTime.UtcNow;
        }
    }
}
