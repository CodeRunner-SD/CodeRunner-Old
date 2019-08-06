using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class CommandLineTemplate : BaseTemplate<string>
    {
        private Dictionary<StringTemplate, StringTemplate> Options { get; } = new Dictionary<StringTemplate, StringTemplate>();

        public List<StringTemplate> Arguments { get; set; } = new List<StringTemplate>();

        private List<StringTemplate> Flags { get; set; } = new List<StringTemplate>();

        public List<StringTemplate> Commands { get; set; } = new List<StringTemplate>();

        public StringTemplate? Raw { get; set; } = null;

        public CommandLineTemplate UseCommand(StringTemplate command)
        {
            Commands.Add(command);
            return this;
        }

        public CommandLineTemplate UseArgument(StringTemplate argument)
        {
            Arguments.Add(argument);
            return this;
        }

        public CommandLineTemplate WithOption(StringTemplate id, StringTemplate value, string prefix = "")
        {
            id.Content = prefix + id.Content;
            StringTemplate f = Options.Where(x => x.Key.Content == id.Content).Select(x => x.Key).FirstOrDefault();
            if (f == null)
            {
                Options.Add(id, value);
            }
            else
            {
                Options[f] = value;
            }

            return this;
        }

        public CommandLineTemplate WithOption(StringTemplate id, object value, string prefix = "")
        {
            return WithOption(id, value.ToString() ?? string.Empty, prefix);
        }

        public CommandLineTemplate WithoutOption(string fullContent)
        {
            StringTemplate f = Options.Where(x => x.Key.Content == fullContent).Select(x => x.Key).FirstOrDefault();
            if (f != null)
            {
                Options.Remove(f);
            }

            return this;
        }

        public CommandLineTemplate WithFlag(StringTemplate id, string prefix = "")
        {
            id.Content = prefix + id.Content;
            Flags.Add(id);
            return this;
        }

        public CommandLineTemplate WithoutFlag(string fullContent)
        {
            StringTemplate f = Flags.Where(x => x.Content == fullContent).FirstOrDefault();
            if (f != null)
            {
                Flags.Remove(f);
            }

            return this;
        }

        public override async Task<string> Resolve(ResolveContext context)
        {
            List<string> items = new List<string>();
            foreach (StringTemplate v in Commands)
            {
                items.Add(await v.Resolve(context));
            }

            foreach (StringTemplate v in Arguments)
            {
                items.Add(await v.Resolve(context));
            }

            foreach (StringTemplate v in Flags)
            {
                items.Add(await v.Resolve(context));
            }

            foreach ((StringTemplate id, StringTemplate value) in Options)
            {
                items.Add(await id.Resolve(context));
                items.Add(await value.Resolve(context));
            }
            if (Raw != null)
            {
                items.Add(await Raw!.Resolve(context));
            }

            return string.Join(' ', items);
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Collect(Commands);
            res.Collect(Flags);
            res.Collect(Arguments);
            res.Collect(Options.Keys);
            res.Collect(Options.Values);
            if (Raw != null)
            {
                res.Collect(Raw!);
            }

            return res;
        }
    }
}
