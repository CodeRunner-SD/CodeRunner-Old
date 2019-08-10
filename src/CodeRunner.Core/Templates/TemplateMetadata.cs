using System;

namespace CodeRunner.Templates
{
    public class TemplateMetadata
    {
        public string Author { get; set; } = "";

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;

        public Version Version { get; set; } = new Version();
    }
}
