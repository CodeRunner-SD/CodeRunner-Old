using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.App.Mocks
{
    internal class TemplateManager : ItemManager<TemplateSettings, Package<BaseTemplate>>, ITemplateManager { }

    internal class OperationManager : ItemManager<OperationSettings, Package<BaseOperation>>, IOperationManager { }


    internal class TestWorkItem : IWorkItem
    {
        public Guid Id => Guid.NewGuid();

        public string Name => "";
    }

    internal class TestWorkspace : IWorkspace
    {
        public TestWorkspace(WorkspaceSettings? settings = null,
                             Func<Task>? onInitialize = null,
                             Func<Task>? onClear = null,
                             Func<string, BaseTemplate?, Func<VariableCollection, ResolveContext, Task>, Task<IWorkItem?>>? onCreate = null,
                             Func<IWorkItem?, BaseOperation, Func<VariableCollection, ResolveContext, Task>, OperationWatcher, ILogger, Task<PipelineResult<Wrapper<bool>>>>? onExecute = null)
        {
            Templates = new TemplateManager();
            Operations = new OperationManager();
            Settings = Task.FromResult(settings);
            Logger = new Logger();
            OnCreate = onCreate;
            OnClear = onClear;
            OnInitialize = onInitialize;
            OnExecute = onExecute;
            LogScope = Logger.CreateScope("tracer", LogLevel.Debug);
        }

        public ILogger Logger { get; }

        private LoggerScope LogScope { get; set; }

        public ITemplateManager Templates { get; }

        public IOperationManager Operations { get; }

        public Task<WorkspaceSettings?> Settings { get; }

        public Func<string, BaseTemplate?, Func<VariableCollection, ResolveContext, Task>, Task<IWorkItem?>>? OnCreate { get; }

        public Func<Task>? OnClear { get; }

        public Func<Task>? OnInitialize { get; }

        public Func<IWorkItem?, BaseOperation, Func<VariableCollection, ResolveContext, Task>, OperationWatcher, ILogger, Task<PipelineResult<Wrapper<bool>>>>? OnExecute { get; }

        public async Task Clear()
        {
            LogScope.Information("Invoke");
            if (OnClear != null)
                await OnClear();
        }

        public async Task<IWorkItem?> Create(string name, BaseTemplate? from, Func<VariableCollection, ResolveContext, Task> resolveCallback)
        {
            LogScope.Information("Invoke");
            return OnCreate != null ? await OnCreate(name, from, resolveCallback) : null;
        }

        public async Task<PipelineResult<Wrapper<bool>>> Execute(IWorkItem? workItem, BaseOperation from, Func<VariableCollection, ResolveContext, Task> resolveCallback, OperationWatcher watcher, ILogger logger)
        {
            LogScope.Information("Invoke");
            return OnExecute != null ? await OnExecute(workItem, from, resolveCallback, watcher, logger) : new PipelineResult<Wrapper<bool>>(false, null, Array.Empty<LogItem>());
        }

        public async Task Initialize()
        {
            LogScope.Information("Invoke");
            if (OnInitialize != null)
                await OnInitialize();
        }

        public void AssertInvoked(string name)
        {
            foreach (LogItem v in Logger.View())
            {
                if (v.Content.Contains("Invoke"))
                {
                    if (v.Content.Contains(name))
                        return;
                }
            }
            Assert.Fail($"{name} not invoked");
        }
    }
}
