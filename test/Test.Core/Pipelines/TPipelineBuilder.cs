using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.Core.Pipelines
{
    [TestClass]
    public class TPipelineBuilder
    {
        public static readonly PipelineOperation<int, Wrapper<int>> initial = context =>
        {
            context.Logs.Warning($"initial with {context.Origin}");
            return Task.FromResult<Wrapper<int>>(context.Origin);
        };
        public static readonly PipelineOperation<int, Wrapper<int>> plus = context =>
        {
            int arg = context.Services.Get<int>();
            context.Logs.Information($"plus with {arg}");
            return Task.FromResult<Wrapper<int>>(context.Result! + arg);
        };
        public static readonly PipelineOperation<int, Wrapper<int>> multiply = context =>
        {
            int arg = context.Services.Get<int>();
            context.Logs.Information($"multiply with {arg}");
            return Task.FromResult<Wrapper<int>>(context.Result! * arg);
        };
        public static readonly PipelineOperation<int, Wrapper<int>> expNotImp = context =>
        {
            context.Logs.Error("exception!");
            throw new NotImplementedException();
        };

        public static PipelineBuilder<int, Wrapper<int>> GetBasicBuilder(int arg)
        {
            PipelineBuilder<int, Wrapper<int>> builder = new PipelineBuilder<int, Wrapper<int>>().Configure("arg", service =>
            {
                service.Add(1);
                return Task.CompletedTask;
            }).Configure("arg-fix", service => service.Replace(arg));
            return builder;
        }

        [TestMethod]
        public async Task Basic()
        {
            PipelineBuilder<int, Wrapper<int>> builder = GetBasicBuilder(2).Use("", initial).Use("", plus).Use("", plus).Use("", multiply);
            Assert.IsNotNull(await builder.Build(0, new CodeRunner.Loggings.Logger()));
        }
    }
}
