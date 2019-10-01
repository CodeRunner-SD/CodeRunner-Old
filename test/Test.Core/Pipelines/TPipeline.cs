using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Core.Pipelines
{

    [TestClass]
    public class TPipeline
    {
        [TestMethod]
        public async Task Basic()
        {
            PipelineBuilder<int, Wrapper<int>> builder = TPipelineBuilder.GetBasicBuilder(2).Use("", TPipelineBuilder.initial).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.multiply);
            {
                Pipeline<int, Wrapper<int>> pipeline = await builder.Build(0, new CodeRunner.Loggings.Logger());
                PipelineResult<Wrapper<int>> res = await pipeline.Consume();
                Assert.IsTrue(res.IsOk);
                Assert.AreEqual<int>(8, res.Result!);
            }
        }

        [TestMethod]
        public async Task Exception()
        {
            PipelineBuilder<int, Wrapper<int>> builder = TPipelineBuilder.GetBasicBuilder(2).Use("", TPipelineBuilder.initial).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.expNotImp).Use("", TPipelineBuilder.multiply);
            {
                Pipeline<int, Wrapper<int>> pipeline = await builder.Build(0, new CodeRunner.Loggings.Logger());
                PipelineResult<Wrapper<int>> res = await pipeline.Consume();
                Assert.AreEqual<int>(4, res.Result!);
                Assert.IsTrue(res.IsError);
                Assert.IsInstanceOfType(res.Exception!.InnerException, typeof(NotImplementedException));
                Assert.AreEqual(CodeRunner.Loggings.LogLevel.Error, res.Logs.Last().Level);
            }
        }
    }
}
