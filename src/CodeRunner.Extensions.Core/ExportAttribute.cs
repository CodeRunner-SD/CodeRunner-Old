using System;

namespace CodeRunner.Extensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ExportAttribute : Attribute
    {
        public ExportAttribute() { }
    }
}
