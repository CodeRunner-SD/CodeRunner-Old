using Microsoft;
using System.Diagnostics.CodeAnalysis;

namespace CodeRunner.Diagnostics
{
    public static class Assert
    {
        public static void IsTrue([DoesNotReturnIf(false)] bool condition)
        {
            if (!condition)
            {
                throw new AssertFailedException(nameof(IsTrue));
            }
        }

        public static void IsFalse([DoesNotReturnIf(true)] bool condition)
        {
            if (condition)
            {
                throw new AssertFailedException(nameof(IsFalse));
            }
        }

        public static void IsNotNull([ValidatedNotNull][NotNull] object? value)
        {
            if (value == null)
            {
                throw new AssertFailedException(nameof(IsNotNull));
            }
        }

        public static void IsNull([MaybeNull] object? value)
        {
            if (value != null)
            {
                throw new AssertFailedException(nameof(IsNull));
            }
        }

        [DoesNotReturn]
        public static void Fail() => throw new AssertFailedException(nameof(Fail));
    }
}
