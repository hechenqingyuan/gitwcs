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
    /// AGV状态查询结果响应模型
    /// </summary>
    public partial class QueryAgvStatusResultDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public QueryAgvStatusResultDTO() { }

        /// <summary>
        /// 机器人编号
        /// </summary>
        public string robotCode { get; set; }

        /// <summary>
        /// 机器人方向 (范围 -180~360 度)
        /// </summary>
        public string robotDir { get; set; }

        /// <summary>
        /// 机器人 IP
        /// </summary>
        public string robotIp { get; set; }

        /// <summary>
        /// 机器人电量, 范围: 0-100
        /// </summary>
        public string battery { get; set; }

        /// <summary>
        /// 机器人 x 坐标,单位:毫米
        /// </summary>
        public string posX { get; set; }

        /// <summary>
        /// 机器人 y 坐标,单位:毫米
        /// </summary>
        public string posY { get; set; }

        /// <summary>
        /// 机器人所在地图
        /// </summary>
        public string mapCode { get; set; }

        /// <summary>
        /// 机器人当前速度, 单位: mm/s
        /// </summary>
        public string speed { get; set; }

        /// <summary>
        /// 机器人状态 AGV 常见状态编号和描述 见附件 6.1
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 是否已被排除，被排除后不接受新 任务（ 1-排除， 0-正常）
        /// </summary>
        public string exclType { get; set; }

        /// <summary>
        /// 是否暂停 0-否 1-是
        /// </summary>
        public string stop { get; set; }

        /// <summary>
        /// 背货架的编号
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 背货架的方向
        /// </summary>
        public string podDir { get; set; }

        /// <summary>
        /// 执行路径,单位是毫米, 格式 x 轴,y 轴,方向示例:[“[x, y, dir]”,”[x, y, dir]”,”[x, y, dir]”]
        /// </summary>
        public string path { get; set; }
    }
}
