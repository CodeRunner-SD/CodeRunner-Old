using System;
using System.Collections.Generic;

namespace CodeRunner.Pipelines
{
    public class ServiceScope
    {
        Dictionary<Type, Dictionary<string, ServiceItem>> Pool { get; }

        public string Name { get; set; }

        internal ServiceScope(string name, Dictionary<Type, Dictionary<string, ServiceItem>> pool)
        {
            Pool = pool;
            Name = name;
        }

        Dictionary<string, ServiceItem> OpenOrCreateSubDictionary<T>()
        {
            var type = typeof(T);
            if (!Pool.TryGetValue(type, out var list))
            {
                list = new Dictionary<string, ServiceItem>();
                Pool.Add(type, list);
            }
            return list;
        }

        Dictionary<string, ServiceItem>? FindSubDictionary<T>()
        {
            if (Pool.TryGetValue(typeof(T), out var list))
                return list;
            else
                return null;
        }

        public void Add<T>(T item, string id = "") where T : notnull
        {
            OpenOrCreateSubDictionary<T>().Add(id, new ServiceItem(item, Name));
        }

        public void Replace<T>(T item, string id = "") where T : notnull
        {
            var list = OpenOrCreateSubDictionary<T>();
            if (list.ContainsKey(id))
            {
                list[id] = new ServiceItem(item, Name);
            }
            else list.Add(id, new ServiceItem(item, Name));
        }

        public void Remove<T>(string id = "")
        {
            var list = FindSubDictionary<T>();
            if (list != null)
            {
                list.Remove(id);
                if (list.Count == 0)
                    Pool.Remove(typeof(T));
            }
        }

        public T Get<T>(string id = "") where T : notnull
        {
            return (T)FindSubDictionary<T>()![id].Value;
        }

        public bool TryGet<T>(out T value, string id = "")
        {
            var list = FindSubDictionary<T>();
            if (list != null && list.TryGetValue(id, out var _value))
            {
                value = (T)_value.Value;
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
