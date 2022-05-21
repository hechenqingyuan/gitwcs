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
    /// <summary>
    /// 服务通道,设定PLC的品牌信息
    /// </summary>
    public partial class ServerModel
    {
        public ServerModel() { }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// 应用程序ID
        /// </summary>
        public string ProgID { get; set; }

        /// <summary>
        /// 更新频率
        /// </summary>
        public int UpdateRate { get; set; }

        /// <summary>
        /// 设备组信息
        /// </summary>
        public List<DeviceModel> Devices { get; set; }
    }
}
