using CodeRunner.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Test.Core.Pipelines
{

    [TestClass]
    public class TPipeline
    {
        [TestMethod]
        public void Basic()
        {
            PipelineBuilder<int, int> builder = TPipelineBuilder.GetBasicBuilder(2).Use("", TPipelineBuilder.initial).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.multiply);
            {
                Pipeline<int, int> pipeline = builder.Build(0, new CodeRunner.Loggings.Logger("", CodeRunner.Loggings.LogLevel.Debug)).Result;
                PipelineResult<int> res = pipeline.Consume().Result;
                Assert.IsTrue(res.IsOk);
                Assert.AreEqual(8, res.Result);
            }
        }

        [TestMethod]
        public void Exception()
        {
            PipelineBuilder<int, int> builder = TPipelineBuilder.GetBasicBuilder(2).Use("", TPipelineBuilder.initial).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.plus).Use("", TPipelineBuilder.expNotImp).Use("", TPipelineBuilder.multiply);
            {
                Pipeline<int, int> pipeline = builder.Build(0, new CodeRunner.Loggings.Logger("", CodeRunner.Loggings.LogLevel.Debug)).Result;
                PipelineResult<int> res = pipeline.Consume().Result;
                Assert.AreEqual(4, res.Result);
                Assert.IsTrue(res.IsError);
                Assert.IsInstanceOfType(res.Exception, typeof(NotImplementedException));
                Assert.AreEqual(CodeRunner.Loggings.LogLevel.Error, res.Logs.Last().Level);
            }
        }
    }
}
