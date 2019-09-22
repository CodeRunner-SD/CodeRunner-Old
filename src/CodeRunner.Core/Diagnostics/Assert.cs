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

        public static void IsNotNull([NotNull] object? value) => IsFalse(value == null);

        public static void IsNull([MaybeNull] object? value) => IsTrue(value == null);

        [DoesNotReturn]
        public static void Fail() => throw new AssertFailedException(nameof(Fail));
    }
}
