using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeRunner.Pipelines
{
    public class ServiceScope
    {
        private Dictionary<Type, Dictionary<string, ServiceItem>> Pool { get; }

        public string Name { get; set; }

        internal ServiceScope(string name, Dictionary<Type, Dictionary<string, ServiceItem>> pool)
        {
            Pool = pool;
            Name = name;
        }

        private Dictionary<string, ServiceItem> OpenOrCreateSubDictionary<T>()
        {
            Type type = typeof(T);
            if (!Pool.TryGetValue(type, out Dictionary<string, ServiceItem>? list))
            {
                list = new Dictionary<string, ServiceItem>();
                Pool.Add(type, list);
            }
            return list;
        }

        private Dictionary<string, ServiceItem>? FindSubDictionary<T>()
        {
            if (Pool.TryGetValue(typeof(T), out Dictionary<string, ServiceItem>? list))
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public void Add<T>(T item, string id = "") where T : notnull
        {
            OpenOrCreateSubDictionary<T>().Add(id, new ServiceItem(item, Name));
        }

        public void Replace<T>(T item, string id = "") where T : notnull
        {
            Dictionary<string, ServiceItem> list = OpenOrCreateSubDictionary<T>();
            if (list.ContainsKey(id))
            {
                list[id] = new ServiceItem(item, Name);
            }
            else
            {
                list.Add(id, new ServiceItem(item, Name));
            }
        }

        public void Remove<T>(string id = "")
        {
            Dictionary<string, ServiceItem>? list = FindSubDictionary<T>();
            if (list != null)
            {
                list.Remove(id);
                if (list.Count == 0)
                {
                    Pool.Remove(typeof(T));
                }
            }
        }

        public T Get<T>(string id = "") where T : notnull
        {
            return (T)FindSubDictionary<T>()![id].Value;
        }

        public string GetSource<T>(string id = "") where T : notnull
        {
            return FindSubDictionary<T>()![id].Source;
        }

        public bool TryGet<T>([MaybeNull] out T value, string id = "") where T:notnull
        {
            try
            {
                value = Get<T>(id);
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
    }
}
