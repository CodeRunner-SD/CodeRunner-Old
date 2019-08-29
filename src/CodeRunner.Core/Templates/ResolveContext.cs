using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeRunner.Templates
{
    public class ResolveContext
    {
        public ResolveContext(IDictionary<string, object>? variables = null) => Variables = variables ?? new Dictionary<string, object>();

        private IDictionary<string, object> Variables { get; }

        public ResolveContext WithVariable<T>(Variable variable, T value) where T : notnull => WithVariable(variable.Name, value);

        public ResolveContext WithVariable<T>(string name, T value) where T : notnull
        {
            if (Variables.ContainsKey(name))
            {
                Variables[name] = value;
            }
            else
            {
                Variables.Add(name, value);
            }

            return this;
        }

        public ResolveContext WithoutVariable(string name)
        {
            _ = Variables.Remove(name);
            return this;
        }

        public T GetVariable<T>(Variable variable)
        {
            if (Variables.TryGetValue(variable.Name, out object? val))
            {
                return (T)val;
            }
            else
            {
                if (variable.IsRequired)
                {
                    throw new Exception($"No required variable with name {variable.Name}.");
                }
                return variable.GetDefault<T>();
            }
        }

        public bool TryGetVariable<T>(Variable variable, [MaybeNull] out T value)
        {
            try
            {
                value = GetVariable<T>(variable);
                return true;
            }
            catch
            {
#pragma warning disable CS8653 // 默认表达式会为类型参数引入 null 值。
                value = default;
#pragma warning restore CS8653 // 默认表达式会为类型参数引入 null 值。
                return false;
            }
        }

        public bool HasVariable(string name) => Variables.ContainsKey(name);
    }
}
