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

namespace Git.HikAgv.App.OPCModel
{
    public partial class DeviceModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceModel() { }

        /// <summary>
        /// CPU 型号信息
        /// </summary>
        public string CpuName { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public List<GroupModel> ListGroups { get; set; }
    }
}
