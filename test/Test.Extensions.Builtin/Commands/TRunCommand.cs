using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Test.App.Mocks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TRunCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            TestWorkspace workspace = new TestWorkspace(onExecute: (item, op, call, watcher, logger) => Task.FromResult(new PipelineResult<Wrapper<bool>>(true, null, Array.Empty<LogItem>())));
            PipelineResult<Wrapper<int>> result = await Utils.UseSampleCommandInvoker(workspace,
                new RunCommand().Build(),
                new string[] { "hello", "--", "name=a" },
                before: async context =>
                {
                    _ = await Utils.InitializeWorkspace(context);
                    await context.Services.GetWorkspace().Operations.SetValue("hello", CodeRunner.Managements.FSBased.Templates.OperationsSpaceTemplate.Hello);
                    return 0;
                },
                after: context => Task.FromResult<Wrapper<int>>(0));

            workspace.AssertInvoked(nameof(IWorkspace.Execute));
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }
    }
}
