using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRunner.Commands
{
    public class CommandLineBuilder
    {
        Dictionary<string, string> Options { get; } = new Dictionary<string, string>();

        public List<string> Arguments { get; } = new List<string>();

        public List<string> Flags { get; } = new List<string>();

        public List<string> Command { get; } = new List<string>();

        public string? Raw { get; set; } = null;

        public void AddOption(string id, string value)
        {
            if (Options.ContainsKey(id))
                Options[id] = value;
            else
                Options.Add(id, value);
        }

        public void RemoveOption(string id)
        {
            Options.Remove(id);
        }

        public void AddOption(string id, object value)
        {
            AddOption(id, value.ToString() ?? string.Empty);
        }

        public override string ToString()
        {
            List<string> items = new List<string>();
            items.AddRange(Command);
            items.AddRange(Arguments);
            foreach (var id in Flags)
            {
                if (id.Length <= 1)
                {
                    items.Add($"-{id}");
                }
                else
                {
                    items.Add($"--{id}");
                }
            }
            foreach (var (id, value) in Options)
            {
                if (id.Length <= 1)
                {
                    items.Add($"-{id}");
                }
                else
                {
                    items.Add($"--{id}");
                }
                items.Add(value);
            }
            if (Raw != null) items.Add(Raw!);
            return string.Join(" ", items);
        }
    }
}
