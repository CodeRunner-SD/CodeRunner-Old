using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeRunner.Pipelines
{
    internal readonly struct ServiceItem : IEquatable<ServiceItem>
    {
        public ServiceItem(object value, string source)
        {
            Value = value;
            Source = source;
        }

        public object Value { get; }

        public string Source { get; }

        public override bool Equals(object? obj) => obj is ServiceItem item && Equals(item);

        public bool Equals([AllowNull] ServiceItem other)
        {
            return EqualityComparer<object>.Default.Equals(Value, other.Value) &&
                   Source == other.Source;
        }

        public override int GetHashCode() => HashCode.Combine(Value, Source);

        public static bool operator ==(ServiceItem left, ServiceItem right) => left.Equals(right);

        public static bool operator !=(ServiceItem left, ServiceItem right) => !(left == right);
    }
}
