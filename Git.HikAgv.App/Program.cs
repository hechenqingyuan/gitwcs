using Git.Framework.Resource;
using Git.HikAgv.App.OPCModel;
using Git.HikAgv.App.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Git.HikAgv.App
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ResourceManager.LoadCache();

            BlockServer Server = new BlockServer();
            Server.Load();

            LoadMapServer MapServer = new LoadMapServer();
            MapServer.Load();

            LoadPalletServer PalletServer = new LoadPalletServer();
            PalletServer.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
