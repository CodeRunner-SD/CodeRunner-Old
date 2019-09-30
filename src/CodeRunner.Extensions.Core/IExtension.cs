using System;

namespace CodeRunner.Extensions
{
    public interface IExtension
    {
        string Name { get; }

        string Publisher { get; }

        string Description { get; }

        Version Version { get; }
    }
}
