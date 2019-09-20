using System;
using System.Threading.Tasks;

namespace CodeRunner.IO
{
    public interface IObjectLoader<T> where T : class
    {
        Task<T?> GetData();

        DateTimeOffset? LoadedTime { get; set; }

        Task<T?> Load();

        Task Save(T value);
    }
}