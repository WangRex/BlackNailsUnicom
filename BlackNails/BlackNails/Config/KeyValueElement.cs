using System.Configuration;

namespace BlackNails.Config
{
    /// <summary>
    /// 键值元素
    /// <remarks>
    /// 创建：2016.03.09
    /// </remarks>
    /// </summary>
    public class KeyValueElement:ConfigurationElement
    {
        /// <summary>
        /// 键
        /// </summary>
        [ConfigurationProperty("key")]
        public string Key {
            get { return this["key"].ToString(); }
            set { this["key"] = value; }
        }
        /// <summary>
        /// 值
        /// </summary>
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //[ConfigurationProperty("name")]
        //public string name
        //{
        //    get { return this["name"].ToString(); }
        //    set { this["name"] = value; }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[ConfigurationProperty("url")]
        //public string url
        //{
        //    get { return this["url"].ToString(); }
        //    set { this["url"] = value; }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[ConfigurationProperty("icon")]
        //public string icon
        //{
        //    get { return this["icon"].ToString(); }
        //    set { this["icon"] = value; }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[ConfigurationProperty("info")]
        //public string info
        //{
        //    get { return this["info"].ToString(); }
        //    set { this["info"] = value; }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[ConfigurationProperty("permission")]
        //public string permission
        //{
        //    get { return this["permission"].ToString(); }
        //    set { this["permission"] = value; }
        //}
    }
}
