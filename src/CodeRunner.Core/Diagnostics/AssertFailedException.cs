using System;

namespace CodeRunner.Diagnostics
{
    public class AssertFailedException : Exception
    {
        public AssertFailedException(string message) : base($"Assert failed: {message}.")
        {
        }

        public AssertFailedException(string message, Exception innerException) : base($"Assert failed: {message}.", innerException)
        {
        }

        public AssertFailedException() : base("Assert failed.")
        {
        }
    }
}
