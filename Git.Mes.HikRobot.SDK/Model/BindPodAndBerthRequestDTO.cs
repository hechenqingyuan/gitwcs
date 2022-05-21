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
    /// 货架与储位的关系绑定请求数据模型
    /// </summary>
    public partial class BindPodAndBerthRequestDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BindPodAndBerthRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一 编号，同一个请求重复提交，使用 同一编号
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
        /// 令牌号, 由调度系统颁发
        /// </summary>
        public string tokenCode { get; set; }

        /// <summary>
        /// 货架编号
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 位置编号，地图位置的别名，能任 意命名(字母+数字)，但要唯一，由 RCS-2000 界面配置
        /// </summary>
        public string positionCode { get; set; }

        /// <summary>
        /// 货架方向 “0”：横向，“1”：纵向 横向：货架长与 X 轴一致； 纵向：货架宽与 X 轴一致。 不传默认为横向
        /// </summary>
        public string podDir { get; set; }

        /// <summary>
        /// "1"：绑定， "0"：解绑
        /// </summary>
        public string indBind { get; set; }
    }
}
