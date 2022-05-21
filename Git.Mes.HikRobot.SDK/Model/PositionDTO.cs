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
    /// 位置实体模型
    /// </summary>
    public partial class PositionDTO
    {
        public PositionDTO() { }

        /// <summary>
        /// 位置编号,单个编号不超过64位
        /// </summary>
        public string positionCode { get; set; }

        /// <summary>
        /// 位置类型说明： 00 表示位置编号 01 表示物料批次号
        /// </summary>
        public string type { get; set; }
    }
}
