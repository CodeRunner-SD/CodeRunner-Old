using CodeRunner.Diagnostics;
using System;
using System.Collections.Generic;

namespace CodeRunner.Templates
{
    public class Variable : IEquatable<Variable>
    {
        public Variable() : this("")
        {
        }

        public Variable(string name = "")
        {
            Assert.IsNotNull(name);

            Name = name;
            _ = Required();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public object? DefaultValue { get; set; }

        public T GetDefault<T>()
        {
            if (IsRequired || DefaultValue == null)
            {
                throw new NullReferenceException("No default value");
            }

            return (T)DefaultValue;
        }

        public Variable Required()
        {
            IsRequired = true;
            DefaultValue = null;
            return this;
        }

        public Variable NotRequired(object value)
        {
            Assert.IsNotNull(value);

            IsRequired = false;
            DefaultValue = value;
            return this;
        }

        public override bool Equals(object? obj) => Equals(obj as Variable);

        public bool Equals(Variable? other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode() => HashCode.Combine(Name);

        public static bool operator ==(Variable? left, Variable? right) => EqualityComparer<Variable>.Default.Equals(left, right);

        public static bool operator !=(Variable? left, Variable? right) => !(left == right);
    }
}
