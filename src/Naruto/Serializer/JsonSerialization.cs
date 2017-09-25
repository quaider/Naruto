using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Naruto.Serializer
{
    /// <summary>
    /// json序列化帮助类
    /// </summary>
    public static class JsonSerialization
    {
        #region 序列化存储类型及反序列化输出制定类型，移植性较差

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
            return (T)DeserializeWithType(serializedObj, typeof(T));
        }

        /// <summary>
        /// 针对使用 <see cref="DeserializeWithType"/> 序列化的对象，进行反序列化到实际类型的操作
        /// </summary>
        public static object DeserializeWithType(string serializedObj, Type typeSpecified = null)
        {
            var typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);

            // 没有存储类型信息
            if (typeSeperatorIndex < 0)
            {
                return typeSpecified == null
                    ? JsonConvert.DeserializeObject(serializedObj)
                    : JsonConvert.DeserializeObject(serializedObj, typeSpecified);
            }

            var type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            var serialized = serializedObj.Substring(typeSeperatorIndex + 1);

            var options = new JsonSerializerSettings();
            //options.Converters.Insert(0, new IsoDateTimeConverter());

            return JsonConvert.DeserializeObject(serialized, type, options);
        }

        #endregion

        /// <summary>
        /// json.net序列化
        /// </summary>
        public static string Serialize(object source)
        {
            return Serialize(source, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        /// <summary>
        /// json.net序列化
        /// </summary>
        public static string Serialize(object source, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(source, settings);
        }

        /// <summary>
        /// json.net序列化
        /// </summary>
        public static string Serialize(object source, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(source, converters);
        }

        /// <summary>
        /// json.net序列化(json key首字母小写)
        /// </summary>
        public static string CanmelCaseSerialize(object source)
        {
            return JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });
        }

        /// <summary>
        /// 带时间格式的对象序列化
        /// </summary>
        /// <param name="source">要序列化的对象</param>
        /// <param name="format">时间格式(默认yyyy-MM-dd HH:mm:ss)</param>
        public static string DateTimeSerialize(object source, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = format
            });
        }

        /// <summary>
        /// 获取对象的byte[]形式
        /// </summary>
        /// <param name="message">消息对象</param>
        public static byte[] SerializeToBytes(object message)
        {
            var msgStr = Serialize(message);
            return Encoding.UTF8.GetBytes(msgStr);
        }

        /// <summary>
        /// json字符串反序列化成指定类型的对象
        /// </summary>
        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// json字符串反序列化成指定类型的对象
        /// </summary>
        public static T Deserialize<T>(string value, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// json字符串反序列化成指定类型的对象
        /// </summary>
        public static T Deserialize<T>(string value, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(value, converters);
        }

        /// <summary>
        /// json字符串反序列化成指定类型的对象
        /// </summary>
        public static object Deserialize(string value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject(value, settings);
        }

        /// <summary>
        /// byte[]反序列化成.net object
        /// </summary>
        public static object DeserializeBytes(byte[] bytes)
        {
            return DeserializeBytes<object>(bytes);
        }

        /// <summary>
        /// 将byte[]反序列化成指定类型的对象
        /// </summary>
        public static T DeserializeBytes<T>(byte[] bytes)
        {
            var serializer = new JsonSerializer();
            using (var stream = new MemoryStream(bytes))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
