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
    public enum EHikResponseCode
    {
        [EnumDescription("成功")]
        Success = 1,

        [EnumDescription("没有权限")]
        NoPermission = 2,

        [EnumDescription("异常")]
        Exception = 3,

        [EnumDescription("未登录")]
        NotLogin = 4
    }
}
