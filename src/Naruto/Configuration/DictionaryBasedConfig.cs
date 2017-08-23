using System;
using System.Collections.Generic;
using Naruto.Collections.Extensions;

namespace Naruto.Configuration
{
    public class DictionaryBasedConfig : IDictionaryBasedConfig
    {
        protected Dictionary<string, object> Configs { get; private set; }

        public object this[string name]
        {
            get { return Configs.GetOrDefault(name); }
            set { Configs[name] = value; }
        }

        protected DictionaryBasedConfig()
        {
            Configs = new Dictionary<string, object>();
        }

        public object Get(string name)
        {
            return Get(name, null);
        }

        public T Get<T>(string name)
        {
            var value = this[name];
            return value == null
                ? default(T)
                : (T)Convert.ChangeType(value, typeof(T));
        }

        public object Get(string name, object defaultValue)
        {
            var value = this[name];
            if (value == null)
            {
                return defaultValue;
            }

            return this[name];
        }

        public T Get<T>(string name, T defaultValue)
        {
            return (T)Get(name, (object)defaultValue);
        }

        public T GetOrCreate<T>(string name, Func<T> creator)
        {
            var value = Get(name);
            if (value == null)
            {
                value = creator();
                Set(name, value);
            }
            return (T)value;
        }

        public void Set<T>(string name, T value)
        {
            this[name] = value;
        }
    }
}
