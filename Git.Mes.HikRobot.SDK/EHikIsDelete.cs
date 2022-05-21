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

using Git.Framework.DataTypes.EnumAtttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Mes.HikRobot.SDK
{
    public enum EHikIsDelete
    {
        /// <summary>
        /// 未删除
        /// </summary>
        [EnumDescription("未删除")]
        NotDelete = 0,

        /// <summary>
        /// 已删除
        /// </summary>
        [EnumDescription("已删除")]
        Deleted = 1
    }
}
