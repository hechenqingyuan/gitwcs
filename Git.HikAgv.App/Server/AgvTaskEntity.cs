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
    /// AGV搬运任务模型
    /// </summary>
    public partial class AgvTaskEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public AgvTaskEntity() { }

        /// <summary>
        /// 请求的唯一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 当前所在的地图坐标
        /// </summary>
        public string currentPositionCode { get; set; }

        /// <summary>
        /// AGV搬运的货架编号
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 参数方法，用于各个模板回传的参数判断
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// AGV编号
        /// </summary>
        public string robotCode { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string taskCode { get; set; }

        /// <summary>
        /// 搬运使用的模板
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// 初始位置坐标名称
        /// </summary>
        public string StartPositionName { get; set; }

        /// <summary>
        /// 中途停靠坐标名称
        /// </summary>
        public string HalfwayPositionName { get; set; }

        /// <summary>
        /// 结束位置坐标名称
        /// </summary>
        public string EndPositionName { get; set; }


        /// <summary>
        /// 初始位置坐标编号
        /// </summary>
        public string StartPositionCode { get; set; }

        /// <summary>
        /// 中途停靠坐标编号
        /// </summary>
        public string HalfwayPositionCode { get; set; }

        /// <summary>
        /// 结束位置坐标编号
        /// </summary>
        public string EndPositionCode { get; set; }
    }
}
