using CodeRunner.Loggers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public delegate Task<TResult> PipelineOperator<TOrigin, TResult>(ExecutionContext<TOrigin, TResult> context);

    public class Pipeline<TOrigin, TResult>
    {
        public Pipeline(TOrigin origin, Logger logger, ServiceProvider services, IReadOnlyList<PipelineOperator<TOrigin, TResult>> ops)
        {
            Origin = origin;
            Logger = logger;
            Services = services;
            Ops = ops;
        }

        private TOrigin Origin { get; }

        private Logger Logger { get; }

        private ServiceProvider Services { get; }

        private IReadOnlyList<PipelineOperator<TOrigin, TResult>> Ops { get; }

        private Exception? Exception { get; set; }

#pragma warning disable CS8653 // 默认表达式为类型参数引入了 null 值。
        private TResult Result { get; set; } = default;
#pragma warning restore CS8653 // 默认表达式为类型参数引入了 null 值。

        public int Position { get; private set; } = 0;

        public async Task<bool> Step()
        {
            if (Exception != null || Position >= Ops.Count)
            {
                return false;
            }

            LogScope logs = Logger.CreateScope(Position.ToString());
            try
            {
                ExecutionContext<TOrigin, TResult> context = new ExecutionContext<TOrigin, TResult>(await Services.CreateScope(Position.ToString()), Origin, Result, logs);
                Result = await Ops[Position].Invoke(context);
            }
            catch (Exception ex)
            {
                Exception = ex;
                logs.Error(ex);
                return false;
            }

            Position++;
            return true;
        }

        public async Task<PipelineResult<TResult>> Consume()
        {
            while (await Step())
            {
                ;
            }

            return new PipelineResult<TResult>(Result, Exception, Logger.Contents.ToArray());
        }
    }
}
