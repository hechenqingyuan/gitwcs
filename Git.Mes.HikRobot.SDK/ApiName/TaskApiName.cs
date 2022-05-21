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

namespace Git.Mes.HikRobot.SDK.ApiName
{
    public class TaskApiName
    {
        /// <summary>
        /// 上层系统平台发送调度请求, RCS通过请求参数, 生成调度 AGV 任务单。
        /// </summary>
        public static string TaskApiName_genAgvSchedulingTask = "/rcms/services/rest/hikRpcService/genAgvSchedulingTask";

        /// <summary>
        /// 上层系统平台发送继续调度请求, RCS-2000 获取 AGV 下一个动作,继续执行
        /// </summary>
        public static string TaskApiName_continueTask = "/rcms/services/rest/hikRpcService/continueTask";

        /// <summary>
        /// 通过正在执行的任务编号,取消该任务,不再执行
        /// </summary>
        public static string TaskApiName_CancelTask = "/rcms/services/rest/hikRpcService/cancelTask";

        /// <summary>
        /// 通过请求参数, 查询 AGV 状态信息，包括电池电量
        /// </summary>
        public static string TaskApiName_QueryAgvStatus = "/rcms-dps/rest/queryAgvStatus";

        /// <summary>
        /// 通过任务编号查询任务当前执行状态，支持批量查询
        /// </summary>
        public static string TaskApiName_QueryTaskStatus = "/rcms-dps/rest/QueryTaskStatus";

        /// <summary>
        /// 停止指定 AGV 或全部 AGV
        /// </summary>
        public static string TaskApiName_StopRobot = "/rcms/services/rest/hikRpcService/stopRobot";

        /// <summary>
        /// 恢复 AGV, 恢复后继续执行未完成的任务
        /// </summary>

        public static string TaskApiName_ResumeRobot = "/rcms/services/rest/hikRpcService/resumeRobot";

        /// <summary>
        /// 设置任务优先级(1~127 级),值越大,优先级越高.优先级生效仅在系统中 AGV 数量不足，存在多个优先级不同的任务时候，会按照优先级的先后 顺序分配 AGV 执行。
        /// </summary>
        public static string TaskApiName_SetTaskPriority = "/rcms/services/rest/hikRpcService/setTaskPriority";

        /// <summary>
        /// 货架与储位的关系绑定, 系统可以通过货架找到对应位置
        /// </summary>
        public static string TaskApiName_BindPodAndBerth = "/rcms/services/rest/hikRpcService/bindPodAndBerth";

        /// <summary>
        /// 货架与物料批次的关系绑定, 系统可以通过物料或批次找到对应货架
        /// </summary>
        public static string TaskApiName_BindPodAndMat = "/rcms/services/rest/hikRpcService/bindPodAndMat";

        /// <summary>
        /// 位置禁用与启用, 位置禁用后, 从区域中寻找位置时, 不能被找到
        /// </summary>
        public static string TaskApiName_LockPosition = "/rcms/services/rest/hikRpcService/lockPosition";

        /// <summary>
        /// 全量同步地码数据
        /// </summary>
        public static string TaskApiName_SyncMapDatas = "/rcms/services/rest/hikRpcService/syncMapDatas";

        /// <summary>
        /// 查询货架\储位与物料批次绑定关系
        /// </summary>
        public static string TaskApiName_QueryPodBerthAndMat = "/rcms/services/rest/hikRpcService/queryPodBerthAndMat";

        /// <summary>
        /// 仓位与容器的关系绑定, 容器类型编号写入仓位表
        /// </summary>
        public static string TaskApiName_BindCtnrAndBin = "/rcms/services/rest/hikRpcService/bindCtnrAndBin";
    }
}
