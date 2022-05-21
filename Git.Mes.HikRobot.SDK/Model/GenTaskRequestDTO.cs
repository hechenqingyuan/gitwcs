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
    /// 层系统平台发送调度请求, RCS 通过请求参数, 生成调度 AGV 任务单(请求模型)
    /// </summary>
    public partial class GenTaskRequestDTO
    {
        public GenTaskRequestDTO() { }

        /// <summary>
        /// 请求编号，每个请求都要一个唯一 编号， 同一个请求重复提交， 使 用同一编号。
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间截 格式: “yyyy-MM-dd HH:mm:ss”。
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
        /// 任务类型，与在 RCS-2000 端配置 的主任务类型编号一致。 内置任务类型
        /// </summary>
        public string taskTyp { get; set; }

        /// <summary>
        /// 容器类型（叉车专用）叉车项目必传
        /// </summary>
        public string ctnrTyp { get; set; }

        /// <summary>
        /// 容器编号（叉车专用）
        /// </summary>
        public string ctnrCode { get; set; }

        /// <summary>
        /// 工作位，一般为机台或工作台位置， 与 RCS-2000 端配置的位置名称一 致, 工作位名称为字母\数字\或组 合, 不超过 32 位
        /// </summary>
        public string wbCode { get; set; }

        /// <summary>
        /// 位置路径：AGV 关键路径位置集合，与任务类型中模板配置的位置路径一一对应。待现场地图部署、配置完成后可获取。
        /// </summary>
        public List<PositionDTO> positionCodePath { get; set; }

        /// <summary>
        /// 货架编号，不指定货架可以为空
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// “180”,”0”,”90”,”-90”	分 别 对 应 地 图 的 ” 左 ”,” 右 ”,” 上”,”下” ，不指定方向可以为空
        /// </summary>
        public string podDir { get; set; }

        /// <summary>
        /// 货架类型, 传空时表示随机找个货 架找空货架传参方式如下：-1: 代表不关心货架类型, 找到空 货架即可.-2: 代表从工作位获取关联货架类 型, 如果未配置, 只找空货架.货架类型编号: 只找该货架类型的 空货架.
        /// </summary>
        public string podTyp { get; set; }

        /// <summary>
        /// 物料批次或货架上的物料唯一编码, 生成任务单时,货架与物料直接绑定 时使用. （通过同时传 podCode 和 materialLot 来 绑 定 或 通 过 wbCode 找 到 位 置 上 的 货 架 和 materialLot 来绑定）
        /// </summary>
        public string materialLot { get; set; }

        /// <summary>
        /// 优先级，从（1~127）级，最大优 先级最高。为空时，采用任务模板的优先级
        /// </summary>
        public string priority { get; set; }

        /// <summary>
        /// 任务 单 号 , 选填 ,  不填系统自动生 成，UUID 小于等于 64 位
        /// </summary>
        public string taskCode { get; set; }

        /// <summary>
        /// AGV 编号，填写表示指定某一编号 的 AGV 执行该任务
        /// </summary>
        public string agvCode { get; set; }

        /// <summary>
        /// 自定义字段，不超过 2000 个字符
        /// </summary>
        public string data { get; set; }
    }
}
