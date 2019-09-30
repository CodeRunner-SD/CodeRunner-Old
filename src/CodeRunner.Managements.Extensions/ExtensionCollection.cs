using CodeRunner.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeRunner.Managements.Extensions
{
    public class ExtensionCollection : IEnumerable<ExtensionLoader>
    {
        private Dictionary<string, ExtensionLoader> Loaders { get; set; } = new Dictionary<string, ExtensionLoader>();

        public void Load(ExtensionLoader loader)
        {
            Assert.IsNotNull(loader);
            loader.Load();
            Assert.IsNotNull(loader.Assembly?.FullName);
            Loaders.Add(loader.Assembly.FullName, loader);
        }

        public void Unload(ExtensionLoader loader)
        {
            Assert.IsNotNull(loader?.Assembly?.FullName);
            _ = Loaders.Remove(loader.Assembly.FullName);
            loader.Unload();
        }

        public ExtensionLoader Get(Type type)
        {
            Assert.IsNotNull(type?.Assembly.FullName);
            string key = type.Assembly.FullName;
            return Loaders[key];
        }

        public bool TryGet(Type type, [NotNullWhen(true), MaybeNullWhen(false)] out ExtensionLoader? value)
        {
            Assert.IsNotNull(type?.Assembly.FullName);
            string key = type.Assembly.FullName;
            return Loaders.TryGetValue(key, out value);
        }

        public IEnumerator<ExtensionLoader> GetEnumerator() => Loaders.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
