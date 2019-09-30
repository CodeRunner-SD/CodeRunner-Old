using System;

namespace CodeRunner.Extensions
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class EntryExtensionAttribute : Attribute
    {
        public Type Extension { get; }

        public EntryExtensionAttribute(Type extensionType) => Extension = extensionType;
    }
}
