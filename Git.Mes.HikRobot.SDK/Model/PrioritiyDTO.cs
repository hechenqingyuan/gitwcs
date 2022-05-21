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
    /// 优先级实体模型
    /// </summary>
    public partial class PrioritiyDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public PrioritiyDTO() { }

        /// <summary>
        /// 必填，正在执行的任务单编号
        /// </summary>
        public string taskCode { get; set; }

        /// <summary>
        /// 必填，优先级，从（1~127）级， 最大优先级最高
        /// </summary>
        public string priority { get; set; }

    }
}
