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
    /// 消息反馈绑定参数对象
    /// </summary>
    public partial class NotifyBindParamDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public NotifyBindParamDTO() { }

        /// <summary>
        /// 货架编号	适用：bindPodAndBerth、bindPodAndMat
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 储位编号	适用：bindPodAndBerth
        /// </summary>
        public string berthCode { get; set; }

        /// <summary>
        /// 物料批次	适用：bindPodAndMat
        /// </summary>
        public string materialLot { get; set; }

        /// <summary>
        /// 仓位编号	适用：bindCtnrAndBin
        /// </summary>
        public string ctnrCode { get; set; }

        /// <summary>
        /// 仓位编号	适用：bindCtnrAndBin
        /// </summary>
        public string ctnrType { get; set; }

        /// <summary>
        /// 仓位编号	适用：bindCtnrAndBin
        /// </summary>
        public string stgBinCode { get; set; }
    }
}
