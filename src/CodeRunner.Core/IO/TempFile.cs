using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeRunner.IO
{
    public class TempFile : IDisposable
    {
        public TempFile()
        {
            File = new FileInfo(Path.GetTempFileName());
        }

        public FileInfo File { get; }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    File.Delete();
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
