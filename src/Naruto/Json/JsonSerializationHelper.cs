using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Json
{
    /// <summary>
    /// json序列化帮助类，主要用于解决反序列化到指定类型的问题
    /// </summary>
    public static class JsonSerializationHelper
    {
        private const char TypeSeperator = '|';

        /// <summary>
        /// 序列化对象时也将对象的类型信息写入，因此可以使用 <see cref="DeserializeWithType"/> 反序列化
        /// </summary>
        public static string SerializeWithType(object obj)
        {
            return SerializeWithType(obj, obj.GetType());
        }

        /// <summary>
        /// 序列化对象时也将对象的类型信息写入，因此可以使用 <see cref="DeserializeWithType"/> 反序列化
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="type">待序列化的对象的类型</param>
        public static string SerializeWithType(object obj, Type type)
        {
            var serialized = obj.ToJsonString();

            return string.Format(
                "{0}{1}{2}",
                type.AssemblyQualifiedName,
                TypeSeperator,
                serialized
                );
        }

        /// <summary>
        /// 针对使用 <see cref="DeserializeWithType"/> 序列化的对象，进行反序列化到实际类型的操作
        /// </summary>

        public static T DeserializeWithType<T>(string serializedObj)
        {
            return (T)DeserializeWithType(serializedObj);
        }

        /// <summary>
        /// 针对使用 <see cref="DeserializeWithType"/> 序列化的对象，进行反序列化到实际类型的操作
        /// </summary>
        public static object DeserializeWithType(string serializedObj)
        {
            var typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
            var type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            var serialized = serializedObj.Substring(typeSeperatorIndex + 1);

            var options = new JsonSerializerSettings();
            //options.Converters.Insert(0, new IsoDateTimeConverter());

            return JsonConvert.DeserializeObject(serialized, type, options);
        }
    }
}
