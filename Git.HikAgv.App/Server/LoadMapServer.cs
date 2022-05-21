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
using Git.Framework.Io;
using Git.Framework.Log;
using Git.Framework.Resource;
using Git.Mes.HikRobot.SDK;
using Git.Mes.HikRobot.SDK.ApiName;
using Git.Mes.HikRobot.SDK.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.HikAgv.App.Server
{
    /// <summary>
    /// 加载地图信息
    /// </summary>
    public partial class LoadMapServer
    { 
        public LoadMapServer() { }

        private string FileName = "Map.txt";

        private Log log = Log.Instance(typeof(LoadMapServer));

        /// <summary>
        /// 加载地图配置
        /// </summary>
        public void Load()
        {
            string FilePath = FileManager.GetDomainRoot() + "\\MapConfig\\"+FileName;

            StreamReader Sr = new StreamReader(FilePath);
            string Content=Sr.ReadToEnd();
            Sr.Close();

            DomainContext.ListMap= HikJsonHelper.DeserializeObject<List<PositionEntity>>(Content);
        }

        /// <summary>
        /// 保存地图数据信息
        /// </summary>
        public void Save()
        {
            string FilePath = FileManager.GetDomainRoot() + "\\MapConfig\\" + FileName;

            string Content = "";

            if (DomainContext.ListMap.IsNullOrEmpty())
            {
                Content = HikJsonHelper.SerializeObject(DomainContext.ListMap);
            }
            StreamWriter Sw = new StreamWriter(FilePath);
            Sw.Write(Content);
            Sw.Flush();
            Sw.Close();
        }

        /// <summary>
        /// 更新地图中料架与地图坐标之间的关系
        /// </summary>
        public void UpdateBin()
        {
            List<PalletEntity> ListPallet = DomainContext.ListPallet;
            ListPallet = ListPallet.IsNull() ? new List<PalletEntity>() : ListPallet;

            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            QueryPodBerthAndMatRequestDTO Model = new QueryPodBerthAndMatRequestDTO();
            Model.reqCode = ConvertHelper.NewGuid().SubString(25);
            Model.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Model.clientCode = "WMS";
            Model.tokenCode = "";
            Model.podCode = "";
            Model.materialLot = "";
            Model.positionCode = "";
            Model.areaCode = "";
            Model.mapShortName = ListMap.First().MapShortName;

            IHikTopClient client = new HikTopClientDefault();
            JObject param = JObject.Parse(HikJsonHelper.SerializeObject(Model));
            string Content=client.Execute(TaskApiName.TaskApiName_QueryPodBerthAndMat, param);

            //log.Info("更新地图中料架与地图坐标之间的关系:"+Content);

            HikDataResult<List<QueryPodBerthAndMatResultDTO>> dataResult = HikJsonHelper.DeserializeObject<HikDataResult<List<QueryPodBerthAndMatResultDTO>>>(Content);

            if (dataResult.code == 0)
            {
                List<QueryPodBerthAndMatResultDTO> listResult = dataResult.data;
                listResult = listResult.IsNull() ? new List<QueryPodBerthAndMatResultDTO>() : listResult;

                //托盘位置的处理
                foreach (QueryPodBerthAndMatResultDTO ModelItem in listResult)
                {
                    string positionCode = ModelItem.positionCode;
                    string podCode = ModelItem.podCode;

                    PalletEntity PalletModel = ListPallet.FirstOrDefault(item=>item.PodCode== podCode);
                    if (PalletModel != null)
                    {
                        PalletModel.PositionCode = ModelItem.positionCode;
                    }
                    if (PalletModel.PositionCode.IsNotEmpty())
                    {
                        PositionEntity BinEntity = ListMap.FirstOrDefault(item=>item.PositionCode== PalletModel.PositionCode);
                        if (BinEntity != null)
                        {
                            PalletModel.PositionName = BinEntity.PositionName;
                        }
                    }
                }
                //ListPallet.Remove(item => !listResult.Exists(a => a.podCode == item.PodCode));
                DomainContext.ListPallet = ListPallet;
                //log.Info("坐标数据：" + HikJsonHelper.SerializeObject(ListPallet));

                //坐标位置的处理
                foreach (PositionEntity ModelItem in ListMap)
                {
                    ModelItem.HasCon = (int)EHikBool.No;
                    ModelItem.PodCode = "";
                    QueryPodBerthAndMatResultDTO ModelResult = listResult.FirstOrDefault(item=>item.positionCode==ModelItem.PositionCode);
                    if (ModelResult != null)
                    {
                        if (ModelResult.podCode.IsNotEmpty())
                        {
                            ModelItem.HasCon = (int)EHikBool.Yes;
                            ModelItem.PodCode = ModelResult.podCode;
                        }
                        else
                        {
                            ModelItem.HasCon = (int)EHikBool.No;
                            ModelItem.PodCode = "";
                        }
                    }

                    if (ModelItem.PositionName == "A-1")
                    {
                        ModelItem.Num = DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num;
                        ModelItem.TotalNum = DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum;
                        ModelItem.Status = DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_LeftGrating_Status;
                    }
                    else if (ModelItem.PositionName == "A-2")
                    {
                        ModelItem.Num = DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num;
                        ModelItem.TotalNum = DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum;
                        ModelItem.Status = DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_RightGrating_Status;
                    }
                    else if (ModelItem.PositionName == "C-1")
                    {
                        ModelItem.Num = DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num;
                        ModelItem.TotalNum = DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum;
                        ModelItem.Status = DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_LeftGrating_Status;
                    }
                    else if (ModelItem.PositionName == "C-2")
                    {
                        ModelItem.Num = DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num;
                        ModelItem.TotalNum = DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum;
                        ModelItem.Status = DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_RightGrating_Status;
                    }
                }
                //log.Info("地图数据：" + HikJsonHelper.SerializeObject(ListMap));
            }
        }

        /// <summary>
        /// 系统自动开启满料架的搬运
        /// </summary>
        /// <param name="reqCode"></param>
        /// <param name="currentPositionCode"></param>
        /// <param name="method"></param>
        /// <param name="robotCode"></param>
        /// <param name="podCode"></param>
        /// <returns></returns>
        public HikDataResult StartTask()
        {
            HikDataResult HikDataResult = new HikDataResult();

            //坐标转换
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            InteractiveEntity Interactive_One = DomainContext.Interactive_One;
            InteractiveEntity Interactive_Two = DomainContext.Interactive_Two;

            List<Params<string, DateTime, string>> ListSource = new List<Params<string, DateTime, string>>();

            if (Interactive_One != null)
            {
                if (Interactive_One.DB_RobotToAgv_Palletizing_Left_Num == Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum 
                    && Interactive_One.DB_RobotToAgv_Palletizing_Notify!=1)
                {
                    if (Interactive_One.DB_RobotToAgv_Palletizing_Left_Num > 0 && Interactive_One.LeftSnNum.IsNotEmpty())
                    {
                        Params<string, DateTime,string> paramA1 = new Params<string, DateTime,string>();
                        paramA1.Item1 = "A-1";
                        paramA1.Item2 = Interactive_One.LeftUpdateTime;
                        paramA1.Item3 = Interactive_One.LeftSnNum;

                        PositionEntity CheckBin = ListMap.FirstOrDefault(item => item.PositionName == paramA1.Item1);
                        if (CheckBin != null && CheckBin.PodCode.IsNotEmpty())
                        {
                            ListSource.Add(paramA1);
                        }
                    }
                }

                if (Interactive_One.DB_RobotToAgv_Palletizing_Right_Num == Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum 
                    && Interactive_One.DB_RobotToAgv_Palletizing_Notify != 2)
                {
                    if (Interactive_One.DB_RobotToAgv_Palletizing_Right_Num > 0 && Interactive_One.RightSnNum.IsNotEmpty())
                    {
                        Params<string, DateTime, string> paramA2 = new Params<string, DateTime, string>();
                        paramA2.Item1 = "A-2";
                        paramA2.Item2 = Interactive_One.RightUpdateTime;
                        paramA2.Item3 = Interactive_One.RightSnNum;
                        PositionEntity CheckBin = ListMap.FirstOrDefault(item => item.PositionName == paramA2.Item1);
                        if (CheckBin != null && CheckBin.PodCode.IsNotEmpty())
                        {
                            ListSource.Add(paramA2);
                        }
                    }
                }
            }

            if (Interactive_Two != null)
            {
                if (Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num == Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum 
                    && Interactive_Two.DB_RobotToAgv_Palletizing_Notify != 1)
                {
                    if (Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num > 0 && Interactive_Two.LeftSnNum.IsNotEmpty())
                    {
                        Params<string, DateTime, string> paramC1 = new Params<string, DateTime, string>();
                        paramC1.Item1 = "C-1";
                        paramC1.Item2 = Interactive_Two.LeftUpdateTime;
                        paramC1.Item3 = Interactive_Two.LeftSnNum;
                        PositionEntity CheckBin = ListMap.FirstOrDefault(item => item.PositionName == paramC1.Item1);
                        if (CheckBin != null && CheckBin.PodCode.IsNotEmpty())
                        {
                            ListSource.Add(paramC1);
                        }
                    }
                }

                if (Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num == Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum 
                    && Interactive_Two.DB_RobotToAgv_Palletizing_Notify != 2)
                {
                    if (Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num > 0 && Interactive_Two.RightSnNum.IsNotEmpty())
                    {
                        Params<string, DateTime,string> paramC2 = new Params<string, DateTime,string>();
                        paramC2.Item1 = "C-2";
                        paramC2.Item2 = Interactive_Two.RightUpdateTime;
                        paramC2.Item3 = Interactive_Two.RightSnNum;
                        PositionEntity CheckBin = ListMap.FirstOrDefault(item => item.PositionName == paramC2.Item1);
                        if (CheckBin != null && CheckBin.PodCode.IsNotEmpty())
                        {
                            ListSource.Add(paramC2);
                        }
                    }
                }
            }

            if (ListSource.IsNullOrEmpty())
            {
                HikDataResult.code = (int)EHikResponseCode.Exception;
                HikDataResult.message = string.Format("{0} 暂无满料架可用于搬运",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return HikDataResult;
            }

            Params<string, DateTime,string> ParamResult = ListSource.FirstOrDefault(item => item.Item2 == ListSource.Min(a => a.Item2));

            //起始点坐标
            PositionEntity BeginBin = null;

            //中转坐标
            PositionEntity HalfwayBin = null;

            //目标位置
            PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == ParamResult.Item1);

            GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
            StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
            StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StartModel.clientCode = "WMS";
            StartModel.agvCode = "5763";
            StartModel.podCode = "";
            StartModel.taskCode= ConvertHelper.NewGuid().SubString(25);
            StartModel.positionCodePath = new List<PositionDTO>();

            if (EndBin.PositionName == "A-1")
            {
                HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-3");
                StartModel.taskTyp = "S01";
            }
            else if (EndBin.PositionName == "A-2")
            {
                HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-2");
                StartModel.taskTyp = "S01";
            }
            else if (EndBin.PositionName == "C-1")
            {
                HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-3");
                StartModel.taskTyp = "S02";
            }
            else if (EndBin.PositionName == "C-2")
            {
                HalfwayBin = ListMap.FirstOrDefault(item => item.PositionName == "Apply_A-2");
                StartModel.taskTyp = "S02";
            }
            StartModel.positionCodePath.Add(new PositionDTO() { positionCode = HalfwayBin.PositionCode, type = "00" });
            StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });

            //搬运任务状态的变更
            AgvTaskEntity TaskModel = new AgvTaskEntity();
            TaskModel.reqCode = StartModel.reqCode;
            TaskModel.currentPositionCode = "";
            TaskModel.podCode = "";
            TaskModel.method = "";
            TaskModel.robotCode = "";
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

            log.Info("由WCS自动发起搬运满料架任务: " + HikJsonHelper.SerializeObject(StartModel));
            IHikTopClient client = new HikTopClientDefault();
            JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
            string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
            log.Info("由WCS自动发起搬运满料架任务下发任务结果:" + Content);

            HikDataResult WCSDataResult = HikJsonHelper.DeserializeObject<HikDataResult>(Content);
            if (WCSDataResult.code==0)
            {
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin.PositionName == "A-1")
                {
                    //搬运左边的料架,推荐使用右边的机械手
                    DomainContext.Interactive_One.LeftSnNum = string.Empty;
                    DomainContext.Interactive_One.LeftUpdateTime = DateTime.MinValue;
                    DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num = 0;
                    DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum = 0;

                    PositionEntity RightEntity = ListMap.FirstOrDefault(item => item.PositionName == "A-2");
                    if (RightEntity != null 
                        && RightEntity.PodCode.IsNotEmpty() 
                        && DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num!= DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum)
                    {
                        ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 2);
                    }
                }
                else if (EndBin.PositionName == "A-2")
                {
                    //搬运右边的料架,推荐使用左边的机械手
                    DomainContext.Interactive_One.RightSnNum = string.Empty;
                    DomainContext.Interactive_One.RightUpdateTime = DateTime.MinValue;
                    DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num = 0;
                    DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum = 0;

                    PositionEntity LeftEntity = ListMap.FirstOrDefault(item => item.PositionName == "A-1");
                    if (LeftEntity != null
                        && LeftEntity.PodCode.IsNotEmpty()
                        && DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num != DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum)
                    {
                        ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 1);
                    }
                }
                else if (EndBin.PositionName == "C-1")
                {
                    //搬运左边的料架,推荐使用右边的机械手
                    DomainContext.Interactive_Two.LeftSnNum = string.Empty;
                    DomainContext.Interactive_Two.LeftUpdateTime = DateTime.MinValue;
                    DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num = 0;
                    DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum = 0;

                    PositionEntity RightEntity = ListMap.FirstOrDefault(item => item.PositionName == "C-2");
                    if (RightEntity != null
                        && RightEntity.PodCode.IsNotEmpty()
                        && DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num != DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum)
                    {
                        ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 2);
                    }
                }
                else if (EndBin.PositionName == "C-2")
                {
                    //搬运右边的料架,推荐使用左边的机械手
                    DomainContext.Interactive_Two.RightSnNum = string.Empty;
                    DomainContext.Interactive_Two.RightUpdateTime = DateTime.MinValue;
                    DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num = 0;
                    DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum = 0;

                    PositionEntity LeftEntity = ListMap.FirstOrDefault(item => item.PositionName == "C-1");
                    if (LeftEntity != null
                        && LeftEntity.PodCode.IsNotEmpty()
                        && DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num != DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum)
                    {
                        ModbusClient.Write_Palletizing_Notify(EndBin.PositionCode, 1);
                    }
                }
            }
            return HikDataResult;
        }

        /// <summary>
        /// 查询AGV的状态
        /// </summary>
        /// <returns></returns>
        public HikDataResult QueryAgv()
        {
            HikDataResult dataResult = new HikDataResult();

            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            QueryAgvStatusRequestDTO QueryModel = new QueryAgvStatusRequestDTO();
            QueryModel.reqCode = ConvertHelper.NewGuid().SubString(25);
            QueryModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            QueryModel.clientCode = "WMS";
            QueryModel.tokenCode = "";
            QueryModel.mapShortName = ListMap.First().MapShortName;

            string BaseApi= ResourceManager.GetSettingEntity("HikRobot_API_URL").Value;
            IHikTopClient client = new HikTopClientDefault(BaseApi);
            JObject param = JObject.Parse(HikJsonHelper.SerializeObject(QueryModel));
            string Content = client.Execute(TaskApiName.TaskApiName_QueryAgvStatus, param);

            log.Info("查询AGV的状态："+Content);

            HikDataResult<List<QueryAgvStatusResultDTO>> QueryDataResult = HikJsonHelper.DeserializeObject<HikDataResult<List<QueryAgvStatusResultDTO>>>(Content);

            List<QueryAgvStatusResultDTO> ListResult = QueryDataResult.data;
            ListResult = ListResult.IsNull() ? new List<QueryAgvStatusResultDTO>() : ListResult;

            DomainContext.ListAgv = ListResult;

            dataResult.code = (int)EHikResponseCode.Success;
            dataResult.message = "响应成功";

            return dataResult;
        }
    }
}
