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

using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Io;
using Git.Mes.HikRobot.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.HikAgv.App.Server
{
    /// <summary>
    /// 加载托盘信息
    /// </summary>
    public partial class LoadPalletServer
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public LoadPalletServer() { }

        private string FileName = "Pallet.txt";


        /// <summary>
        /// 加载料架配置
        /// </summary>
        public void Load()
        {
            string FilePath = FileManager.GetDomainRoot() + "\\MapConfig\\" + FileName;

            StreamReader Sr = new StreamReader(FilePath);
            string Content = Sr.ReadToEnd();
            Sr.Close();

            DomainContext.ListPallet = HikJsonHelper.DeserializeObject<List<PalletEntity>>(Content);
        }

        /// <summary>
        /// 保存料架数据信息
        /// </summary>
        public void Save()
        {
            string FilePath = FileManager.GetDomainRoot() + "\\MapConfig\\" + FileName;

            string Content = "";

            if (DomainContext.ListMap.IsNullOrEmpty())
            {
                Content = HikJsonHelper.SerializeObject(DomainContext.ListPallet);
            }

            StreamWriter Sw = new StreamWriter(FilePath);
            Sw.Write(Content);
            Sw.Flush();
            Sw.Close();
        }
    }
}
