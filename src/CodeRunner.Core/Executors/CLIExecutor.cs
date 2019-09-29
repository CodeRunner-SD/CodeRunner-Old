using CodeRunner.Diagnostics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeRunner.Executors
{
    public sealed class CLIExecutor : IDisposable
    {
        private Process? Process { get; set; }

        private ExecutorResult? Result { get; set; }

        private CLIExecutorSettings Settings { get; set; }

        public CLIExecutor(CLIExecutorSettings settings) => Settings = settings;

        public void Initialize()
        {
            if (Result != null && Result.State == ExecutorState.Running)
            {
                throw new Exception("The process is running.");
            }

            Result = new ExecutorResult();
            Process = new Process { StartInfo = Settings.CreateStartInfo(), EnableRaisingEvents = true };
        }

        public async Task Kill()
        {
            if (Process == null || Process.HasExited)
            {
                return;
            }

            try
            {
                Process.Kill(true);
                await Task.Run(() => Process.WaitForExit()).ConfigureAwait(false);
            }
            catch (InvalidOperationException) { }
        }

        private async Task GetMemory()
        {
            Assert.IsNotNull(Result);

            Result.MaximumMemory = 0;
            while (Result.State == ExecutorState.Running && Process?.HasExited == false)
            {
                Process.Refresh();
                long mem = Math.Max(Process.PagedMemorySize64, Process.PeakPagedMemorySize64);
                mem = Math.Max(mem, Process.WorkingSet64);
                mem = Math.Max(mem, Process.PeakWorkingSet64);
                mem = Math.Max(mem, Process.PrivateMemorySize64);
                Result.MaximumMemory = Math.Max(Result.MaximumMemory, mem);
                if (Settings.MemoryLimit.HasValue && Result.MaximumMemory > Settings.MemoryLimit)
                {
                    Result.State = ExecutorState.OutOfMemory;
                    await Kill().ConfigureAwait(false);
                }
                await Task.Delay(5).ConfigureAwait(false);
            }
        }

        private async Task Running()
        {
            Assert.IsNotNull(Process);
            Assert.IsNotNull(Result);

            _ = Process.Start();

            if (Settings.CollectOutput)
            {
                Process.OutputDataReceived += (sender, e) => Result.AppendOutput(e.Data);
                Process.BeginOutputReadLine();
            }
            if (Settings.CollectError)
            {
                Process.ErrorDataReceived += (sender, e) => Result.AppendError(e.Data);
                Process.BeginErrorReadLine();
            }

            if (!string.IsNullOrEmpty(Settings.Input))
            {
                await Process.StandardInput.WriteAsync(Settings.Input).ConfigureAwait(false);
                Process.StandardInput.Close();
            }

            await Task.Run(async () =>
            {
                if (Settings.TimeLimit.HasValue)
                {
                    if (Process.WaitForExit((int)Math.Ceiling(Settings.TimeLimit.Value.TotalMilliseconds)))
                    {
                    }
                    else
                    {
                        Result.State = ExecutorState.OutOfTime;
                        await Kill().ConfigureAwait(false);
                    }
                }
                else
                {
                    Process.WaitForExit();
                }
            }).ConfigureAwait(false);
        }

        public async Task<ExecutorResult> Run()
        {
            if (Result == null)
            {
                Initialize();
            }

            Assert.IsNotNull(Result);

            if (Result.State != ExecutorState.Pending)
            {
                throw new Exception("Can't run before initialized.");
            }

            Result.State = ExecutorState.Running;
            Result.StartTime = DateTimeOffset.Now;

            await Task.WhenAll(Running(), GetMemory()).ConfigureAwait(false);

            Assert.IsNotNull(Process);

            Process.Refresh();

            Result.ExitCode = Process.ExitCode;
            Result.EndTime = DateTimeOffset.Now;

            if (Result.State == ExecutorState.Running)
            {
                Result.State = ExecutorState.Ended;
            }

            Process.Dispose();

            return Result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Process != null)
                    {
                        Process.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
        }
        #endregion
    }
}
