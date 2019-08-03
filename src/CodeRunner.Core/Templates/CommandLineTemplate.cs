using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner.Templates
{
    public class CommandLineTemplate : BaseTemplate<string>
    {
        public CommandLineTemplate() : base(null)
        {
        }

        Dictionary<StringTemplate, StringTemplate> Options { get; } = new Dictionary<StringTemplate, StringTemplate>();

        public List<StringTemplate> Arguments { get; } = new List<StringTemplate>();

        List<StringTemplate> Flags { get; } = new List<StringTemplate>();

        public List<StringTemplate> Commands { get; } = new List<StringTemplate>();

        public StringTemplate? Raw { get; set; } = null;

        public CommandLineTemplate WithOption(StringTemplate id, StringTemplate value, string prefix = "")
        {
            id.Content = prefix + id.Content;
            var f = Options.Where(x => x.Key.Content == id.Content).Select(x => x.Key).FirstOrDefault();
            if (f == null)
                Options.Add(id, value);
            else
                Options[f] = value;
            return this;
        }

        public CommandLineTemplate WithOption(StringTemplate id, object value, string prefix = "")
        {
            return WithOption(id, value.ToString() ?? string.Empty, prefix);
        }

        public CommandLineTemplate WithoutOption(string fullContent)
        {
            var f = Options.Where(x => x.Key.Content == fullContent).Select(x => x.Key).FirstOrDefault();
            if (f != null)
                Options.Remove(f);
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
            var f = Flags.Where(x => x.Content == fullContent).FirstOrDefault();
            if (f != null)
                Flags.Remove(f);
            return this;
        }

        public override async Task<string> Resolve(TemplateResolveContext context)
        {
            List<string> items = new List<string>();
            foreach (var v in Commands)
                items.Add(await v.Resolve(context));
            foreach (var v in Arguments)
                items.Add(await v.Resolve(context));
            foreach (var v in Flags)
                items.Add(await v.Resolve(context));
            foreach (var (id, value) in Options)
            {
                items.Add(await id.Resolve(context));
                items.Add(await value.Resolve(context));
            }
            if (Raw != null)
                items.Add(await Raw!.Resolve(context));
            return string.Join(' ', items);
        }
    }
}
