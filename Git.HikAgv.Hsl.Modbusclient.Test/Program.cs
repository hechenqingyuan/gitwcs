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

using HslCommunication;
using HslCommunication.ModBus;
using Newtonsoft.Json;
using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Git.HikAgv.Hsl.Modbusclient.Test
{
    class Program
    {
        private static HslCommunication.ModBus.ModbusTcpServer busTcpServer = null;

        private static ModbusTcpNet busTcpClient = null;

        static void Main(string[] args)
        {
            //using (TcpClient client = new TcpClient("127.0.0.1", 502))
            //{
            //    var factory = new ModbusFactory();
            //    IModbusMaster master = factory.CreateMaster(client);

            //    ushort startAddress = 0;
            //    ushort numInputs = 5;

            //    //for (int i = 0; i < 20; i++)
            //    //{
            //    //    ushort[] registers = master.ReadHoldingRegisters(1, startAddress, numInputs);

            //    //    Console.WriteLine(JsonConvert.SerializeObject(registers));

            //    //    Thread.Sleep(800);
            //    //}

            //    master.WriteSingleRegister(1,startAddress,2455);
            //}

            busTcpClient = new ModbusTcpNet("127.0.0.1", 502, 1);
            busTcpClient.AddressStartWithZero = true;
            OperateResult connect = busTcpClient.ConnectServer();
            if (connect.IsSuccess)
            {

                //for (int i = 1; i < 20; i++)
                //{
                //    short Result = busTcpClient.ReadInt16("1").Content;

                //    Console.WriteLine(Result);

                //    Thread.Sleep(800);
                //}

                //for (int i = 1; i < 10; i++)
                //{
                //    Random random = new Random(DateTime.Now.Millisecond);
                //    int Index=random.Next(1, 10);

                //    Random randomResult = new Random(DateTime.Now.Millisecond);
                //    int Value = randomResult.Next(100, 2000);
                //    Value = 2;
                //    Console.WriteLine(Index + "==>" + Value);
                //    busTcpClient.Write(i.ToString(),Value);

                //    Thread.Sleep(1500);
                //}

                OperateResult OpResult= busTcpClient.Write("450", "AFFFFFFFFFFF");// 写入线圈100为通
            }

            Console.ReadLine();
        }

        public static void StartServer()
        {
            busTcpServer = new HslCommunication.ModBus.ModbusTcpServer();
            busTcpServer.OnDataReceived += BusTcpServer_OnDataReceived;
            busTcpServer.ServerStart(502);

            short Result = busTcpServer.ReadInt16("1").Content;
            Console.WriteLine(JsonConvert.SerializeObject(Result));

            Console.ReadLine();
        }

        private static void BusTcpServer_OnDataReceived(object sender, byte[] data)
        {
            short Result = busTcpServer.ReadInt16("1").Content;
            Console.WriteLine(JsonConvert.SerializeObject(Result));
        }
    }
}
