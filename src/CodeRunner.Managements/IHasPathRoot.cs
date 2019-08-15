using System.IO;

namespace CodeRunner.Managements
{
    public interface IHasPathRoot
    {
        DirectoryInfo PathRoot { get; }
    }
}
