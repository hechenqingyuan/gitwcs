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

using Git.Mes.HikRobot.SDK.ApiName;
using Git.Mes.HikRobot.SDK.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Mes.HikRobot.SDK.Server
{
    public partial class HikRobotTaskServer
    {
        public HikRobotTaskServer() { }

        /// <summary>
        /// 生成海康机器人任务
        /// </summary>
        /// <param name="TaskEntity"></param>
        /// <returns></returns>
        public HikDataResult GenAgvSchedulingTask(GenTaskRequestDTO TaskEntity)
        {
            HikDataResult dataResult = new HikDataResult();

            string ApiName = TaskApiName.TaskApiName_genAgvSchedulingTask;
            JObject param = JObject.Parse(HikJsonHelper.SerializeObject(TaskEntity));
            IHikTopClient client = new HikTopClientDefault();
            string result = client.Execute(ApiName, param);

            dataResult = HikJsonHelper.DeserializeObject<HikDataResult>(result);

            return dataResult;
        }
    }
}
