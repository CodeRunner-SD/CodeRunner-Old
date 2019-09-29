using CodeRunner.Diagnostics;

namespace CodeRunner.Pipelines
{
    public class Wrapper<T> where T : struct
    {
        public Wrapper(T value) => Value = value;

        private T Value { get; set; }

        public static implicit operator Wrapper<T>(T value) => new Wrapper<T>(value);

        public static implicit operator T(Wrapper<T> value)
        {
            Assert.IsNotNull(value);
            return value.Value;
        }

        public Wrapper<T> ToWrapper(in T value)
        {
            Value = value;
            return this;
        }

        public T FromWrapper() => Value;
    }
}
