using CodeRunner.Diagnostics;
using System.Collections.Generic;
using System.IO;

namespace CodeRunner.IO
{
    public abstract class FileLoaderPool<TLoader, TTarget> where TTarget : class where TLoader : ObjectFileLoader<TTarget>
    {
        private Dictionary<string, TLoader> Pool { get; set; } = new Dictionary<string, TLoader>();

        protected abstract TLoader Create(FileInfo file);

        public TLoader Get(FileInfo file)
        {
            Assert.IsNotNull(file);

            if (Pool.TryGetValue(file.FullName, out TLoader? value))
            {
                return value;
            }
            else
            {
                TLoader item = Create(file);
                Pool.Add(file.FullName, item);
                return item;
            }
        }
    }
}
