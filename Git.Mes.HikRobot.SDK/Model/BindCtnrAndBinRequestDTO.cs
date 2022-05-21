/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 吉特日化MES,吉特WMS
 * Create Date: 2022/5/16 12:46:17
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright:  贺臣 15800466429
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 *2022/5/16 12:46:17      贺臣
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Mes.HikRobot.SDK.Model
{
    /// <summary>
    /// 仓位与容器的关系绑定 请求数据模型
    /// </summary>
    public partial class BindCtnrAndBinRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BindCtnrAndBinRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一 编号，同一个请求重复提交，使用 同一编号。由上层系统提供
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间戳，格式 :  “yyyy-MM-dd HH:mm:ss”。由上层系统提供
        /// </summary>
        public string reqTime { get; set; }

        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等。 由 RCS-2000 告知上层系统
        /// </summary>
        public string clientCode { get; set; }

        /// <summary>
        /// 令牌号 ,  由 调 度 系 统 颁 发 。 由RCS-2000 告知上层系统
        /// </summary>
        public string tokenCode { get; set; }

        /// <summary>
        /// bindPodAndBerth
        /// </summary>
        public string interfaceName { get; set; }

        /// <summary>
        /// 容器编号
        /// </summary>
        public string ctnrCode { get; set; }

        /// <summary>
        /// 容器类型
        /// </summary>
        public string ctnrTyp { get; set; }

        /// <summary>
        /// 仓位编号，与仓位绑定解绑必填
        /// </summary>
        public string stgBinCode { get; set; }

        /// <summary>
        /// 地图数据编号，用于虚拟货架仓位 的绑定解绑
        /// </summary>
        public string positionCode { get; set; }

        /// <summary>
        /// "1"：绑定， "0"：解绑
        /// </summary>
        public string indBind { get; set; }
    }
}
