using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public abstract class ObjectFileLoader<T> where T : class
    {
        private T? data;

        protected ObjectFileLoader(FileInfo file)
        {
            File = file;
        }

        public FileInfo File { get; }

        public Task<T?> Data
        {
            get
            {
                if (data == null || LoadedTime == null)
                    return Load();
                else
                {
                    File.Refresh();
                    if (File.LastWriteTime > LoadedTime)
                        return Load();
                }
                return Task.FromResult<T?>(data);
            }
        }

        public DateTimeOffset? LoadedTime { get; set; }

        public async Task<T?> Load()
        {
            LoadedTime = DateTimeOffset.Now;
            data = await OnLoading();
            return data;
        }

        protected abstract Task<T?> OnLoading();
    }
}
