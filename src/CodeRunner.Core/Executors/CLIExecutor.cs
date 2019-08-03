using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Executors
{
    public class CLIExecutor
    {
        ProcessStartInfo StartInfo { get; set; }

        public string Input { get; set; } = "";

        public long? MemoryLimit { get; set; }

        public TimeSpan? TimeLimit { get; set; }

        public Process Process { get; private set; }

        ExecutorResult Result { get; set; }

        BackgroundWorker? bwMemory;

        public CLIExecutor(ProcessStartInfo startInfo)
        {
            StartInfo = startInfo;
            StartInfo.UseShellExecute = false;
            StartInfo.RedirectStandardError = true;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
            Result = new ExecutorResult();
            Process = new Process { StartInfo = StartInfo };
        }

        public async Task<ExecutorResult> Run()
        {
            Result = new ExecutorResult();
            Process.EnableRaisingEvents = true;
            Process.Exited += Process_Exited;
            {
                if (bwMemory != null) bwMemory.Dispose();
                bwMemory = new BackgroundWorker { WorkerSupportsCancellation = true };
                bwMemory.DoWork += BwMemory_DoWork;
            }

            Result.State = ExecutorState.Running;
            Result.StartTime = DateTimeOffset.Now;

            Process.Start();
            bwMemory.RunWorkerAsync();
            {
                await Process.StandardInput.WriteLineAsync(Input);
                Process.StandardInput.Close();
            }
            if (TimeLimit.HasValue)
            {
                if (Process.WaitForExit((int)Math.Ceiling(TimeLimit.Value.TotalMilliseconds)))
                {
                }
                else
                {
                    Result.State = ExecutorState.OutOfTime;
                    Kill();
                }
            }
            else
            {
                Process.WaitForExit();
            }

            while (bwMemory?.IsBusy == true) Thread.Sleep(5);
            while (Result.State == ExecutorState.Running) Thread.Sleep(5);

            return Result;
        }

        public void Kill()
        {
            Process.Kill();

            while (bwMemory?.IsBusy == true) Thread.Sleep(5);
            while (Result.State == ExecutorState.Running) Thread.Sleep(5);
        }

        private void BwMemory_DoWork(object sender, DoWorkEventArgs e)
        {
            Result.MaximumMemory = 0;
            while (Result.State == ExecutorState.Running && !e.Cancel)
            {
                try
                {
                    Result.MaximumMemory = Math.Max(Result.MaximumMemory, Math.Max(Process.PagedMemorySize64, Process.PeakPagedMemorySize64));
                    if (MemoryLimit.HasValue && Result.MaximumMemory > MemoryLimit)
                    {
                        Result.State = ExecutorState.OutOfMemory;
                        Kill();
                    }
                    Thread.Sleep(5);
                }
                catch { }
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            Result.ExitCode = Process.ExitCode;
            if (bwMemory?.IsBusy == true) bwMemory.CancelAsync();

            Result.EndTime = DateTimeOffset.Now;

            var output = new List<string>();
            while (!Process.StandardOutput.EndOfStream)
                output.Add(Process.StandardOutput.ReadLine()!);
            Result.Output = output.ToArray();

            var error = new List<string>();
            while (!Process.StandardError.EndOfStream)
                error.Add(Process.StandardError.ReadLine()!);
            Result.Error = error.ToArray();

            if (Result.State == ExecutorState.Running)
                Result.State = ExecutorState.Ended;
        }
    }
}
