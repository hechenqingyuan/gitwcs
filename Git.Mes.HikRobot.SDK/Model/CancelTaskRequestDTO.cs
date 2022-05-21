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
    /// 通过正在执行的任务编号,取消该任务,不再执行
    /// </summary>
    public partial class CancelTaskRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public CancelTaskRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一 编号，同一个请求重复提交，使用 同一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请  求  时  间  截	格  式  : “yyyy-MM-dd   HH:mm:ss”
        /// </summary>
        public string reqTime { get; set; }

        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string clientCode { get; set; }

        /// <summary>
        /// 令 牌 号 ,  由 调 度 系 统 颁 发 。 由 RCS-2000 告知上层系统
        /// </summary>
        public string tokenCode { get; set; }

        /// <summary>
        /// 取消类型 0 表示：取消后货架直接放地上1 表示：AGV 仍然背着货架， 根据 回库区域执行回库指令， 只有潜伏 车支持。默认的取消模式为 0
        /// </summary>
        public string forceCancel { get; set; }

        /// <summary>
        /// forcecancel=1 时有意义， 回库区域编号， 如果为空，采用货架配置的库区
        /// </summary>
        public string matterArea { get; set; }

        /// <summary>
        /// 取消该 AGV 正在执行的任务单
        /// </summary>
        public string agvCode { get; set; }

        /// <summary>
        /// 任务单编号, 取消该任务单
        /// </summary>
        public string taskCode { get; set; }
    }
}
