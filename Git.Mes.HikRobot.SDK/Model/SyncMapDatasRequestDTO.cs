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
    /// 全量同步地码数据请求模型
    /// </summary>
    public partial class SyncMapDatasRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SyncMapDatasRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一编 号， 同一个请求重复提交， 使用同 一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间截  格式: “yyyy-MM-dd HH:mm:ss”
        /// </summary>
        public string reqTime { get; set; }

        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string clientCode { get; set; }

        /// <summary>
        /// 令牌号,由调度系统颁发
        /// </summary>
        public string tokenCode { get; set; }

        /// <summary>
        /// 地码编号，唯一标识, 代表只获取这 个位置
        /// </summary>
        public string mapDataCode { get; set; }

        /// <summary>
        /// 地图简称, 同步该地图的位置信息
        /// </summary>
        public string mapShortName { get; set; }

        /// <summary>
        /// 地图元素类型, 为空时, 代表同步该地图全部地码
        /// </summary>
        public string dataTyp { get; set; }
    }
}
