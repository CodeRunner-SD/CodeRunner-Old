using System;
using System.Collections.Generic;

namespace CodeRunner.Templates
{
    public class ResolveContext
    {
        public ResolveContext(IDictionary<string, object>? variables = null)
        {
            Variables = variables ?? new Dictionary<string, object>();
        }

        private IDictionary<string, object> Variables { get; }

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
            Variables.Remove(name);
            return this;
        }

        public T GetVariable<T>(Variable variable)
        {
            if (Variables.TryGetValue(variable.Name, out object? val))
            {
                return (T)val!;
            }
            else
            {
                if (variable.IsRequired)
                {
                    throw new Exception($"No required variable with name {variable.Name}.");
                }
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                return (T)variable.Default;
#pragma warning restore CS8601 // 可能的 null 引用赋值。
            }
        }

        public bool HasVariable(string name)
        {
            return Variables.ContainsKey(name);
        }
    }
}
