using CodeRunner.IO;
using CodeRunner.Templates;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public abstract class BaseManager<TSettings> : IHasPathRoot where TSettings : class
    {
        protected BaseManager(DirectoryInfo pathRoot, Lazy<DirectoryTemplate>? directoryTemplate = null)
        {
            PathRoot = pathRoot;
            DirectoryTemplate = directoryTemplate;
        }

        protected IObjectLoader<TSettings>? SettingsLoader { get; set; }

        protected Lazy<DirectoryTemplate>? DirectoryTemplate { get; }

        public DirectoryInfo PathRoot { get; }

        public Task<TSettings?> Settings => SettingsLoader == null ? Task.FromResult<TSettings?>(null) : SettingsLoader.Data;

        public virtual async Task Initialize()
        {
            if (DirectoryTemplate != null)
            {
                _ = await DirectoryTemplate.Value.ResolveTo(new ResolveContext(), PathRoot.FullName);
            }
        }
    }
}
