using System;
using System.Collections.Generic;

namespace CodeRunner.Templates
{
    public class Variable : IEquatable<Variable>
    {
        private string name;
        private bool isRequired = true;

        public Variable() : this("")
        {
        }

        public Variable(string name = "")
        {
            this.name = name;
            Required();
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool IsRequired
        {
            get => isRequired;
            set => isRequired = value;
        }

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
            IsRequired = false;
            DefaultValue = value;
            return this;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Variable);
        }

        public bool Equals(Variable? other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(Variable? left, Variable? right)
        {
            return EqualityComparer<Variable>.Default.Equals(left, right);
        }

        public static bool operator !=(Variable? left, Variable? right)
        {
            return !(left == right);
        }
    }
}
