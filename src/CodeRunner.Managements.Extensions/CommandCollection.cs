using CodeRunner.Diagnostics;
using CodeRunner.Extensions;
using CodeRunner.Extensions.Commands;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;

namespace CodeRunner.Managements.Extensions
{
    public class CommandCollection : IEnumerable<Command>
    {
        private Dictionary<ICommandBuilder, Command> Builders { get; set; } = new Dictionary<ICommandBuilder, Command>();

        private Dictionary<Command, ICommandBuilder> RBuilders { get; set; } = new Dictionary<Command, ICommandBuilder>();

        private Dictionary<Command, IExtension> Commands { get; set; } = new Dictionary<Command, IExtension>();

        public IEnumerator<Command> GetEnumerator() => Commands.Keys.GetEnumerator();

        public IExtension GetExtension(Command command) => Commands[command];

        public ICommandBuilder GetBuilder(Command command) => RBuilders[command];

        public bool Contains(Command command) => Commands.ContainsKey(command);

        public void Register(ICommandBuilder command, IExtension extension)
        {
            Assert.IsNotNull(command);
            Assert.IsNotNull(extension);
            Command builded = command.Build();
            Assert.IsNotNull(builded);

            Builders.Add(command, builded);
            RBuilders.Add(builded, command);
            Commands.Add(builded, extension);
        }

        public void Unregister(params Command[] commands)
        {
            Assert.IsNotNull(commands);
            foreach (Command cmd in commands)
            {
                Assert.IsNotNull(cmd);
                _ = Commands.Remove(cmd);
                if (RBuilders.TryGetValue(cmd, out ICommandBuilder? value))
                {
                    _ = RBuilders.Remove(cmd);
                    _ = Builders.Remove(value);
                }
            }
        }

        public void Unregister(IExtension extension)
        {
            Assert.IsNotNull(extension);
            List<Command> toRemoved = new List<Command>();
            foreach ((Command k, IExtension v) in Commands)
            {
                if (v == extension)
                    toRemoved.Add(k);
            }
            Unregister(toRemoved.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
