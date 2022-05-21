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
    /// 机械手和AGV混合搬运交互点状态
    /// </summary>
    public partial class InteractiveEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public InteractiveEntity() { }

        /// <summary>
        /// 下工装： =1 AGV有严重报警，=0 AGV无报警
        /// </summary>
        public short DB_AgvToRobot_Palletizing_Warn_Serious { get; set; }

        /// <summary>
        /// 下工装： =1 AGV有报警，=0 AGV无报警
        /// </summary>
        public short DB_AgvToRobot_Palletizing_Warn_Tip { get; set; }

        /// <summary>
        /// 下工装：由AGV下发指令给机械手  1 推荐使用机器人左料架 =2 推荐机器人使用右料架
        /// </summary>
        public short DB_AgvToRobot_Palletizing_Notify { get; set; }

        /// <summary>
        /// 下工装： 1 AGV进入机器人左边光栅
        /// </summary>
        public short DB_AgvToRobot_Palletizing_AgvMove_LeftGrating { get; set; }

        /// <summary>
        /// 下工装： 1 AGV进入机器人右边光栅
        /// </summary>
        public short DB_AgvToRobot_Palletizing_AgvMove_RightGrating { get; set; }


        




        /// <summary>
        /// 下工装：机械手将地址位存储供AGV读取状态 =1 机器人正在使用左料架 =2 机器人正在使用右料架
        /// </summary>
        public short DB_RobotToAgv_Palletizing_Notify { get; set; }

        /// <summary>
        /// 下工装： 机器人左工位计数
        /// </summary>
        public short DB_RobotToAgv_Palletizing_Left_Num { get; set; }

        /// <summary>
        /// 下工装： 机器人左工位可摆放总数
        /// </summary>
        public short DB_RobotToAgv_Palletizing_Left_TotalNum { get; set; }

        /// <summary>
        /// 更新完毕时间
        /// </summary>
        public DateTime LeftUpdateTime { get; set; }

        /// <summary>
        /// 码垛唯一跟踪号
        /// </summary>
        public string LeftSnNum { get; set; }

        /// <summary>
        /// 下工装： 机器人右工位计数
        /// </summary>
        public short DB_RobotToAgv_Palletizing_Right_Num { get; set; }

        /// <summary>
        /// 下工装： 机器人右工位可摆放总数
        /// </summary>
        public short DB_RobotToAgv_Palletizing_Right_TotalNum { get; set; }

        /// <summary>
        /// 下工装： 机器人左光栅状态 =1已经开放，0=未开放
        /// </summary>
        public short DB_RobotToAgv_Palletizing_LeftGrating_Status { get; set; }

        /// <summary>
        /// 下工装： 机器人右光栅状态 =1已经开放，0=未开放
        /// </summary>
        public short DB_RobotToAgv_Palletizing_RightGrating_Status { get; set; }

        /// <summary>
        /// 更新完毕时间
        /// </summary>
        public DateTime RightUpdateTime { get; set; }

        /// <summary>
        /// 码垛唯一跟踪号
        /// </summary>
        public string RightSnNum { get; set; }

    }
}
