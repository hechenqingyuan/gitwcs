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

using Git.Mes.HikRobot.SDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.HikAgv.App.Server
{
    /// <summary>
    /// 应用域上下文值
    /// </summary>
    public partial class DomainContext
    {
        public static InovanceModbusClient ModbusClient = null;

        /// <summary>
        /// AGV搬运任务的状态
        /// </summary>
        public static AgvTaskEntity AgvTask = null;

        /// <summary>
        /// 机械手和AGV交互实体对象(第一条生产线)
        /// </summary>
        public static InteractiveEntity Interactive_One { get; set; }

        /// <summary>
        /// 机械手和AGV交互实体对象(第二条生产线)
        /// </summary>
        public static InteractiveEntity Interactive_Two { get; set; }

        /// <summary>
        /// 地图坐标数据
        /// </summary>
        public static List<PositionEntity> ListMap { get; set; }

        /// <summary>
        /// 料架托盘数据
        /// </summary>
        public static List<PalletEntity> ListPallet { get; set; }

        /// <summary>
        /// 查询系统使用的AGV以及AGV的状态集合
        /// </summary>
        public static List<QueryAgvStatusResultDTO> ListAgv { get; set; }
    }
}
