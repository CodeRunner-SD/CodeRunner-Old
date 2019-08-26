using System;

namespace CodeRunner.Loggings
{
    public class LoggerScope
    {
        public LoggerScope(ILogger source, string name, LogLevel level)
        {
            Source = source;
            Name = name;
            Level = level;
        }

        private ILogger Source { get; }

        public string Name { get; }

        public LogLevel Level { get; set; }

        public void Log(string content, LogLevel level,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (level >= Level)
            {
                Source.Log(new LogItem
                {
                    Level = level,
                    Content = content,
                    Scope = Name,
                    Time = DateTimeOffset.Now
                }, memberName, sourceFilePath, sourceLineNumber);
            }
        }

        public void Error(Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(exception.ToString(), memberName, sourceFilePath, sourceLineNumber);
        }

        public void Fatal(Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(exception.ToString(), memberName, sourceFilePath, sourceLineNumber);
        }

        public void Error(string content,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(content, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Warning(string content,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(content, LogLevel.Warning, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Information(string content,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(content, LogLevel.Information, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Fatal(string content,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(content, LogLevel.Fatal, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Debug(string content,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(content, LogLevel.Debug, memberName, sourceFilePath, sourceLineNumber);
        }

        public LoggerScope CreateScope(string name, LogLevel level)
        {
            return new LoggerScope(Source, $"{Name}/{name}", level);
        }
    }
}
