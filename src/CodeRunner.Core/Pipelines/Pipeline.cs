using CodeRunner.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public delegate Task<TResult> PipelineOperator<TOrigin, TResult>(OperationContext<TOrigin, TResult> context);

    public class Pipeline<TOrigin, TResult>
    {
        public Pipeline(TOrigin origin, Logger logger, ServiceProvider services, IReadOnlyList<(string, PipelineOperator<TOrigin, TResult>)> ops)
        {
            Origin = origin;
            Logger = logger;
            Services = services;
            Ops = ops;
            Logs = Logger.CreateScope("pipeline");
        }

        private TOrigin Origin { get; }

        private Logger Logger { get; }

        private ServiceProvider Services { get; }

        private IReadOnlyList<(string, PipelineOperator<TOrigin, TResult>)> Ops { get; }

        private Exception? Exception { get; set; }

#pragma warning disable CS8653 // 默认表达式为类型参数引入了 null 值。
        private TResult Result { get; set; } = default;
#pragma warning restore CS8653 // 默认表达式为类型参数引入了 null 值。

        public int Position { get; private set; } = 0;

        private LogScope Logs { get; set; }

        private bool HasEnd { get; set; } = false;

        public async Task<bool> Step()
        {
            if (Exception != null || Position >= Ops.Count || HasEnd)
            {
                return false;
            }

            var op = Ops[Position];

            Logs.Debug($"Executing {op.Item1} at {Position}.");

            var subLogScope = Logger.CreateScope(op.Item1);

            try
            {
                OperationContext<TOrigin, TResult> context = new OperationContext<TOrigin, TResult>(await Services.CreateScope(op.Item1), Origin, Result, subLogScope);
                var result = await op.Item2.Invoke(context);
                if (!context.IgnoreResult)
                    Result = result;
                if (context.IsEnd)
                {
                    HasEnd = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Exception = ex;
                subLogScope.Error(ex);
                return false;
            }

            Logs.Debug($"Executed {op.Item1} at {Position}.");

            Position++;
            return true;
        }

        public async Task<PipelineResult<TResult>> Consume()
        {
            while (await Step()) ;

            return new PipelineResult<TResult>(Result, Exception, Logger.GetAll());
        }
    }
}
