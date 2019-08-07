using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Core
{
    [TestClass]
    public class Pipelines
    {
        private PipelineBuilder<int, int> GetBasicBuilder(int arg)
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

        private readonly PipelineOperator<int, int> initial = context =>
        {
            context.Logs.Warning($"initial with {context.Origin}");
            return Task.FromResult(context.Origin);
        };
        private readonly PipelineOperator<int, int> plus = context =>
        {
            int arg = context.Services.Get<int>();
            context.Logs.Information($"plus with {arg}");
            return Task.FromResult(context.Result + arg);
        };
        private readonly PipelineOperator<int, int> multiply = context =>
        {
            int arg = context.Services.Get<int>();
            context.Logs.Information($"multiply with {arg}");
            return Task.FromResult(context.Result * arg);
        };
        private readonly PipelineOperator<int, int> expNotImp = context =>
        {
            context.Logs.Error("exception!");
            context.Logs.Fatal("exception!");
            throw new NotImplementedException();
        };

        [TestMethod]
        public void Basic()
        {
            PipelineBuilder<int, int> builder = GetBasicBuilder(2).Use("", initial).Use("", plus).Use("", plus).Use("", multiply);
            {
                Pipeline<int, int> pipeline = builder.Build(0, new CodeRunner.Loggings.Logger()).Result;
                PipelineResult<int> res = pipeline.Consume().Result;
                Assert.IsTrue(res.IsOk);
                Assert.AreEqual(8, res.Result);
            }
        }

        [TestMethod]
        public void Exception()
        {
            PipelineBuilder<int, int> builder = GetBasicBuilder(2).Use("", initial).Use("", plus).Use("", plus).Use("", expNotImp).Use("", multiply);
            {
                Pipeline<int, int> pipeline = builder.Build(0, new CodeRunner.Loggings.Logger()).Result;
                PipelineResult<int> res = pipeline.Consume().Result;
                Assert.AreEqual(4, res.Result);
                Assert.IsTrue(res.IsError);
                Assert.IsInstanceOfType(res.Exception, typeof(NotImplementedException));
                Assert.AreEqual(CodeRunner.Loggings.LogLevel.Error, res.Logs.Last().Level);
            }
        }
    }
}
