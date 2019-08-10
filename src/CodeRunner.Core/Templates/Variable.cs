using System;
using System.Collections.Generic;

namespace CodeRunner.Templates
{
    public class Variable : IEquatable<Variable>
    {
        private string name;
        private bool isRequired = true;
        private bool isReadOnly = false;
        private object? @default = null;

        public Variable() : this("")
        {
        }

        public Variable(string name = "")
        {
            this.name = name;
        }

        public string Name
        {
            get => name;
            set
            {
                if (IsReadOnly)
                {
                    throw new Exception("Can not modify a readonly variable.");
                }

                name = value;
            }
        }

        public bool IsRequired
        {
            get => isRequired;
            set
            {
                if (IsReadOnly)
                {
                    throw new Exception("Can not modify a readonly variable.");
                }

                isRequired = value;
            }
        }

        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                if (IsReadOnly)
                {
                    throw new Exception("Can not modify a readonly variable.");
                }

                isReadOnly = value;
            }
        }

        public object? Default
        {
            get => @default;
            set
            {
                if (IsReadOnly)
                {
                    throw new Exception("Can not modify a readonly variable.");
                }

                @default = value;
            }
        }

        public Variable ReadOnly()
        {
            IsReadOnly = true;
            return this;
        }

        public Variable Required()
        {
            if (IsReadOnly)
            {
                throw new Exception("Can not modify a readonly variable.");
            }

            IsRequired = true;
            return this;
        }

        public Variable NotRequired(object? value)
        {
            if (IsReadOnly)
            {
                throw new Exception("Can not modify a readonly variable.");
            }

            IsRequired = false;
            Default = value;
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
