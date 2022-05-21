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
    /// <summary>
    /// 坐标定义实体模型
    /// </summary>
    public partial class PositionEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public PositionEntity() { }

        /// <summary>
        /// 坐标标识数字唯一码,用于和PLC匹配对接
        /// </summary>
        public int PositionNum { get; set; }

        /// <summary>
        /// 坐标唯一编码，用于和AGV坐标对应
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// 地图编码
        /// </summary>
        public string MapDataCode { get; set; }

        /// <summary>
        /// 地图简称
        /// </summary>
        public string MapShortName { get; set; }

        /// <summary>
        /// 坐标名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 所属区域编号
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 是否被料架占用
        /// </summary>
        public int HasCon { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public string PodCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 交互的PLC区域代号
        /// </summary>
        public string PlcCode { get; set; }

        /// <summary>
        /// 码垛数
        /// </summary>
        public short Num { get; set; }

        /// <summary>
        /// 码垛总数
        /// </summary>
        public short TotalNum { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public short Status { get; set; }
    }
}
