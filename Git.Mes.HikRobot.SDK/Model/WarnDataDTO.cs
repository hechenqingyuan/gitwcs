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
    /// 警告信息数据模型
    /// </summary>
    public partial class WarnDataDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public WarnDataDTO() { }

        /// <summary>
        /// 车号
        /// </summary>
        public string robotCode { get; set; }

        /// <summary>
        /// 告警开始时间
        /// </summary>
        public string beginTime { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        public string warnContent { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string taskCode { get; set; }
    }
}
