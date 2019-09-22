using CodeRunner.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public abstract class ObjectFileLoader<T> : IObjectLoader<T> where T : class
    {
        private T? data;

        protected ObjectFileLoader(FileInfo file)
        {
            Assert.IsNotNull(file);
            File = file;
        }

        public FileInfo File { get; }

        public Task<T?> GetData()
        {
            if (data == null || LoadedTime == null)
            {
                return Load();
            }
            else
            {
                File.Refresh();
                if (File.LastWriteTime > LoadedTime)
                {
                    return Load();
                }
            }
            return Task.FromResult<T?>(data);
        }

        public DateTimeOffset? LoadedTime { get; set; }

        public async Task<T?> Load()
        {
            data = await OnLoading().ConfigureAwait(false);
            LoadedTime = DateTimeOffset.Now;
            return data;
        }

        public abstract Task Save(T value);

        protected abstract Task<T?> OnLoading();
    }
}
