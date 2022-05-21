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
    /// 通过任务编号查询任务当前执行状态，支持批量查询（请求数据模型）
    /// </summary>
    public partial class QueryTaskStatusReqeustDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public QueryTaskStatusReqeustDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一 编号，同一个请求重复提交，使用 同一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间戳，格式: “yyyy-MM-dd HH:mm:ss”
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
        /// 任务单编号数组 任务单编号数组与 AGV 编号至少传 其中之一
        /// </summary>
        public List<string> taskCodes { get; set; }

        /// <summary>
        /// AGV 编号任务编号数组与 AGV 编号至少传其 中之一
        /// </summary>
        public string agvCode { get; set; }
    }
}
