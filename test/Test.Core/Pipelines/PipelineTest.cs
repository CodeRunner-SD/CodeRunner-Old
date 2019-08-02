using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Core
{
    [TestClass]
    public class PipelineTest
    {
        async Task<PipelineBuilder<int, int>> GetBasicBuilder(int arg)
        {
            var builder = new PipelineBuilder<int, int>();
            await builder.Configure("arg", service =>
            {
                service.Add(1);
                return Task.CompletedTask;
            });
            await builder.Configure("arg-fix", service =>
            {
                service.Replace(arg);
            });
            return builder;
        }

        PipelineOperator<int, int> initial = context =>
        {
            context.Logs.Info($"initial with {context.Origin}");
            return Task.FromResult(context.Origin);
        };

        PipelineOperator<int, int> plus = context =>
        {
            var arg = context.Services.Get<int>();
            context.Logs.Info($"plus with {arg}");
            return Task.FromResult(context.Result + arg);
        };

        PipelineOperator<int, int> multiply = context =>
        {
            var arg = context.Services.Get<int>();
            context.Logs.Info($"multiply with {arg}");
            return Task.FromResult(context.Result * arg);
        };

        PipelineOperator<int, int> expNotImp = context =>
        {
            var arg = context.Services.Get<int>();
            context.Logs.Info($"multiply with {arg}");
            throw new NotImplementedException();
        };

        [TestMethod]
        public void Basic()
        {
            var builder = GetBasicBuilder(2).Result;
            builder.Use(initial);
            builder.Use(plus); // + 2
            builder.Use(plus); // + 2
            builder.Use(multiply); // * 2
            {
                var pipeline = builder.Build(0, new CodeRunner.Loggers.Logger()).Result;
                var res = pipeline.Consume().Result;
                Assert.IsTrue(res.IsOk);
                Assert.AreEqual(8, res.Result);
            }
        }

        [TestMethod]
        public void Exception()
        {
            var builder = GetBasicBuilder(2).Result;
            builder.Use(initial);
            builder.Use(plus); // + 2
            builder.Use(plus); // + 2
            builder.Use(expNotImp);
            builder.Use(multiply); // * 2
            {
                var pipeline = builder.Build(0, new CodeRunner.Loggers.Logger()).Result;
                var res = pipeline.Consume().Result;
                Assert.AreEqual(4, res.Result);
                Assert.IsTrue(res.IsError);
                Assert.IsInstanceOfType(res.Exception, typeof(NotImplementedException));
                Assert.AreEqual(CodeRunner.Loggers.LogLevel.Error, res.Logs.Last().Level);
            }
        }
    }
}
