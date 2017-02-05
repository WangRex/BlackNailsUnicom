

using System.Runtime.Remoting.Messaging;

namespace BlackNails.DAL
{
    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前线程的数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        public static DbContexts CurrentContext()
        {
            DbContexts _nContext = CallContext.GetData("BlackNailsContexts") as DbContexts;
            if (_nContext == null)
            {
                _nContext = new DbContexts();
                CallContext.SetData("BlackNailsContexts", _nContext);
            }
            return _nContext;
        }
    }
}
