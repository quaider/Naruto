using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Naruto.Serializer
{
    /// <summary>
    /// Xml序列化和反序列化(System.Xml.Serialization.XmlSerializer实现)
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// 序列化成XML字符串
        /// </summary>
        public static string Serialize<T>(T value)
        {
            return Serialize(value, typeof(T));
        }

        /// <summary>
        /// 序列化成XML字符串
        /// </summary>
        public static string Serialize(object value, Type type)
        {
            var serializer = new XmlSerializer(type);
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                return stream.ToString();
            }
        }

        /// <summary>
        /// 序列化成byte[]
        /// </summary>
        public static byte[] SerializerToBytes<T>(object value)
        {
            return SerializerToBytes(value, typeof(T));
        }

        /// <summary>
        /// 序列化成byte[]
        /// </summary>
        public static byte[] SerializerToBytes(object value, Type type)
        {
            var serializer = new XmlSerializer(type);
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// xml字符串反序列化成.net object
        /// </summary>
        public static object Deserialize(string value, Type type)
        {
            var serializer = new XmlSerializer(type);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// xml形式的字符串反序列化指定类型的对象
        /// </summary>
        public static T Deserialize<T>(string value) where T : class
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Deserialize<T>(value);
        }

        /// <summary>
        /// xml形式的byte[]反序列化成指定类型的对象
        /// </summary>
        public static T Deserialize<T>(byte[] value) where T : class
        {
            return DeserializeFromBytes(value, typeof(T)) as T;
        }

        /// <summary>
        /// xml形式的byte[]反序列化成.net object
        /// </summary>
        public static object DeserializeFromBytes(byte[] value, Type type)
        {
            var serializer = new XmlSerializer(type);
            using (var stream = new MemoryStream(value))
            {
                return serializer.Deserialize(stream);
            }
        }
    }
}
