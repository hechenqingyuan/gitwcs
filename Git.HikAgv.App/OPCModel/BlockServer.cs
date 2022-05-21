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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.HikAgv.App.OPCModel
{
    public partial class BlockServer
    {
        public BlockServer() { }

        private Log log = Log.Instance(typeof(BlockServer));

        /// <summary>
        /// 服务通道集合
        /// </summary>
        public static List<ServerModel> ListServer;

        /// <summary>
        /// 设备分组管理
        /// </summary>
        public static List<GroupModel> Groups;

        /// <summary>
        /// 设备分组参数项集合管理
        /// </summary>
        public static List<GroupItemModel> Items;

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public void Load()
        {
            try
            {
                string FilePath = FileManager.GetDomainRoot() + "\\PlcConfig\\";
                List<FileItem> listFiles = FileManager.GetDirectoryItems(FilePath);
                if (listFiles.IsNullOrEmpty())
                {
                    return;
                }
                if (BlockServer.ListServer.IsNull())
                {
                    BlockServer.ListServer = new List<ServerModel>();
                }
                BlockServer.ListServer.Clear();

                foreach (FileItem file in listFiles)
                {
                    ServerModel ChannelEntity = ReadServer(file.Name, file);
                    if (ChannelEntity != null)
                    {
                        List<DeviceModel> listDevice = ReadDevice(file);
                        if (!listDevice.IsNullOrEmpty())
                        {
                            ChannelEntity.Devices = listDevice;
                        }
                        BlockServer.ListServer.Add(ChannelEntity);
                    }
                }

                BlockServer.Items = BlockServer.Items.IsNull() ? new List<GroupItemModel>() : BlockServer.Items;
                BlockServer.Groups = BlockServer.Groups.IsNull() ? new List<GroupModel>() : BlockServer.Groups;

                foreach (ServerModel ChannelEntity in BlockServer.ListServer)
                {
                    foreach (DeviceModel DevModel in ChannelEntity.Devices)
                    {
                        foreach (GroupModel Group in DevModel.ListGroups)
                        {
                            foreach (GroupItemModel GroupItem in Group.Items)
                            {
                                GroupItem.GroupName = Group.DeviceName;
                                GroupItem.DeviceName = DevModel.CpuName;
                                GroupItem.ChannelName = ChannelEntity.ChannelName;
                                BlockServer.Items.Add(GroupItem);
                            }
                            BlockServer.Groups.Add(Group);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("读取配置文件异常:"+e.Message);
            }
            
        }

        

        /// <summary>
        /// 读取服务通道的数据配置
        /// </summary>
        public ServerModel ReadServer(string ChannelName, FileItem fileItem)
        {
            string FileName = fileItem.FullName + "\\Server.txt";

            List<string> list = new List<string>();
            StreamReader SR = new StreamReader(FileName);
            string Line = string.Empty;
            Line = SR.ReadLine();
            while (!string.IsNullOrEmpty(Line))
            {
                list.Add(Line);
                Line = SR.ReadLine();
            }
            SR.Close();
            if (list.Count != 3)
            {
                throw new Exception("服务配置文件错误");
            }

            ServerModel Model = new ServerModel();
            Model.ChannelName = ChannelName;
            foreach (string Str in list)
            {
                string[] items = Str.Split('=');
                if (items[0] == "ServerIP")
                {
                    Model.ServerIP = items[1];
                }
                else if (items[0] == "ProgID")
                {
                    Model.ProgID = items[1];
                }
                else if (items[0] == "UpdateRate")
                {
                    Model.UpdateRate = ConvertHelper.ToType<int>(items[1], 1000);
                }
            }
            return Model;
        }

        /// <summary>
        /// 获取服务通道下面的设备信息-对应CPU的信息
        /// </summary>
        /// <param name="fileItem"></param>
        /// <returns></returns>
        public List<DeviceModel> ReadDevice(FileItem fileItem)
        {
            List<FileItem> listFiles = FileManager.GetDirectoryItems(fileItem.FullName);
            if (listFiles.IsNullOrEmpty())
            {
                return null;
            }
            List<DeviceModel> listDevice = new List<DeviceModel>();
            foreach (FileItem file in listFiles)
            {
                DeviceModel Model = new DeviceModel();
                Model.CpuName = file.Name;
                List<GroupModel> listGroup = ReadGroups(file);
                Model.ListGroups = listGroup;
                listDevice.Add(Model);
            }
            return listDevice;
        }

        /// <summary>
        /// 读取设备分组的内容
        /// </summary>
        /// <param name="fileItem"></param>
        /// <returns></returns>
        public List<GroupModel> ReadGroups(FileItem fileItem)
        {
            List<FileItem> listFiles = FileManager.GetFileItems(fileItem.FullName);
            if (listFiles.IsNullOrEmpty())
            {
                return null;
            }
            List<GroupModel> listGroup = new List<GroupModel>();
            foreach (FileItem item in listFiles)
            {
                string fileName = item.Name;
                GroupModel Model = new GroupModel();
                Model.DeviceName = fileName.SubStr(0,fileName.IndexOf("."));
                Model.Items = ReadItems(item.FullName);
                listGroup.Add(Model);
            }
            return listGroup;
        }

        /// <summary>
        /// 读取配置的参数信息
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public List<GroupItemModel> ReadItems(string FileName)
        {
            List<string> list = new List<string>();
            StreamReader SR = new StreamReader(FileName);
            string Line = string.Empty;
            Line = SR.ReadLine();
            while (!string.IsNullOrEmpty(Line))
            {
                list.Add(Line);
                Line = SR.ReadLine();
            }
            SR.Close();

            List < GroupItemModel >  listSource=new List<GroupItemModel>();
            GroupItemModel ItemModel = new GroupItemModel();
            for (int i = 0; i < list.Count; i++)
            {
                string Str = list[i];
                string[] items = Str.Split('=');
                if (i % 5 == 0)
                {
                    if (ItemModel == null)
                    {
                        ItemModel = new GroupItemModel();
                    }
                    ItemModel.ItemID = items[1];
                }
                else if (i % 5 == 1)
                {
                    ItemModel.DBBlock = items[1];
                }
                else if (i % 5 == 2)
                {
                    ItemModel.DataType = items[1];
                }
                else if (i % 5 == 3)
                {
                    ItemModel.PropName = items[1];
                }
                else if (i % 5 == 4)
                {
                    listSource.Add(ItemModel);
                    ItemModel = null;
                }
            }
            return listSource;
        }
    }
}
