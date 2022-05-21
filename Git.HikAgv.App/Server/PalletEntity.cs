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

namespace Git.HikAgv.App.Server
{
    public partial class PalletEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public PalletEntity() { }

        /// <summary>
        /// 料架编号
        /// </summary>
        public string PodCode { get; set; }

        /// <summary>
        /// 码垛数量
        /// </summary>
        public short Num { get; set; }

        /// <summary>
        /// 总共码垛数量
        /// </summary>
        public short TotalNum { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// 坐标名称
        /// </summary>
        public string PositionName { get; set; }
    }
}
