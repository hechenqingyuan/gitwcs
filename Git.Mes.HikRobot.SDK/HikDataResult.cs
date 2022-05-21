using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Mes.HikRobot.SDK
{
    public partial class HikDataResult
    {
        /// <summary>
        /// 相应状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 相应状态消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 业务相应消息
        /// </summary>
        //public string data { get; set; }

        /// <summary>
        /// 请求编码
        /// </summary>
        public string reqCode { get; set; }
    }

    public partial class HikDataResult<T> : HikDataResult
    {
        public T data { get; set; }
    }

}
