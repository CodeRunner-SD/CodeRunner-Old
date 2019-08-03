using System.Collections.Generic;

namespace CodeRunner.Templates
{
    public class TemplateResolveContext
    {
        public TemplateResolveContext(IDictionary<string, object>? variables = null)
        {
            Variables = variables ?? new Dictionary<string, object>();
        }

        IDictionary<string, object> Variables { get; }

        public TemplateResolveContext With<T>(string name, T value) where T : notnull
        {
            Variables.Add(name, value);
            return this;
        }

        public TemplateResolveContext Without(string name)
        {
            Variables.Remove(name);
            return this;
        }

        public T GetVariable<T>(string name)
        {
            return (T)Variables[name];
        }

        public bool TryGetVariable<T>(string name, out T value)
        {
            if (Variables.TryGetValue(name, out var val))
            {
                value = (T)val;
                return true;
            }
            else
            {
#pragma warning disable CS8653 // 默认表达式为类型参数引入了 null 值。
                value = default;
#pragma warning restore CS8653 // 默认表达式为类型参数引入了 null 值。
                return false;
            }
        }
    }
}
