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
    /// 地图数据模型
    /// </summary>
    public partial class SyncMapDatasResultDTO
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SyncMapDatasResultDTO() { }

        /// <summary>
        /// 储位类型，1-外层储位，2-内层储位， 3-普通储位; 位置是储位类型时必填
        /// </summary>
        public string berthType { get; set; }

        /// <summary>
        /// 地码 X 坐标(mm)
        /// </summary>
        public string cooX { get; set; }

        /// <summary>
        /// 地码 Y 坐标(mm)
        /// </summary>
        public string cooY { get; set; }

        /// <summary>
        /// 地图元素类型,常用类型: 11-充电桩， 10-工作台，1-储位，20-缓冲区, 55: 巷道存储区
        /// </summary>
        public string dataTyp { get; set; }

        /// <summary>
        /// 工	作	台	方	向 “180”,”0”,”90”,”-90”    分别 代表”左”,”右”,”上”,”下” 工作台方向为工作人员面向货架拣货 的方向
        /// </summary>
        public string direction { get; set; }

        /// <summary>
        /// 地图编号
        /// </summary>
        public string mapCode { get; set; }

        /// <summary>
        /// 地码编号，唯一标识 
        /// </summary>
        public string mapDataCode { get; set; }

        /// <summary>
        /// 位置编号，地图位置的别名，能任意 命名 ( 字母 + 数字 ) ， 但 要 唯 一 ， 由 RCS-2000 界面配置
        /// </summary>
        public string positionCode { get; set; }
    }
}
