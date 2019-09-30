using System.CommandLine;

namespace CodeRunner.Extensions.Commands
{
    public interface ICommandBuilder
    {
        Command Build();
    }
}
