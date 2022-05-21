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
    /// 上层系统平台发送继续调度请求, RCS-2000 获取 AGV 下一个动作,继续执行(请求模型)
    /// </summary>
    public partial class ContinueTaskRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ContinueTaskRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一编 号， 同一个请求重复提交， 使用同 一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间截 格式: “yyyy-MM-dd HH:mm:ss”
        /// </summary>
        //public string reqTime { get; set; }

        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        //public string clientCode { get; set; }

        /// <summary>
        /// 令牌号, 由调度系统颁发。
        /// </summary>
        //public string tokenCode { get; set; }

        /// <summary>
        /// 工作位，与 RCS-2000 端配置的位 置名称一致。
        /// </summary>
        //public string wbCode { get; set; }

        /// <summary>
        /// 货架号，采用货架号触发的方式。
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// AGV 编号，采用 AGV 编号触发的 方式。
        /// </summary>
        //public string agvCode { get; set; }

        /// <summary>
        /// 任务单号 , 选填 ,  不 填 系 统 自 动 生 成，必须为 64 位 UUID
        /// </summary>
        //public string taskCode { get; set; }

        /// <summary>
        /// 下一个子任务的序列，指定第几个子 任务开始执行，校验子任务执行是否 正确。不填默认执行下一个子任务。
        /// </summary>
        //public string taskSeq { get; set; }

        /// <summary>
        /// 下一个位置信息，在任务类型中配置 外部设置时需要传入，否则不需要设 置。待现场地图部署、配置完成后可 获取
        /// </summary>
        public PositionDTO nextPositionCode { get; set; }
    }
}
