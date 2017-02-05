using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BlackNails.CommonClass
{
    public enum SerializationType
    {
        Xml,
        Json,
        DataContract,
        Binary
    }

    [System.Serializable]
    public class SerializeHelper
    {
        /// <summary>
        /// 将C#数据实体转化为xml数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>xml格式字符串</returns>
        public static string XmlSerialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            StringWriter sWriter = new StringWriter();
            ser.Serialize(sWriter, obj);
            return sWriter.ToString();
        }

        /// <summary>
        /// 将xml数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合xml格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static T XmlDeserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml.ToCharArray()));
            T obj = (T)serializer.Deserialize(ms);
            ms.Close();

            return obj;
        }

        /// <summary>
        /// 将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public static string JsonSerialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static T JsonDeserialize<T>(string json)
        {
            T obj = (T)JsonConvert.DeserializeObject(json);
            return obj;
        }


        public static string JsonSerializeList<T>(List<T> obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static List<T> JsonListDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        #region ========== BinaryBytes ==========
        /// <summary>
        /// 将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="obj">待保存的对象</param>
        /// <returns>byte数组</returns>
        public static byte[] SaveToBinaryBytes(object obj)
        {
            //将对象序列化到MemoryStream中
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            //从MemoryStream中获取获取byte数组
            return ms.ToArray();
        }

        /// <summary>
        /// 将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>对象</returns>
        public static object LoadFromBinaryBytes(byte[] bytes)
        {
            object result = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (bytes != null)
            {
                MemoryStream ms = new MemoryStream(bytes);
                result = formatter.Deserialize(ms);
            }
            return result;
        }
        #endregion

        #region ========= other ==========
        /// <summary>
        /// 使用BinaryFormatter将对象系列化到MemoryStream中。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>保存对象的MemoryStream</returns>
        public static MemoryStream SaveToMemoryStream(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BufferedStream stream = new BufferedStream(ms);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms;
        }

        public static object XmlDeserialize(Type type, string xmlStr)
        {
            if (xmlStr == null || xmlStr.Trim() == "")
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(type);
            StringReader sWriter = new StringReader(xmlStr);
            return ser.Deserialize(sWriter);
        }

        #endregion
    }
}
