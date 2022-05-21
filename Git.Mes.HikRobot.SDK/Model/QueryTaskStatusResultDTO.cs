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
    /// 查询任务状态响应结果
    /// </summary>
    public partial class QueryTaskStatusResultDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public QueryTaskStatusResultDTO() { }

        /// <summary>
        /// 任务单编号
        /// </summary>
        public string taskCode { get; set; }

        /// <summary>
        /// 任务状态： 1-已创建，2-正在执行， 5-取消完成，9-已结束
        /// </summary>
        public string taskStatus { get; set; }

        /// <summary>
        /// AGV 编号, 任务分配车后有值
        /// </summary>
        public string agvCode { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public string taskTyp { get; set; }
    }
}
