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

using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Log;
using Git.Mes.HikRobot.SDK;
using Git.Mes.HikRobot.SDK.ApiName;
using Git.Mes.HikRobot.SDK.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Git.HikAgv.App.Server
{
    public partial class AgvCallbackServiceController : ApiController
    {
        private Log log = Log.Instance(typeof(AgvCallbackServiceController));

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HikDataResult TestAgv()
        {
            HikDataResult HikDataResult = new HikDataResult();
            HikDataResult.code = (int)EHikResponseCode.Success;
            HikDataResult.message = "测试连接成功";
            return HikDataResult;
        }

        /// <summary>
        /// AGV 执行回调的方法，包含任务开始，走出储位，任务完成及任务取消。取消通知为任务单，其他通知为单个任务组或子任务。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public HikDataResult<string> AgvCallback([FromBody] TaskCallbackDTO entity)
        {
            HikDataResult<string> HikDataResult = new HikDataResult<string>();
            log.Info("AGV 执行回调的方法: " + HikJsonHelper.SerializeObject(entity));

            string reqCode = entity.reqCode;                            //请求唯一编号
            string currentPositionCode = entity.currentPositionCode;    //当前所在的地图坐标
            string podCode = entity.podCode;                            //AGV搬运的货架编号
            string method = entity.method;                              //参数方法，用于各个模板回传的参数判断
            string robotCode = entity.robotCode;                        //AGV编号
            string taskCode=entity.taskCode;

            log.Info(string.Format("reqCode:{0} ; taskCode:{1} ; method:{2} ;podCode:{3} ;currentPositionCode:{4} ;", reqCode,taskCode, method,podCode, currentPositionCode));
            //坐标转换
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            if (currentPositionCode.IsNotEmpty())
            {
                PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (Position == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("{0}:对应的坐标点数据配置不正确, 请核对", currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }
            }

            log.Info("判断回传的参数:-->" + method);

            /**路线一：开始任务和结束任务监控**/
            if (method == "btnStartOne")
            {
                AgvTaskEntity TaskModel = new AgvTaskEntity();
                TaskModel.reqCode = reqCode;
                TaskModel.currentPositionCode = currentPositionCode;
                TaskModel.podCode = podCode;
                TaskModel.method = method;
                TaskModel.robotCode = robotCode;
                TaskModel.TaskType = "T01";
                TaskModel.taskCode = taskCode;
                log.Info("[btnStartOne]请求号1:{"+taskCode+"}的搬运任务" + HikJsonHelper.SerializeObject(TaskModel));
                TaskModel.EndPositionName = "Apply_A-1";
                PositionEntity PositionModel = ListMap.FirstOrDefault(item => item.PositionName == TaskModel.EndPositionName);
                if (PositionModel != null)
                {
                    TaskModel.EndPositionCode = PositionModel.PositionCode;
                }
                log.Info("[btnStartOne]请求号2:{" + taskCode + "}的搬运任务" + HikJsonHelper.SerializeObject(TaskModel));
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndOne")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndOne]请求号:{0}的搬运任务,在P1暂停点任务与系统记录不一致", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }
                if (podCode.IsEmpty())
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndOne]请求号:{0}的搬运任务,在P1暂停点没有货架,不能继续搬运", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                TaskModel.currentPositionCode = currentPositionCode;
                TaskModel.podCode = podCode;
                TaskModel.method = method;
                TaskModel.robotCode = robotCode;
                TaskModel.TaskType = "T01";
                TaskModel.taskCode = taskCode;

                if (TaskModel.currentPositionCode.IsNotEmpty())
                {
                    GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
                    StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                    StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    StartModel.clientCode = "WMS";
                    StartModel.agvCode = TaskModel.robotCode;
                    StartModel.podCode = TaskModel.podCode;
                    StartModel.taskCode = ConvertHelper.NewGuid().SubString(25);
                    StartModel.positionCodePath = new List<PositionDTO>();

                    //目标点坐标
                    PositionEntity EndBin = ListMap.Where(item => item.PositionName == "A-1" || item.PositionName == "A-2")
                        .Where(item => item.HasCon == (int)EHikBool.No && item.PodCode.IsEmpty())
                        .FirstOrDefault();
                    //PositionEntity EndBin = ListMap.Where(item => item.PositionName == "A-2")
                    //    .Where(item => item.HasCon == (int)EHikBool.No && item.PodCode.IsEmpty())
                    //    .FirstOrDefault();
                    if (EndBin == null)
                    {
                        HikDataResult.code = (int)EHikResponseCode.Exception;
                        HikDataResult.message = string.Format("[btnEndOne]请求号:{0}的搬运任务,在A-1,A-2中无可搬运的目标点", reqCode);
                        log.Info("坐标异常:->" + HikDataResult.message);
                        return HikDataResult;
                    }

                    //起始点坐标
                    PositionEntity BeginBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                    //StartModel.positionCodePath.Add(new PositionDTO() { positionCode = BeginBin.PositionCode, type = "00" });

                    PositionEntity HalfwayBin = null;
                    //确定目标坐标点以及中途停顿点
                    if (EndBin.PositionName == "A-2")
                    {
                        StartModel.taskTyp = "T03";

                        HalfwayBin = ListMap.Where(item => item.PositionName == "Apply_A-2").FirstOrDefault();
                        if (HalfwayBin != null)
                        {
                            StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                        }
                    }
                    else if (EndBin.PositionName == "A-1")
                    {
                        StartModel.taskTyp = "T04";
                        HalfwayBin = ListMap.Where(item => item.PositionName == "Apply_A-3").FirstOrDefault();
                        if (HalfwayBin != null)
                        {
                            StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                        }
                    }

                    StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });

                    if (HalfwayBin != null)
                    {
                        StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                    }

                    //搬运任务状态的变更
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = StartModel.reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = "";
                    TaskModel.robotCode = robotCode;
                    TaskModel.TaskType = StartModel.taskTyp;
                    TaskModel.taskCode = StartModel.taskCode;
                    if (BeginBin != null)
                    {
                        TaskModel.StartPositionCode = BeginBin.PositionCode;
                        TaskModel.StartPositionName = BeginBin.PositionName;
                    }
                    if (HalfwayBin != null)
                    {
                        TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                        TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                    }
                    if (EndBin != null)
                    {
                        TaskModel.EndPositionCode = EndBin.PositionCode;
                        TaskModel.EndPositionName = EndBin.PositionName;
                    }
                    DomainContext.AgvTask = TaskModel;

                    log.Info("[btnEndOne]第一条生产线搬运控制权转移: " + HikJsonHelper.SerializeObject(StartModel));
                    IHikTopClient client = new HikTopClientDefault();
                    JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
                    string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
                    log.Info("[btnEndOne]第一条生产线搬运控制权转移下发任务结果:" + Content);
                }
            }


            /**路线二：开始任务和结束任务监控**/
            else if (method == "btnStartTwo")
            {
                AgvTaskEntity TaskModel = new AgvTaskEntity();
                TaskModel.reqCode = reqCode;
                TaskModel.currentPositionCode = currentPositionCode;
                TaskModel.podCode = podCode;
                TaskModel.method = method;
                TaskModel.robotCode = robotCode;
                TaskModel.taskCode = taskCode;
                TaskModel.TaskType = "T02";

                TaskModel.EndPositionName = "Apply_A-1";
                log.Info("[btnStartTwo]请求号1:{"+taskCode+"}的搬运任务" + HikJsonHelper.SerializeObject(TaskModel));
                PositionEntity PositionModel = ListMap.FirstOrDefault(item => item.PositionName == TaskModel.EndPositionName);
                if (PositionModel != null)
                {
                    TaskModel.EndPositionCode = PositionModel.PositionCode;
                }
                log.Info("[btnStartTwo]请求号2:{" + taskCode + "}的搬运任务" + HikJsonHelper.SerializeObject(TaskModel));
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndTwo")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndTwo]请求号:{0}的搬运任务,在P1暂停点任务与系统记录不一致", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }
                if (podCode.IsEmpty())
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndTwo]请求号:{0}的搬运任务,在P1暂停点没有货架,不能继续搬运", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }
                TaskModel.currentPositionCode = currentPositionCode;
                TaskModel.podCode = podCode;
                TaskModel.method = method;
                TaskModel.robotCode = robotCode;
                TaskModel.taskCode = taskCode;
                TaskModel.TaskType = "T02";

                if (TaskModel.currentPositionCode.IsNotEmpty())
                {
                    GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
                    StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                    StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    StartModel.clientCode = "WMS";
                    StartModel.agvCode = TaskModel.robotCode;
                    StartModel.podCode = TaskModel.podCode;
                    StartModel.taskCode = ConvertHelper.NewGuid().SubString(25);
                    StartModel.positionCodePath = new List<PositionDTO>();

                    //目标点坐标
                    PositionEntity EndBin = ListMap.Where(item => item.PositionName == "C-1" || item.PositionName == "C-2")
                        .Where(item => item.HasCon == (int)EHikBool.No && item.PodCode.IsEmpty())
                        .FirstOrDefault();
                    //PositionEntity EndBin = ListMap.Where(item => item.PositionName == "C-2")
                    //    .Where(item => item.HasCon == (int)EHikBool.No && item.PodCode.IsEmpty())
                    //    .FirstOrDefault();
                    if (EndBin == null)
                    {
                        HikDataResult.code = (int)EHikResponseCode.Exception;
                        HikDataResult.message = string.Format("[btnEndTwo]请求号:{0}的搬运任务,在C-1,C-2中无可搬运的目标点", reqCode);
                        log.Info("坐标异常:->" + HikDataResult.message);
                        return HikDataResult;
                    }

                    //起始点坐标
                    PositionEntity BeginBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                    //StartModel.positionCodePath.Add(new PositionDTO() { positionCode = BeginBin.PositionCode, type = "00" });

                    PositionEntity HalfwayBin = null;
                    //确定目标坐标点以及中途停顿点
                    if (EndBin.PositionName == "C-2")
                    {
                        StartModel.taskTyp = "T05";

                        HalfwayBin = ListMap.Where(item => item.PositionName == "Apply_A-2").FirstOrDefault();
                        if (HalfwayBin != null)
                        {
                            StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                        }
                    }
                    else if (EndBin.PositionName == "C-1")
                    {
                        StartModel.taskTyp = "T06";
                        HalfwayBin = ListMap.Where(item => item.PositionName == "Apply_A-3").FirstOrDefault();
                        if (HalfwayBin != null)
                        {
                            StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                        }
                    }

                    StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });
                    if (HalfwayBin != null)
                    {
                        StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                    }

                    //搬运任务状态的变更
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = StartModel.reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = "";
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = StartModel.taskCode;
                    TaskModel.TaskType = StartModel.taskTyp;
                    if (BeginBin != null)
                    {
                        TaskModel.StartPositionCode = BeginBin.PositionCode;
                        TaskModel.StartPositionName = BeginBin.PositionName;
                    }
                    if (HalfwayBin != null)
                    {
                        TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                        TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                    }
                    if (EndBin != null)
                    {
                        TaskModel.EndPositionCode = EndBin.PositionCode;
                        TaskModel.EndPositionName = EndBin.PositionName;
                    }
                    DomainContext.AgvTask = TaskModel;

                    log.Info("[btnEndTwo]第二条生产线搬运控制权转移: " + HikJsonHelper.SerializeObject(StartModel));
                    IHikTopClient client = new HikTopClientDefault();
                    JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
                    string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
                    log.Info("[btnEndTwo]第二条生产线搬运控制权转移下发任务结果:" + Content);
                }
            }

            /**路线三：P1，P2，A-2 (自动回到P2)**/
            else if (method == "btnStartThree")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.taskCode != taskCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T03";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T03";
                }

                //起始点坐标
                PositionEntity BeginBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-1");
                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-2");
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-2");

                if (BeginBin != null)
                {
                    TaskModel.StartPositionCode = BeginBin.PositionCode;
                    TaskModel.StartPositionName = BeginBin.PositionName;
                }
                if (HalfwayBin != null)
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueThree")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueThree]请求号:{0}的搬运任务,在P2暂停点任务与系统记录不一致", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //进入右边的光栅 1
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-2");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin!=null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 1);
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = "";
                ContinueModel.podCode = TaskModel.podCode;
                //ContinueModel.taskCode = TaskModel.taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };

                log.Info("[btnContinueThree]路线三光栅交互点继续搬运请求参数:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueThree]路线三光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndThree")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                //if (TaskModel == null || TaskModel.taskCode != taskCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnEndThree]请求号:{0}的搬运任务,在A-2点任务与系统记录不一致", taskCode);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //进入右边的光栅 2
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-2");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 2);
                }

                //通知机械手码垛
                DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum = 0;
                DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num = 0;
                if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num == DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum
                    && DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num > 0)
                {
                    ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 2);
                }
                
                //将当前的搬运任务置空
                DomainContext.AgvTask = null;

                //自动开启满料架的搬运
                LoadMapServer MapServer = new LoadMapServer();
                MapServer.StartTask();
            }

            /**路线四：P1，P3，A-1(自动回到P3)**/
            else if (method == "btnStartFour")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.taskCode != taskCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T04";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T04";
                }

                //起始点坐标
                PositionEntity BeginBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-1");
                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-3");
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-1");

                if (BeginBin != null)
                {
                    TaskModel.StartPositionCode = BeginBin.PositionCode;
                    TaskModel.StartPositionName = BeginBin.PositionName;
                }
                if (HalfwayBin != null)
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueFour")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueFour]请求号:{0}的搬运任务,在P3暂停点任务与系统记录不一致", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //进入左边的光栅 1
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-1");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 1);
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = ""; ;
                ContinueModel.podCode = TaskModel.podCode;
                //ContinueModel.taskCode = taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };

                log.Info("[btnContinueFour]路线四光栅交互点继续搬运请求参数:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueFour]路线四光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndFour")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                //if (TaskModel == null || TaskModel.taskCode != taskCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnContinueFour]请求号:{0}的搬运任务,在A-1点任务与系统记录不一致", taskCode);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //离开左边的光栅 2
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-1");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 2);
                }

                //通知机械手码垛
                DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum = 0;
                DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num = 0;
                if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num == DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum
                    && DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num > 0)
                {
                    ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 1);
                }
                
                //将当前的搬运任务置空
                DomainContext.AgvTask = null;

                //自动开启满料架的搬运
                LoadMapServer MapServer = new LoadMapServer();
                MapServer.StartTask();
            }

            /**路线五：P1，P2，C-2(自动回到P2)**/
            else if (method == "btnStartFive")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.taskCode != taskCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T05";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T05";
                }

                //起始点坐标
                PositionEntity BeginBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-1");
                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-2");
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-2");

                if (BeginBin != null)
                {
                    TaskModel.StartPositionCode = BeginBin.PositionCode;
                    TaskModel.StartPositionName = BeginBin.PositionName;
                }
                if (HalfwayBin != null)
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueFive")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueFive]请求号:{0}的搬运任务,在P2暂停点任务与系统记录不一致", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //进入右边的光栅
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-2");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 1);
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = "";
                ContinueModel.podCode = TaskModel.podCode;
                //ContinueModel.taskCode = TaskModel.taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };

                log.Info("[btnContinueFive]路线五光栅交互点继续搬运请求参数:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueFive]路线五光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndFive")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                //if (TaskModel == null || TaskModel.taskCode != taskCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnEndFive]请求号:{0}的搬运任务,在C-2点任务与系统记录不一致", taskCode);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //离开右边的光栅 2
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-2");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 2);
                }

                //通知机械手码垛
                DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum = 0;
                DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num = 0;
                if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num == DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum
                    && DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num > 0)
                {
                    ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 2);
                }
                
                //将当前的搬运任务置空
                DomainContext.AgvTask = null;

                //自动开启满料架的搬运
                LoadMapServer MapServer = new LoadMapServer();
                MapServer.StartTask();
            }

            /**路线六：P1，P3，C-1(自动回到P3)**/
            else if (method == "btnStartSix")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.taskCode != taskCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T06";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "T06";
                }

                //起始点坐标
                PositionEntity BeginBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-1");
                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-3");
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-1");

                if (BeginBin != null)
                {
                    TaskModel.StartPositionCode = BeginBin.PositionCode;
                    TaskModel.StartPositionName = BeginBin.PositionName;
                }
                if (HalfwayBin != null)
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueSix")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnStartSix]请求号:{0}的搬运任务,在P3暂停点任务与系统记录不一致", taskCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //进入左边的光栅 1 
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-1");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 1);
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = "";
                ContinueModel.podCode = TaskModel.podCode;
                //ContinueModel.taskCode = TaskModel.taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };

                log.Info("[btnStartSix]路线六光栅交互点继续搬运请求参数:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnStartSix]路线六光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndSix")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                //if (TaskModel == null || TaskModel.taskCode != taskCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("请求号:{0}的搬运任务,在C-1点任务与系统记录不一致", taskCode);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //离开左边的光栅 1
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-1");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 0);
                }

                //通知机械手码垛
                DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum = 0;
                DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num = 0;
                if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num == DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum
                    && DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num > 0)
                {
                    ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 1);
                }
                
                //将当前的搬运任务置空
                DomainContext.AgvTask = null;

                //自动开启满料架的搬运
                LoadMapServer MapServer = new LoadMapServer();
                MapServer.StartTask();
            }

            /**路线七：A-2[省略],P2,P3,A-1 (或者A-1[省略],P3,P2,A-2)**/
            else if (method == "btnStartSeven")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.taskCode != taskCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S01";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S01";
                }

                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueSeven")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (HalfwayBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueSeven]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueSeven]请求号:{0}的搬运任务,在{1}暂停点任务与系统记录不一致", taskCode, HalfwayBin.PositionName);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                PositionEntity EndBin = null;
                if (HalfwayBin.PositionName == "Apply_A-2")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-2");
                }
                else if (HalfwayBin.PositionName == "Apply_A-3")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-1");
                }

                if (TaskModel.HalfwayPositionCode.IsEmpty())
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null && TaskModel.EndPositionCode.IsEmpty())
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }

                //进入光栅搬运满料架
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    if (EndBin.PositionName == "A-1")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 1);
                    }
                    else if (EndBin.PositionName == "A-2")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 1);
                    }
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                ContinueModel.podCode = "";
                //ContinueModel.taskCode = TaskModel.taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };
                log.Info("[btnContinueSeven]路线七光栅交互点继续搬运请求参数:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                param.Add("agvCode", robotCode);
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueSeven]路线七光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndSeven")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity CurrentBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (CurrentBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndSeven]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //if (TaskModel == null || TaskModel.taskCode != taskCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnEndSeven]请求号:{0}的搬运任务,在{1}点任务与系统记录不一致", taskCode, CurrentBin.PositionName);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //将当前的搬运任务置空
                DomainContext.AgvTask = null;

                PositionEntity EndBin = ListMap.Where(item => item.PositionName == "B-1" || item.PositionName == "B-2")
                                .Where(item => item.HasCon == (int)EHikBool.No || item.PodCode.IsEmpty())
                                .FirstOrDefault();
                if (EndBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndSeven]搬运满料架到工台：无可用的空库位用于搬运，其实坐标位:{0}", CurrentBin.PositionName);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                PositionEntity BeginBin = null;
                PositionEntity HalfwayBin = null;

                BeginBin = CurrentBin;
                if (BeginBin.PositionName == "A-1")
                {
                    HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-3");
                }
                else if (BeginBin.PositionName == "A-2")
                {
                    HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-2");
                }

                if (BeginBin.PodCode.IsNotEmpty())
                {
                    podCode = BeginBin.PodCode;
                }

                GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
                StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StartModel.clientCode = "WMS";
                StartModel.agvCode = robotCode;
                StartModel.podCode = podCode;
                StartModel.taskTyp = "S03";
                StartModel.taskCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.positionCodePath = new List<PositionDTO>();
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = BeginBin.PositionCode, type = "00" });
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });

                //搬运任务状态的变更
                TaskModel = new AgvTaskEntity();
                TaskModel.reqCode = StartModel.reqCode;
                TaskModel.currentPositionCode = currentPositionCode;
                TaskModel.podCode = podCode;
                TaskModel.robotCode = robotCode;
                TaskModel.TaskType = StartModel.taskTyp;
                TaskModel.taskCode = StartModel.taskCode;
                if (BeginBin != null)
                {
                    TaskModel.StartPositionCode = BeginBin.PositionCode;
                    TaskModel.StartPositionName = BeginBin.PositionName;
                }
                if (HalfwayBin != null)
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;

                log.Info("[btnEndSeven]第一条生产线搬运控制权转移(使用路线九 S03): " + HikJsonHelper.SerializeObject(StartModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
                string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
                log.Info("[btnEndSeven]第一条生产线搬运控制权转移下发任务结果(使用路线九 S03):" + Content);
            }

            /**路线八：C-2[省略],P2,P3,C-1 (或者C-1[省略],P3,P2,C-2)**/
            else if (method == "btnStartEight")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.taskCode != taskCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S02";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S02";
                }

                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueEight")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (HalfwayBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueEight]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueEight]请求号:{0}的搬运任务,在{1}暂停点任务与系统记录不一致", taskCode, HalfwayBin.PositionName);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                PositionEntity EndBin = null;
                if (HalfwayBin.PositionName == "Apply_A-2")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-2");
                }
                else if (HalfwayBin.PositionName == "Apply_A-3")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-1");
                }

                if (TaskModel.HalfwayPositionCode.IsEmpty())
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null && TaskModel.EndPositionCode.IsEmpty())
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }

                //进入光栅搬运满料架
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    if (EndBin.PositionName == "C-1")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 1);
                    }
                    else if (EndBin.PositionName == "C-2")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 1);
                    }
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = "";
                ContinueModel.podCode = "";
                //ContinueModel.taskCode = taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };
                log.Info("[btnContinueEight]路线八光栅交互点继续搬运请求参数:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                param.Add("agvCode", robotCode);
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueEight]路线八光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndEight")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity CurrentBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (CurrentBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndEight]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //if (TaskModel == null || TaskModel.taskCode != taskCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnEndEight]请求号:{0}的搬运任务,在{1}点任务与系统记录不一致", taskCode, CurrentBin.PositionName);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //将当前的搬运任务置空
                DomainContext.AgvTask = null;

                PositionEntity EndBin = ListMap.Where(item => item.PositionName == "D-1" || item.PositionName == "D-2")
                                .Where(item => item.HasCon == (int)EHikBool.No || item.PodCode.IsEmpty())
                                .FirstOrDefault();
                if (EndBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndEight]搬运满料架到工台：无可用的空库位用于搬运，其实坐标位:{0}", CurrentBin.PositionName);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                PositionEntity BeginBin = null;
                PositionEntity HalfwayBin = null;

                BeginBin = CurrentBin;
                if (BeginBin.PositionName == "C-1")
                {
                    HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-3");
                }
                else if (BeginBin.PositionName == "C-2")
                {
                    HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-2");
                }
                if (BeginBin.PodCode.IsNotEmpty())
                {
                    podCode = BeginBin.PodCode;
                }

                GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
                StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StartModel.clientCode = "WMS";
                StartModel.agvCode = robotCode;
                StartModel.podCode = podCode;
                StartModel.taskTyp = "S04";
                StartModel.taskCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.positionCodePath = new List<PositionDTO>();
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = BeginBin.PositionCode, type = "00" });
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });

                //搬运任务状态的变更
                TaskModel = new AgvTaskEntity();
                TaskModel.reqCode = StartModel.reqCode;
                TaskModel.currentPositionCode = currentPositionCode;
                TaskModel.podCode = podCode;
                TaskModel.robotCode = robotCode;
                TaskModel.TaskType = StartModel.taskTyp;
                TaskModel.taskCode = StartModel.taskCode;
                if (BeginBin != null)
                {
                    TaskModel.StartPositionCode = BeginBin.PositionCode;
                    TaskModel.StartPositionName = BeginBin.PositionName;
                }
                if (HalfwayBin != null)
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;

                log.Info("[btnEndEight]第一条生产线搬运控制权转移(使用路线十 S04): " + HikJsonHelper.SerializeObject(StartModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
                string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
                log.Info("[btnEndEight]第一条生产线搬运控制权转移下发任务结果(使用路线十 S04):" + Content);
            }

            /**路线九：A-2,P2,B-2(B-1) 【或者A-1,P3,B-2(B-1)】**/
            else if (method == "btnStartNine")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.reqCode != reqCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S03";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S03";
                }

                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueNine")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (HalfwayBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueNine]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueNine]请求号:{0}的搬运任务,在{1}暂停点任务与系统记录不一致", taskCode, HalfwayBin.PositionName);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                string PrePodCode = TaskModel.podCode;
                if (podCode.IsNotEmpty())
                {
                    TaskModel.podCode = podCode;
                }
                if (TaskModel.podCode.IsEmpty())
                {
                    TaskModel.podCode = HalfwayBin.PodCode;
                }
                if (TaskModel.podCode.IsEmpty())
                {
                    TaskModel.podCode = PrePodCode;
                }
                TaskModel.robotCode = robotCode;

                PositionEntity EndBin = null;
                if (HalfwayBin.PositionName == "Apply_A-2")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-2");
                }
                else if (HalfwayBin.PositionName == "Apply_A-3")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-1");
                }

                if (TaskModel.HalfwayPositionCode.IsEmpty())
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null && TaskModel.EndPositionCode.IsEmpty())
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }

                //离开光栅(搬运到工台)
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (ModbusClient != null)
                {
                    if (HalfwayBin.PositionName == "Apply_A-2")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 2);
                    }
                    else if (HalfwayBin.PositionName == "Apply_A-3")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 2);
                    }
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = "";
                ContinueModel.podCode = TaskModel.podCode;
                //ContinueModel.taskCode = TaskModel.taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };

                log.Info("[btnContinueNine]路线九光栅交互点继续搬运请求内容:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueNine]路线九光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndNine")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity CurrentBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (CurrentBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndNine]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //if (TaskModel == null || TaskModel.reqCode != reqCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnEndNine]请求号:{0}的搬运任务,在{1}点任务与系统记录不一致", reqCode, CurrentBin.PositionName);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //将当前的搬运任务置空
                DomainContext.AgvTask = null;
            }

            /**路线十：C-2,P2,D-2(D-1) 【或者C-1,P3,D-2(D-1)】**/
            else if (method == "btnStartTen")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;
                if (TaskModel == null || TaskModel.reqCode != reqCode)
                {
                    TaskModel = new AgvTaskEntity();
                    TaskModel.reqCode = reqCode;
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S04";
                }
                else
                {
                    TaskModel.currentPositionCode = currentPositionCode;
                    TaskModel.podCode = podCode;
                    TaskModel.method = method;
                    TaskModel.robotCode = robotCode;
                    TaskModel.taskCode = taskCode;
                    TaskModel.TaskType = "S04";
                }

                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnContinueTen")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity HalfwayBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (HalfwayBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueTen]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                if (TaskModel == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnContinueTen]请求号:{0}的搬运任务,在{1}暂停点任务与系统记录不一致", taskCode, HalfwayBin.PositionName);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                string PrePodCode = TaskModel.podCode;
                if (podCode.IsNotEmpty())
                {
                    TaskModel.podCode = podCode;
                }
                if (TaskModel.podCode.IsEmpty())
                {
                    TaskModel.podCode = HalfwayBin.PodCode;
                }
                if (TaskModel.podCode.IsEmpty())
                {
                    TaskModel.podCode = PrePodCode;
                }
                TaskModel.robotCode = robotCode;

                PositionEntity EndBin = null;
                if (HalfwayBin.PositionName == "Apply_A-2")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-2");
                }
                else if (HalfwayBin.PositionName == "Apply_A-3")
                {
                    EndBin = ListMap.FirstOrDefault(item => item.PositionName == "C-1");
                }

                if (TaskModel.HalfwayPositionCode.IsEmpty())
                {
                    TaskModel.HalfwayPositionCode = HalfwayBin.PositionCode;
                    TaskModel.HalfwayPositionName = HalfwayBin.PositionName;
                }
                if (EndBin != null && TaskModel.EndPositionCode.IsEmpty())
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }

                //离开光栅(搬运到工台)
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (ModbusClient != null)
                {
                    if (HalfwayBin.PositionName == "Apply_A-2")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 2);
                    }
                    else if (HalfwayBin.PositionName == "Apply_A-3")
                    {
                        ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 2);
                    }
                }

                //继续搬运
                ContinueTaskRequestDTO ContinueModel = new ContinueTaskRequestDTO();
                ContinueModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                //ContinueModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //ContinueModel.clientCode = "WMS";
                //ContinueModel.agvCode = "";
                ContinueModel.podCode = TaskModel.podCode;
                //ContinueModel.taskCode = TaskModel.taskCode;
                ContinueModel.nextPositionCode = new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" };

                log.Info("[btnContinueTen]路线十光栅交互点继续搬运请求内容:" + HikJsonHelper.SerializeObject(ContinueModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(ContinueModel));
                string Content = client.Execute(TaskApiName.TaskApiName_continueTask, param);
                log.Info("[btnContinueTen]路线十光栅交互点继续搬运请求返回结果:" + Content);

                //记录AGV的搬运状态
                TaskModel.method = method;
                TaskModel.currentPositionCode = currentPositionCode;
                DomainContext.AgvTask = TaskModel;
            }
            else if (method == "btnEndTen")
            {
                AgvTaskEntity TaskModel = DomainContext.AgvTask;

                PositionEntity CurrentBin = ListMap.FirstOrDefault(item => item.PositionCode == currentPositionCode);
                if (CurrentBin == null)
                {
                    HikDataResult.code = (int)EHikResponseCode.Exception;
                    HikDataResult.message = string.Format("[btnEndTen]请求号:{0}的搬运任务,继续任务的坐标{1}不存在", taskCode, currentPositionCode);
                    log.Info("坐标异常:->" + HikDataResult.message);
                    return HikDataResult;
                }

                //if (TaskModel == null || TaskModel.reqCode != reqCode)
                //{
                //    HikDataResult.code = (int)EHikResponseCode.Exception;
                //    HikDataResult.message = string.Format("[btnEndTen]请求号:{0}的搬运任务,在{1}点任务与系统记录不一致", taskCode, CurrentBin.PositionName);
                //    log.Info("坐标异常:->" + HikDataResult.message);
                //    return HikDataResult;
                //}

                //将当前的搬运任务置空
                DomainContext.AgvTask = null;
            }
            
            HikDataResult.code = 0;
            HikDataResult.reqCode = entity.reqCode;
            HikDataResult.message = "响应成功";

            return HikDataResult;
        }

        /// <summary>
        /// 告警推送回调的方法，调度系统将导致 AGV 停止运行的严重告警推送给上层系统 推送频率：10 秒一次
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public HikDataResult WarnCallback([FromBody] WarnCallbackRequestDTO entity)
        {
            HikDataResult HikDataResult = new HikDataResult();
            log.Info("告警推送回调的方法: " + HikJsonHelper.SerializeObject(entity));
            return HikDataResult;
        }

        /// <summary>
        /// 可在执行绑定货架与储位,绑定货架与物料,绑定仓位与容器后通知上层
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public HikDataResult BindNotify([FromBody] BindNotifyRequestDTO entity)
        {
            HikDataResult HikDataResult = new HikDataResult();
            log.Info("可在执行绑定货架与储位,绑定货架与物料,绑定仓位与容器后通知上层: " + HikJsonHelper.SerializeObject(entity));
            return HikDataResult;
        }
    }
}
