using CodeRunner.Diagnostics;
using CodeRunner.IO;
using CodeRunner.Loggings;
using CodeRunner.Managements.Configurations;
using CodeRunner.Managements.FSBased.Templates;
using CodeRunner.Operations;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased
{
    public class Workspace : Manager<WorkspaceSettings>, IWorkspace
    {
        public const string PCRRoot = ".crw";
        public const string PSettings = "settings.json";
        public const string PTemplatesRoot = "templates";
        public const string POperatorsRoot = "operations";

        public Workspace(DirectoryInfo pathRoot) : base(pathRoot, new Lazy<DirectoryTemplate>(() => new WorkspaceTemplate()))
        {
            Assert.IsNotNull(pathRoot);

            CRRoot = new DirectoryInfo(Path.Join(pathRoot.FullName, PCRRoot));
            Templates = new TemplateManager(new DirectoryInfo(Path.Join(CRRoot.FullName, PTemplatesRoot)));
            Operations = new OperationManager(new DirectoryInfo(Path.Join(CRRoot.FullName, POperatorsRoot)));
            SettingsLoader = new JsonFileLoader<WorkspaceSettings>(new FileInfo(Path.Join(CRRoot.FullName, PSettings)));
        }

        private DirectoryInfo CRRoot { get; set; }

        public ITemplateManager Templates { get; }

        public IOperationManager Operations { get; }

        public override async Task Clear()
        {
            await Templates.Clear().ConfigureAwait(false);
            await Operations.Clear().ConfigureAwait(false);

            if (CRRoot.Exists)
            {
                CRRoot.Delete(true);
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await Templates.Initialize().ConfigureAwait(false);
            await Operations.Initialize().ConfigureAwait(false);
        }

        public async Task<IWorkItem?> Create(string name, BaseTemplate? from, Func<VariableCollection, ResolveContext, Task> resolveCallback)
        {
            Assert.IsNotNull(resolveCallback);
            ResolveContext context = new ResolveContext()
                .WithVariable(nameof(name), name)
                .WithVariable(DirectoryTemplate.Var, PathRoot.FullName);
            if (from == null)
            {
                from = new RegisterWorkItemTemplate();
                _ = context.WithVariable(RegisterWorkItemTemplate.Workspace, this);
            }
            await resolveCallback(from.GetVariables(), context).ConfigureAwait(false);
            IWorkItem? res;
            switch (from)
            {
                case FileTemplate ft:
                    FileInfo f = await ft.Resolve(context).ConfigureAwait(false);
                    res = WorkItem.CreateByFile(this, f);
                    break;
                case DirectoryTemplate dt:
                    DirectoryInfo d = await dt.Resolve(context).ConfigureAwait(false);
                    res = WorkItem.CreateByDirectory(this, d);
                    break;
                case RegisterWorkItemTemplate rt:
                    res = await rt.Resolve(context).ConfigureAwait(false);
                    break;
                default:
                    await from.DoResolve(context).ConfigureAwait(false);
                    res = null;
                    break;
            }
            return res;
        }

        public async Task<PipelineResult<Wrapper<bool>>> Execute(IWorkItem? workItem, BaseOperation from, Func<VariableCollection, ResolveContext, Task> resolveCallback, OperationWatcher watcher, ILogger logger)
        {
            Assert.IsNotNull(from);
            Assert.IsNotNull(resolveCallback);

            ResolveContext context = new ResolveContext();
            WorkspaceSettings? settings = await Settings.ConfigureAwait(false);
            if (settings != null)
            {
                _ = context.SetShell(settings.DefaultShell);
            }
            _ = context.SetWorkingDirectory(PathRoot.FullName);

            if (workItem != null && workItem is WorkItem item)
            {
                _ = context.SetInputPath(item.RelativePath);
            }
            await resolveCallback(from.GetVariables(), context).ConfigureAwait(false);
            PipelineBuilder<OperationWatcher, Wrapper<bool>> builder = await from.Resolve(context).ConfigureAwait(false);
            Pipeline<OperationWatcher, Wrapper<bool>> pipeline = await builder.Build(watcher, logger).ConfigureAwait(false);
            return await pipeline.Consume().ConfigureAwait(false);
        }
    }
}
