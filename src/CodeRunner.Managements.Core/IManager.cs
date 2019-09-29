using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public interface IManager<TSettings> where TSettings : class
    {
        Task<TSettings?> Settings { get; }

        Task Initialize();

        Task Clear();
    }
}
