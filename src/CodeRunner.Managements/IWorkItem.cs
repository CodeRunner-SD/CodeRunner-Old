using System;

namespace CodeRunner.Managements
{
    public interface IWorkItem
    {
        Guid Id { get; }

        string Name { get; }
    }
}
