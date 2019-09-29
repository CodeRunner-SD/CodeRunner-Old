using CodeRunner.IO;
using CodeRunner.Templates;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased
{
    public abstract class Manager<TSettings> : IManager<TSettings> where TSettings : class
    {
        protected Manager(DirectoryInfo pathRoot, Lazy<DirectoryTemplate>? directoryTemplate = null)
        {
            PathRoot = pathRoot;
            DirectoryTemplate = directoryTemplate;
        }

        protected IObjectLoader<TSettings>? SettingsLoader { get; set; }

        private Lazy<DirectoryTemplate>? DirectoryTemplate { get; }

        public DirectoryInfo PathRoot { get; protected set; }

        public Task<TSettings?> Settings => SettingsLoader == null ? Task.FromResult<TSettings?>(null) : SettingsLoader.GetData();

        public virtual async Task Initialize()
        {
            if (DirectoryTemplate != null)
            {
                _ = await DirectoryTemplate.Value.ResolveTo(new ResolveContext(), PathRoot.FullName).ConfigureAwait(false);
            }
        }

        public virtual Task Clear()
        {
            if (PathRoot.Exists)
            {
                PathRoot.Delete(true);
            }

            return Task.CompletedTask;
        }
    }
}
