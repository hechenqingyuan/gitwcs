using DSkin.Controls;
using DSkin.Forms;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Log;
using Git.Framework.Resource;
using Git.HikAgv.App.Server;
using Git.Mes.HikRobot.SDK;
using Git.Mes.HikRobot.SDK.ApiName;
using Git.Mes.HikRobot.SDK.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Git.HikAgv.App
{
    public partial class Form1 : DSkinForm
    {
        private InovanceModbusClient ModbusClient = null;
        private Log log = Log.Instance(typeof(Form1));

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Start();

            ModbusClient = new InovanceModbusClient();
            ModbusClient.Init();
            DomainContext.ModbusClient = this.ModbusClient;

            this.tmPLC.Enabled = true;
            this.tmAgv.Enabled = true;
            this.tmAgvTask.Enabled = true;
            this.tmUpdate.Enabled = true;

            //重连PLC
            this.tmReConnection.Interval = 1000 * 60 * 15;
            this.tmReConnection.Enabled = true;

            this.InitGrid();
            this.SetTable();
        }

        /// <summary>
        /// 开启API服务
        /// </summary>
        public void Start()
        {
            try
            {
                string baseAddress = ResourceManager.GetSettingEntity("API_URL").Value;
                Microsoft.Owin.Hosting.WebApp.Start<Startup>(baseAddress);
                log.Info("API程序已启动,按任意键退出");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                log.Error("启动API服务异常:"+e.Message);
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 初始化料架位的表格信息
        /// </summary>
        public void InitGrid()
        {
            DSkinGridListColumn ColName = null;

            ColName = new DSkinGridListColumn();
            ColName.Item.Font = new System.Drawing.Font("微软雅黑", 9F);
            ColName.Item.ForeColor = System.Drawing.Color.Black;
            ColName.Item.InheritanceSize = new System.Drawing.SizeF(0F, 1F);
            ColName.Item.Location = new System.Drawing.Point(540, 0);
            ColName.Item.Name = "PositionName";
            ColName.Item.Size = new System.Drawing.Size(110, 30);
            //ColName.Item.Tag = "PositionName";
            ColName.Item.Text = "标号";
            ColName.Item.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ColName.Name = "标号";
            ColName.Visble = false;
            ColName.Width = 20;
            this.dvGrid.Columns.Add(ColName);

            ColName = new DSkinGridListColumn();
            ColName.Item.Font = new System.Drawing.Font("微软雅黑", 9F);
            ColName.Item.ForeColor = System.Drawing.Color.Black;
            ColName.Item.InheritanceSize = new System.Drawing.SizeF(0F, 1F);
            ColName.Item.Location = new System.Drawing.Point(540, 0);
            ColName.Item.Name = "Description";
            ColName.Item.Size = new System.Drawing.Size(110, 30);
            //ColName.Item.Tag = "Description";
            ColName.Item.Text = "料架位";
            ColName.Item.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ColName.Name = "料架位";
            ColName.Visble = true;
            ColName.Width = 80;
            this.dvGrid.Columns.Add(ColName);

            ColName = new DSkinGridListColumn();
            ColName.Item.Font = new System.Drawing.Font("微软雅黑", 9F);
            ColName.Item.ForeColor = System.Drawing.Color.Black;
            ColName.Item.InheritanceSize = new System.Drawing.SizeF(0F, 1F);
            ColName.Item.Location = new System.Drawing.Point(540, 0);
            ColName.Item.Name = "PositionCode";
            ColName.Item.Size = new System.Drawing.Size(110, 30);
            //ColName.Item.Tag = "PositionCode";
            ColName.Item.Text = "地图坐标";
            ColName.Item.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ColName.Name = "地图坐标";
            ColName.Visble = true;
            ColName.Width = 120;
            this.dvGrid.Columns.Add(ColName);

            ColName = new DSkinGridListColumn();
            ColName.Item.Font = new System.Drawing.Font("微软雅黑", 9F);
            ColName.Item.ForeColor = System.Drawing.Color.Black;
            ColName.Item.InheritanceSize = new System.Drawing.SizeF(0F, 1F);
            ColName.Item.Location = new System.Drawing.Point(540, 0);
            ColName.Item.Name = "PodCode";
            ColName.Item.Size = new System.Drawing.Size(110, 30);
            //ColName.Item.Tag = "PodCode";
            ColName.Item.Text = "货架号";
            ColName.Item.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ColName.Name = "货架号";
            ColName.Visble = true;
            ColName.Width = 100;
            this.dvGrid.Columns.Add(ColName);

            ColName = new DSkinGridListColumn();
            ColName.Item.Font = new System.Drawing.Font("微软雅黑", 9F);
            ColName.Item.ForeColor = System.Drawing.Color.Black;
            ColName.Item.InheritanceSize = new System.Drawing.SizeF(0F, 1F);
            ColName.Item.Location = new System.Drawing.Point(540, 0);
            ColName.Item.Name = "TotalNum";
            ColName.Item.Size = new System.Drawing.Size(110, 30);
            //ColName.Item.Tag = "TotalNum";
            ColName.Item.Text = "可码垛数";
            ColName.Item.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ColName.Name = "可码垛数";
            ColName.Visble = true;
            ColName.Width = 80;
            this.dvGrid.Columns.Add(ColName);

            ColName = new DSkinGridListColumn();
            ColName.Item.Font = new System.Drawing.Font("微软雅黑", 9F);
            ColName.Item.ForeColor = System.Drawing.Color.Black;
            ColName.Item.InheritanceSize = new System.Drawing.SizeF(0F, 1F);
            ColName.Item.Location = new System.Drawing.Point(540, 0);
            ColName.Item.Name = "Num";
            ColName.Item.Size = new System.Drawing.Size(110, 30);
            //ColName.Item.Tag = "Num";
            ColName.Item.Text = "已码垛数";
            ColName.Item.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ColName.Name = "已码垛数";
            ColName.Visble = true;
            ColName.Width = 80;
            this.dvGrid.Columns.Add(ColName);
        }

        /// <summary>
        /// 设置表格显示
        /// </summary>
        public void SetTable()
        {
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;
            this.dvGrid.Rows.Clear();
            foreach (PositionEntity BinModel in ListMap)
            {
                this.dvGrid.Rows.AddRow(BinModel.PositionName, BinModel.Description,BinModel.PositionCode, BinModel.PodCode,BinModel.TotalNum,BinModel.Num);
            }
        }

        /// <summary>
        /// 码垛变更机械手的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPalletizing_Click(object sender, EventArgs e)
        {
            DSkinButton Btn = sender as DSkinButton;
            string Tag = Btn.Tag as string;

            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;
            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionName == Tag);

            if (Position == null)
            {
                DSkinMessageBox.Show("地图配置文件中未配置该坐标数据");
                return;
            }

            PositionEntity StartBin = null;
            PositionEntity EndBin = null;

            StartBin = ListMap.Where(item => item.PositionName == Tag).FirstOrDefault();
            EndBin = ListMap.Where(item => item.PositionName == "Apply_A-2").FirstOrDefault();

            if (StartBin != null && EndBin != null)
            {
                GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
                StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StartModel.clientCode = "WMS";
                StartModel.taskTyp = "T01";
                StartModel.positionCodePath = new List<PositionDTO>();
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = StartBin.PositionCode, type = "00" });
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });

                log.Info("料架搬运：" + HikJsonHelper.SerializeObject(StartModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
                string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
                log.Info("料架搬运:" + Content);
            }

            HikDataResult dataResult = null;

            if (Tag == "A-1" || Tag == "C-1")
            {
                dataResult=this.ModbusClient.Write_Palletizing_Notify(Position.PositionCode, 1);
            }
            else if (Tag == "A-2" || Tag == "C-2")
            {
                dataResult=this.ModbusClient.Write_Palletizing_Notify(Position.PositionCode, 2);
            }
            else
            {
                DSkinMessageBox.Show("点击的按钮无法触发相关功能");
            }
            if (dataResult.code == (int)EHikResponseCode.Exception)
            {
                DSkinMessageBox.Show(dataResult.message);
            }

            
        }

        /// <summary>
        /// 定时读取PLC数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmPLC_Tick(object sender, EventArgs e)
        {
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            //读取第一条生产线点位状态
            if (true)
            {
                string Tag = "A-1";
                PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionName == Tag);

                HikDataResult NewData = null;
                NewData = this.ModbusClient.Read_Palletizing_Notify(Position.PositionCode);

                NewData = this.ModbusClient.Read_Palletizing_Left_Num(Position.PositionCode);
                NewData = this.ModbusClient.Read_Palletizing_Left_TotalNum(Position.PositionCode);

                NewData = this.ModbusClient.Read_Palletizing_Right_Num(Position.PositionCode);
                NewData = this.ModbusClient.Read_Palletizing_Right_TotalNum(Position.PositionCode);

                NewData = this.ModbusClient.Read_Palletizing_LeftGrating_Status(Position.PositionCode);
                NewData = this.ModbusClient.Read_Palletizing_RightGrating_Status(Position.PositionCode);

                if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num == 0)
                {
                    DomainContext.Interactive_One.LeftSnNum = string.Empty;
                    DomainContext.Interactive_One.LeftUpdateTime = DateTime.MinValue;
                }
                else
                {
                    if (DomainContext.Interactive_One.LeftSnNum.IsEmpty())
                    {
                        if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num > 0)
                        {
                            DomainContext.Interactive_One.LeftSnNum = ConvertHelper.NewGuid().SubString(25);
                        }
                    }
                    else
                    {
                        if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_Num == DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Left_TotalNum)
                        {
                            DomainContext.Interactive_One.LeftUpdateTime = DateTime.Now;
                        }
                    }
                }

                if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num == 0)
                {
                    DomainContext.Interactive_One.RightSnNum = string.Empty;
                    DomainContext.Interactive_One.RightUpdateTime = DateTime.MinValue;
                }
                else
                {
                    if (DomainContext.Interactive_One.RightSnNum.IsEmpty())
                    {
                        if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num > 0)
                        {
                            DomainContext.Interactive_One.RightSnNum = ConvertHelper.NewGuid().SubString(25);
                        }
                    }
                    else
                    {
                        if (DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_Num == DomainContext.Interactive_One.DB_RobotToAgv_Palletizing_Right_TotalNum)
                        {
                            DomainContext.Interactive_One.RightUpdateTime = DateTime.Now;
                        }
                    }
                }

                if (NewData.code != (int)EHikResponseCode.Success)
                {
                    log.Info("自动读取异常:"+NewData.message);
                }
            }

            //读取第二条生产线点位状态
            if (true)
            {
                string Tag = "C-1";
                PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionName == Tag);

                HikDataResult NewData = null;
                NewData = this.ModbusClient.Read_Palletizing_Notify(Position.PositionCode);

                NewData = this.ModbusClient.Read_Palletizing_Left_Num(Position.PositionCode);
                NewData = this.ModbusClient.Read_Palletizing_Left_TotalNum(Position.PositionCode);

                NewData = this.ModbusClient.Read_Palletizing_Right_Num(Position.PositionCode);
                NewData = this.ModbusClient.Read_Palletizing_Right_TotalNum(Position.PositionCode);

                NewData = this.ModbusClient.Read_Palletizing_LeftGrating_Status(Position.PositionCode);
                NewData = this.ModbusClient.Read_Palletizing_RightGrating_Status(Position.PositionCode);

                if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num == 0)
                {
                    DomainContext.Interactive_Two.LeftSnNum = string.Empty;
                    DomainContext.Interactive_Two.LeftUpdateTime = DateTime.MinValue;
                }
                else
                {
                    if (DomainContext.Interactive_Two.LeftSnNum.IsEmpty())
                    {
                        if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num > 0)
                        {
                            DomainContext.Interactive_Two.LeftSnNum = ConvertHelper.NewGuid().SubString(25);
                        }
                    }
                    else
                    {
                        if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_Num == DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Left_TotalNum)
                        {
                            DomainContext.Interactive_Two.LeftUpdateTime = DateTime.Now;
                        }
                    }
                }

                if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num == 0)
                {
                    DomainContext.Interactive_Two.RightSnNum = string.Empty;
                    DomainContext.Interactive_Two.RightUpdateTime = DateTime.MinValue;
                }
                else
                {
                    if (DomainContext.Interactive_Two.RightSnNum.IsEmpty())
                    {
                        if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num > 0)
                        {
                            DomainContext.Interactive_Two.RightSnNum = ConvertHelper.NewGuid().SubString(25);
                        }
                    }
                    else
                    {
                        if (DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_Num == DomainContext.Interactive_Two.DB_RobotToAgv_Palletizing_Right_TotalNum)
                        {
                            DomainContext.Interactive_Two.RightUpdateTime = DateTime.Now;
                        }
                    }
                }

                if (NewData.code != (int)EHikResponseCode.Success)
                {
                    log.Info("自动读取异常:" + NewData.message);
                }
            }

            //log.Info("自动读取PLC的数据Interactive_One:=>" + HikJsonHelper.SerializeObject(DomainContext.Interactive_One));
            //log.Info("自动读取PLC的数据Interactive_Two:=>" + HikJsonHelper.SerializeObject(DomainContext.Interactive_Two));


            LoadMapServer MapServer = new LoadMapServer();
            MapServer.UpdateBin();
        }

        /// <summary>
        /// 定时读取AGV的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmAgv_Tick(object sender, EventArgs e)
        {
            //LoadMapServer MapServer = new LoadMapServer();
            //MapServer.QueryAgv();
        }

        /// <summary>
        /// 定时发起AGV的搬运任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmAgvTask_Tick(object sender, EventArgs e)
        {
            //LoadMapServer MapServer = new LoadMapServer();
            //MapServer.StartTask();
        }

        /// <summary>
        /// 申请单交互
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            //坐标转换
            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;

            if (true)
            {
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-2");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_RightGrating(EndBin.PositionCode, 1);
                }
            }
            if (true)
            {
                PositionEntity EndBin = ListMap.FirstOrDefault(item => item.PositionName == "A-1");
                InovanceModbusClient ModbusClient = DomainContext.ModbusClient;
                if (EndBin != null)
                {
                    ModbusClient.Write_Palletizing_AgvMove_LeftGrating(EndBin.PositionCode, 1);
                }
            }
        }

        /// <summary>
        /// 搬运空货架
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveEmpty_Click(object sender, EventArgs e)
        {
            DSkinButton Btn = sender as DSkinButton;
            string Tag = Btn.Tag as string;

            List<PositionEntity> ListMap = DomainContext.ListMap;
            ListMap = ListMap.IsNull() ? new List<PositionEntity>() : ListMap;
            PositionEntity Position = ListMap.FirstOrDefault(item => item.PositionName == Tag);

            if (Position == null)
            {
                DSkinMessageBox.Show("地图配置文件中未配置该坐标数据");
                return;
            }

            PositionEntity StartBin = null;
            PositionEntity EndBin = null;

            StartBin = ListMap.Where(item => item.PositionName==Tag).FirstOrDefault();
            EndBin = ListMap.Where(item => item.PositionName == "Apply_A-1").FirstOrDefault();

            if (StartBin != null && EndBin != null)
            {
                GenTaskRequestDTO StartModel = new GenTaskRequestDTO();
                StartModel.reqCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StartModel.clientCode = "WMS";
                if (StartBin.PositionName == "B-1" || StartBin.PositionName == "B-2")
                {
                    StartModel.taskTyp = "T01";
                }
                else if (StartBin.PositionName == "D-1" || StartBin.PositionName == "D-2")
                {
                    StartModel.taskTyp = "T02";
                }
                StartModel.taskCode = ConvertHelper.NewGuid().SubString(25);
                StartModel.positionCodePath = new List<PositionDTO>();
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = StartBin.PositionCode, type = "00" });
                StartModel.positionCodePath.Add(new PositionDTO() { positionCode = EndBin.PositionCode, type = "00" });

                AgvTaskEntity TaskModel = new AgvTaskEntity();
                TaskModel.reqCode = StartModel.reqCode;
                TaskModel.currentPositionCode = "";
                TaskModel.podCode = "";
                TaskModel.method = "";
                TaskModel.robotCode = "5763";
                TaskModel.taskCode = StartModel.taskCode;
                TaskModel.TaskType = StartModel.taskTyp;
                if (StartBin != null)
                {
                    TaskModel.StartPositionCode = StartBin.PositionCode;
                    TaskModel.StartPositionName = StartBin.PositionName;
                }
                if (EndBin != null)
                {
                    TaskModel.EndPositionCode = EndBin.PositionCode;
                    TaskModel.EndPositionName = EndBin.PositionName;
                }
                DomainContext.AgvTask = TaskModel;

                log.Info("人工软件触发按钮搬运空货架到码垛区：" + HikJsonHelper.SerializeObject(StartModel));
                IHikTopClient client = new HikTopClientDefault();
                JObject param = JObject.Parse(HikJsonHelper.SerializeObject(StartModel));
                string Content = client.Execute(TaskApiName.TaskApiName_genAgvSchedulingTask, param);
                log.Info("人工软件触发按钮搬运空货架到码垛区请求结果:" + Content);
            }
        }

        /// <summary>
        /// 自动更新界面显示的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmUpdate_Tick(object sender, EventArgs e)
        {
            this.SetTable();
        }

        /// <summary>
        /// 重连PLC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmReConnection_Tick(object sender, EventArgs e)
        {
            DomainContext.ModbusClient.ReConnection();
        }
    }
}
