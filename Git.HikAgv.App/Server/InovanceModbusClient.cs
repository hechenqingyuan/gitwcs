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
using Git.Framework.Resource;
using Git.HikAgv.App.OPCModel;
using Git.Mes.HikRobot.SDK;
using HslCommunication;
using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.HikAgv.App.Server
{
    /// <summary>
    /// 汇川机器人PLC交互
    /// </summary>
    public partial class InovanceModbusClient
    {
        /// <summary>
        /// Modbus 客户端连接池
        /// </summary>
        private Dictionary<string, ModbusTcpNet> DicTcpClient = null;
        private Log log = Log.Instance(typeof(InovanceModbusClient));

        private string ChannelName = "Inovance";

        /// <summary>
        /// 构造方法
        /// </summary>
        public InovanceModbusClient() { }

        /// <summary>
        /// 初始化 Modbus 连接
        /// </summary>
        public void Init()
        {
            if (this.DicTcpClient == null)
            {
                this.DicTcpClient = new Dictionary<string, ModbusTcpNet>();
            }

            //第一组PLC连接
            if (true)
            {
                string Key = "Inovance1";
                ModbusTcpNet busTcpClient = null;
                if (this.DicTcpClient.ContainsKey(Key))
                {
                    busTcpClient = this.DicTcpClient[Key];
                }

                if (busTcpClient == null)
                {
                    string Ip = ResourceManager.GetSettingEntity("Inovance_IP_1").Value;
                    string Port = ResourceManager.GetSettingEntity("Inovance_Port_1").Value;
                    string Slave = ResourceManager.GetSettingEntity("Inovance_Slave_1").Value;

                    busTcpClient = new ModbusTcpNet(Ip, ConvertHelper.ToType<int>(Port, 502), ConvertHelper.ToType<byte>(Slave));
                    busTcpClient.AddressStartWithZero = true;
                    OperateResult connect = busTcpClient.ConnectServer();

                    if (connect.IsSuccess)
                    {
                        this.DicTcpClient.Add(Key, busTcpClient);
                        log.Info("Inovance1连成功");
                    }
                }
            }

            //第二组PLC连接
            if (true)
            {
                string Key = "Inovance2";
                ModbusTcpNet busTcpClient = null;
                if (this.DicTcpClient.ContainsKey(Key))
                {
                    busTcpClient = this.DicTcpClient[Key];
                }

                if (busTcpClient == null)
                {
                    string Ip = ResourceManager.GetSettingEntity("Inovance_IP_2").Value;
                    string Port = ResourceManager.GetSettingEntity("Inovance_Port_2").Value;
                    string Slave = ResourceManager.GetSettingEntity("Inovance_Slave_2").Value;

                    busTcpClient = new ModbusTcpNet(Ip, ConvertHelper.ToType<int>(Port, 502), ConvertHelper.ToType<byte>(Slave));
                    busTcpClient.AddressStartWithZero = true;
                    OperateResult connect = busTcpClient.ConnectServer();

                    if (connect.IsSuccess)
                    {
                        this.DicTcpClient.Add(Key, busTcpClient);
                        log.Info("Inovance2连成功");
                    }
                }
            }

            //初始化 机械手和AGV混合搬运交互点状态
            DomainContext.Interactive_One = DomainContext.Interactive_One.IsNull() ? new InteractiveEntity():DomainContext.Interactive_One;
            DomainContext.Interactive_Two = DomainContext.Interactive_Two.IsNull() ? new InteractiveEntity() : DomainContext.Interactive_Two;
        }

        /// <summary>
        /// 根据PLC代号获取Modbus客户端连接
        /// </summary>
        /// <param name="PlcCode"></param>
        /// <returns></returns>
        public ModbusTcpNet GetBusTcpClient(string PlcCode)
        {
            if (this.DicTcpClient == null)
            {
                this.Init();
            }

            ModbusTcpNet busTcpClient = null;
            if (this.DicTcpClient != null && this.DicTcpClient.ContainsKey(PlcCode))
            {
                busTcpClient = this.DicTcpClient[PlcCode];
            }

            return busTcpClient;
        }

        /// <summary>
        /// 重连PLC
        /// </summary>
        public void ReConnection()
        {
            if (this.DicTcpClient != null)
            {
                if (true)
                {
                    string Key = "Inovance1";
                    ModbusTcpNet busTcpClient = null;
                    if (this.DicTcpClient.ContainsKey(Key))
                    {
                        busTcpClient = this.DicTcpClient[Key];
                    }
                    if (busTcpClient != null)
                    {
                        busTcpClient.ConnectClose();
                        this.DicTcpClient.Remove(Key);
                    }
                }


                if (true)
                {
                    string Key = "Inovance2";
                    ModbusTcpNet busTcpClient = null;
                    if (this.DicTcpClient.ContainsKey(Key))
                    {
                        busTcpClient = this.DicTcpClient[Key];
                    }
                    if (busTcpClient != null)
                    {
                        busTcpClient.ConnectClose();
                        this.DicTcpClient.Remove(Key);
                    }
                }
            }
            this.Init();
        }

        /// <summary>
        /// AGV有严重报警通知到机械手的PLC：1=AGV有严重报警，0=AGV无报警
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public HikDataResult Write_Palletizing_Warn_Serious(string PositionCode, short Value)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_AgvToRobot_Palletizing_Warn_Serious";
            
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            if (PlcCode == "Inovance1")
            {
                DomainContext.Interactive_One.DB_AgvToRobot_Palletizing_Warn_Serious = Value;
            }
            else if (PlcCode == "Inovance2")
            {
                DomainContext.Interactive_Two.DB_AgvToRobot_Palletizing_Warn_Serious = Value;
            }
            OperateResult OpResult = busTcpClient.Write(Address, Value);

            if (OpResult.IsSuccess)
            {
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "写入成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV有报警提示通知到机械手的PLC：1=AGV有报警，0=AGV无报警
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public HikDataResult Write_Palletizing_Warn_Tip(string PositionCode, short Value)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_AgvToRobot_Palletizing_Warn_Tip";
            
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            if (PlcCode == "Inovance1")
            {
                DomainContext.Interactive_One.DB_AgvToRobot_Palletizing_Warn_Tip = Value;
            }
            else if (PlcCode == "Inovance2")
            {
                DomainContext.Interactive_Two.DB_AgvToRobot_Palletizing_Warn_Tip = Value;
            }
            OperateResult OpResult = busTcpClient.Write(Address, Value);

            if (OpResult.IsSuccess)
            {
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "写入成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV往机械手PLC写入值，推荐机械手的码垛方向： 1=往A-1码垛(左边的料架) ； 2=往A-2码垛（右边的料架）
        /// </summary>
        public HikDataResult Write_Palletizing_Notify(string PositionCode,short Value)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_AgvToRobot_Palletizing_Notify";
            
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item=>item.PositionCode== PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags= BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }
            if (PlcCode == "Inovance1")
            {
                DomainContext.Interactive_One.DB_AgvToRobot_Palletizing_Notify = Value;
            }
            else if (PlcCode == "Inovance2")
            {
                DomainContext.Interactive_Two.DB_AgvToRobot_Palletizing_Notify = Value;
            }
            OperateResult OpResult = busTcpClient.Write(Address, Value);

            if (OpResult.IsSuccess)
            {
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "写入成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV往机械手PLC写入值，AGV进出左光栅：1=AGV进入左边的光栅 0=AGV离开左边的光栅
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public HikDataResult Write_Palletizing_AgvMove_LeftGrating(string PositionCode, short Value)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_AgvToRobot_Palletizing_AgvMove_LeftGrating";
            
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }
            if (PlcCode == "Inovance1")
            {
                DomainContext.Interactive_One.DB_AgvToRobot_Palletizing_AgvMove_LeftGrating = Value;
            }
            else if (PlcCode == "Inovance2")
            {
                DomainContext.Interactive_Two.DB_AgvToRobot_Palletizing_AgvMove_LeftGrating = Value;
            }
            OperateResult OpResult = busTcpClient.Write(Address, Value);

            if (OpResult.IsSuccess)
            {
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "写入成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV往机械手PLC写入值，AGV进出右边光栅：1=AGV进入右边的光栅 0=AGV离开右边的光栅
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public HikDataResult Write_Palletizing_AgvMove_RightGrating(string PositionCode, short Value)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_AgvToRobot_Palletizing_AgvMove_RightGrating";
            
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }
            if (PlcCode == "Inovance1")
            {
                DomainContext.Interactive_One.DB_AgvToRobot_Palletizing_AgvMove_RightGrating = Value;
            }
            else if (PlcCode == "Inovance2")
            {
                DomainContext.Interactive_Two.DB_AgvToRobot_Palletizing_AgvMove_RightGrating = Value;
            }
            OperateResult OpResult = busTcpClient.Write(Address, Value);

            if (OpResult.IsSuccess)
            {
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "写入成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }




        /// <summary>
        /// AGV主动读取机械手的码垛状态： 1 机械手正在往 A-1 码垛(坐标的料架) ； 2 机械手正在往 A-2 码垛(右边的料架)
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_Notify(string PositionCode,Action action=null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_Notify";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);

            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Notify != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Notify = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Notify != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Notify = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV主动读取机械手的左料架码垛数量
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_Left_Num(string PositionCode, Action action=null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_Left_Num";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);
            
            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV主动读取机械手的左料架可以码垛的总数量
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_Left_TotalNum(string PositionCode, Action action = null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_Left_TotalNum";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);

            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV主动读取机械手的右料架码垛数量
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_Right_Num(string PositionCode, Action action = null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_Right_Num";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);

            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV主动读取机械手的右料架可以码垛的总数量
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_Right_TotalNum(string PositionCode, Action action = null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_Right_TotalNum";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);

            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }


        /// <summary>
        /// AGV主动读取机械手左光栅的状态
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_LeftGrating_Status(string PositionCode, Action action = null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_LeftGrating_Status";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);

            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_LeftGrating_Status != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_LeftGrating_Status = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_LeftGrating_Status != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_LeftGrating_Status = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// AGV主动读取机械手右光栅的状态
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HikDataResult Read_Palletizing_RightGrating_Status(string PositionCode, Action action = null)
        {
            HikDataResult dataResult = new HikDataResult();

            string ItemID = "DB_RobotToAgv_Palletizing_RightGrating_Status";


            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionCode == PositionCode);

            if (Position == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "对应的坐标位置不存在";
                return dataResult;
            }

            string PlcCode = Position.PlcCode;

            List<GroupItemModel> ListTags = BlockServer.Items;
            ListTags = ListTags.IsNull() ? new List<GroupItemModel>() : ListTags;

            GroupItemModel TagEntity = ListTags.Where(item => item.ChannelName == this.ChannelName && item.DeviceName == PlcCode && item.ItemID == ItemID).FirstOrDefault();

            if (TagEntity == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "系统还未配置相应的点位数据";
                return dataResult;
            }

            string Address = TagEntity.DBBlock;

            ModbusTcpNet busTcpClient = this.GetBusTcpClient(PlcCode);
            if (busTcpClient == null)
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = "Modbus未能够连接到服务端";
                return dataResult;
            }

            OperateResult<short> OpResult = busTcpClient.ReadInt16(Address);

            if (OpResult.IsSuccess)
            {
                if (PlcCode == "Inovance1")
                {
                    if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_RightGrating_Status != OpResult.Content)
                    {
                        DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_RightGrating_Status = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                else if (PlcCode == "Inovance2")
                {
                    if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_RightGrating_Status != OpResult.Content)
                    {
                        DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_RightGrating_Status = OpResult.Content;
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
                dataResult.code = (int)EHikResponseCode.Success;
                dataResult.message = "读取成功";
            }
            else
            {
                dataResult.code = (int)EHikResponseCode.Exception;
                dataResult.message = OpResult.Message;
            }
            return dataResult;
        }
    }
}
