using CodeRunner.Diagnostics;
using CodeRunner.Extensions;
using CodeRunner.Extensions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace CodeRunner.Managements.Extensions
{
    internal class ExtensionLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver Resolver { get; }

        public ExtensionLoadContext(string extensionPath) => Resolver = new AssemblyDependencyResolver(extensionPath);

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = Resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = Resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath != null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
        }
    }

    public class ExtensionLoader
    {
        private ExtensionLoadContext? Context { get; set; }

        public Assembly? Assembly { get; private set; }

        private readonly Lazy<IExtension> _extension;
        private readonly Lazy<List<ICommandBuilder>> _commands;

        public IExtension Extension => _extension.Value;

        public IEnumerable<ICommandBuilder> Commands => _commands.Value.AsEnumerable();

        public ExtensionLoader(string path, string name)
        {
            Assert.IsNotNull(path);
            Assert.IsNotNull(name);

            Path = path;
            Name = name;
            _extension = new Lazy<IExtension>(() => GetExtension());
            _commands = new Lazy<List<ICommandBuilder>>(() => GetCommands().ToList());
        }

        public ExtensionLoader(Assembly assembly)
        {
            Assert.IsNotNull(assembly);

            Assembly = assembly;
            _extension = new Lazy<IExtension>(() => GetExtension());
            _commands = new Lazy<List<ICommandBuilder>>(() => GetCommands().ToList());
        }

        public string? Path { get; }

        public string? Name { get; }

        private IExtension GetExtension()
        {
            Assert.IsNotNull(Assembly);
            EntryExtensionAttribute? entry = Assembly.GetCustomAttribute<EntryExtensionAttribute>();
            if (entry == null)
            {
                throw new ApplicationException("The assembly has no extension.");
            }
            if (!typeof(IExtension).IsAssignableFrom(entry.Extension))
            {
                throw new ApplicationException("The entry extension type is not an extension.");
            }
            if (Activator.CreateInstance(entry.Extension) is IExtension result)
            {
                return result;
            }
            else
            {
                throw new ApplicationException("Can't load the extension.");
            }
        }

        private IEnumerable<ICommandBuilder> GetCommands()
        {
            Assert.IsNotNull(Assembly);
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.GetCustomAttribute<ExportAttribute>() != null)
                {
                    if (typeof(ICommandBuilder).IsAssignableFrom(type))
                    {
                        if (Activator.CreateInstance(type) is ICommandBuilder result)
                        {
                            yield return result;
                        }
                    }
                }
            }
        }

        public void Load()
        {
            Assert.IsNull(Context);
            if (Assembly == null && Path != null && Name != null)
            {
                Context = new ExtensionLoadContext(Path);
                Assembly = Context.LoadFromAssemblyName(new AssemblyName(Name));
            }
        }

        public void Unload()
        {
            if (Context != null)
            {
                Assembly = null;
                Context.Unload();
            }
        }
    }
}
