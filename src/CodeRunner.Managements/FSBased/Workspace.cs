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
        public const string P_CRRoot = ".cr";
        public const string P_Settings = "settings.json";
        public const string P_TemplatesRoot = "templates";
        public const string P_OperatorsRoot = "operations";

        public Workspace(DirectoryInfo pathRoot) : base(pathRoot, new Lazy<DirectoryTemplate>(() => new WorkspaceTemplate()))
        {
            CRRoot = new DirectoryInfo(Path.Join(pathRoot.FullName, P_CRRoot));
            Templates = new TemplateManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_TemplatesRoot)));
            Operations = new OperationManager(new DirectoryInfo(Path.Join(CRRoot.FullName, P_OperatorsRoot)));
            SettingsLoader = new JsonFileLoader<WorkspaceSettings>(new FileInfo(Path.Join(CRRoot.FullName, P_Settings)));
        }

        private DirectoryInfo CRRoot { get; set; }

        public ITemplateManager Templates { get; }

        public IOperationManager Operations { get; }

        public override async Task Clear()
        {
            await Templates.Clear();
            await Operations.Clear();

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

        public async Task<IWorkItem?> Create(string name, BaseTemplate? template, Func<VariableCollection, ResolveContext, Task> resolveCallback)
        {
            ResolveContext context = new ResolveContext()
                .WithVariable(nameof(name), name)
                .WithVariable(DirectoryTemplate.Var, PathRoot.FullName);
            if (template == null)
            {
                template = new RegisterWorkItemTemplate();
                _ = context.WithVariable(RegisterWorkItemTemplate.Workspace, this);
            }
            await resolveCallback(template.GetVariables(), context).ConfigureAwait(false);
            IWorkItem? res;
            switch (template)
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
                    await template.DoResolve(context).ConfigureAwait(false);
                    res = null;
                    break;
            }
            return res;
        }

        public async Task<PipelineResult<bool>> Execute(IWorkItem? workItem, BaseOperation operation, Func<VariableCollection, ResolveContext, Task> resolveCallback, OperationWatcher watcher, ILogger logger)
        {
            ResolveContext context = new ResolveContext();
            WorkspaceSettings? settings = await Settings;
            if (settings != null)
            {
                _ = context.SetShell(settings.DefaultShell);
            }
            _ = context.SetWorkingDirectory(PathRoot.FullName);

            if (workItem != null && workItem is WorkItem item)
            {
                _ = context.SetInputPath(item.RelativePath);
            }
            await resolveCallback(operation.GetVariables(), context).ConfigureAwait(false);
            PipelineBuilder<OperationWatcher, bool> builder = await operation.Resolve(context).ConfigureAwait(false);
            Pipeline<OperationWatcher, bool> pipeline = await builder.Build(watcher, logger).ConfigureAwait(false);
            return await pipeline.Consume().ConfigureAwait(false);
        }
    }
}
