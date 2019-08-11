using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.Core.Pipelines
{
    [TestClass]
    public class TPipelineBuilder
    {
        public static readonly PipelineOperator<int, int> initial = context =>
        {
            context.Logs.Warning($"initial with {context.Origin}");
            return Task.FromResult(context.Origin);
        };
        public static readonly PipelineOperator<int, int> plus = context =>
        {
            int arg = context.Services.Get<int>();
            context.Logs.Information($"plus with {arg}");
            return Task.FromResult(context.Result + arg);
        };
        public static readonly PipelineOperator<int, int> multiply = context =>
        {
            int arg = context.Services.Get<int>();
            context.Logs.Information($"multiply with {arg}");
            return Task.FromResult(context.Result * arg);
        };
        public static readonly PipelineOperator<int, int> expNotImp = context =>
        {
            context.Logs.Error("exception!");
            throw new NotImplementedException();
        };

        public static PipelineBuilder<int, int> GetBasicBuilder(int arg)
        {
            PipelineBuilder<int, int> builder = new PipelineBuilder<int, int>().Configure("arg", service =>
            {
                service.Add(1);
                return Task.CompletedTask;
            }).Configure("arg-fix", service =>
            {
                service.Replace(arg);
            });
            return builder;
        }

        [TestMethod]
        public async Task Basic()
        {
            PipelineBuilder<int, int> builder = GetBasicBuilder(2).Use("", initial).Use("", plus).Use("", plus).Use("", multiply);
            Assert.IsNotNull(await builder.Build(0, new CodeRunner.Loggings.Logger("", CodeRunner.Loggings.LogLevel.Debug)));
        }
    }
}
