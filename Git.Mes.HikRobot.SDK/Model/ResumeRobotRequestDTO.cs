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
    /// 请求回复AGV数据模型
    /// </summary>
    public partial class ResumeRobotRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ResumeRobotRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一编号，同一个请求重复提交，使用 同一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间戳，格式: “yyyy-MM-dd HH:mm:ss”
        /// </summary>
        public string reqTime { get; set; }

        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string clientCode { get; set; }

        /// <summary>
        /// 令牌号,由调度系统颁发
        /// </summary>
        public string tokenCode { get; set; }

        /// <summary>
        /// 停止的机器人数，-1 表示所有机器 人
        /// </summary>
        public string robotCount { get; set; }

        /// <summary>
        /// AGV 所在地图的简称 注： robotCount  填 -1  的 话 ， mapShortName 必填
        /// </summary>
        public string mapShortName { get; set; }

        /// <summary>
        /// 具体机器人编号列表
        /// </summary>
        public List<string> robots { get; set; }
    }
}
