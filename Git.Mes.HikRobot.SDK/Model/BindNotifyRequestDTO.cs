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
    /// 可在执行绑定货架与储位,绑定货架与物料,绑定仓位与容器后通知上层
    /// </summary>
    public partial class BindNotifyRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BindNotifyRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一 编号， 同一个请求重复提交， 使 用同一编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请 求 时 间 戳 ， 格 式 :  “yyyy-MM-dd HH:mm:ss”
        /// </summary>
        public string reqTime { get; set; }

        /// <summary>
        /// 客户端编号， 如 PDA， HCWMS 等
        /// </summary>
        public string clientCode { get; set; }

        /// <summary>
        /// 令牌号,  由调度系统颁发
        /// </summary>
        public string tokenCode { get; set; }

        /// <summary>
        /// 方法名 bindPodAndBerth : 货架与储位绑 定解绑 bindPodAndMat : 货架与物料绑定 解绑 bindCtnrAndBin : 仓位与容器绑定 解绑
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// 绑定—1  解绑--0
        /// </summary>
        public string indBind { get; set; }

        public List<NotifyBindParamDTO> bindParam { get; set; }
    }
}
