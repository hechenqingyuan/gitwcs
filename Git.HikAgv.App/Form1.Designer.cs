namespace Git.HikAgv.App
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dSkinPanel1 = new DSkin.Controls.DSkinPanel();
            this.btnApply_A_2 = new DSkin.Controls.DSkinButton();
            this.btnApply_A_1 = new DSkin.Controls.DSkinButton();
            this.btnApply_A_3 = new DSkin.Controls.DSkinButton();
            this.btnC_1 = new DSkin.Controls.DSkinButton();
            this.btnC_2 = new DSkin.Controls.DSkinButton();
            this.btnA_2 = new DSkin.Controls.DSkinButton();
            this.btnA_1 = new DSkin.Controls.DSkinButton();
            this.btnB_1 = new DSkin.Controls.DSkinButton();
            this.btnB_2 = new DSkin.Controls.DSkinButton();
            this.btnD_1 = new DSkin.Controls.DSkinButton();
            this.btnD_2 = new DSkin.Controls.DSkinButton();
            this.tmPLC = new System.Windows.Forms.Timer(this.components);
            this.tmAgv = new System.Windows.Forms.Timer(this.components);
            this.tmAgvTask = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dvGrid = new DSkin.Controls.DSkinGridList();
            this.tmUpdate = new System.Windows.Forms.Timer(this.components);
            this.tmReConnection = new System.Windows.Forms.Timer(this.components);
            this.dSkinPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dSkinPanel1
            // 
            this.dSkinPanel1.BackColor = System.Drawing.Color.LightGray;
            this.dSkinPanel1.Controls.Add(this.btnApply_A_2);
            this.dSkinPanel1.Controls.Add(this.btnApply_A_1);
            this.dSkinPanel1.Controls.Add(this.btnApply_A_3);
            this.dSkinPanel1.Location = new System.Drawing.Point(16, 158);
            this.dSkinPanel1.Name = "dSkinPanel1";
            this.dSkinPanel1.RightBottom = ((System.Drawing.Image)(resources.GetObject("dSkinPanel1.RightBottom")));
            this.dSkinPanel1.Size = new System.Drawing.Size(1293, 169);
            this.dSkinPanel1.TabIndex = 0;
            this.dSkinPanel1.Text = "dSkinPanel1";
            // 
            // btnApply_A_2
            // 
            this.btnApply_A_2.ButtonBorderWidth = 1;
            this.btnApply_A_2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnApply_A_2.HoverColor = System.Drawing.Color.Empty;
            this.btnApply_A_2.HoverImage = null;
            this.btnApply_A_2.Location = new System.Drawing.Point(156, 58);
            this.btnApply_A_2.Name = "btnApply_A_2";
            this.btnApply_A_2.NormalImage = null;
            this.btnApply_A_2.PressColor = System.Drawing.Color.Empty;
            this.btnApply_A_2.PressedImage = null;
            this.btnApply_A_2.Radius = 0;
            this.btnApply_A_2.ShowButtonBorder = true;
            this.btnApply_A_2.Size = new System.Drawing.Size(78, 54);
            this.btnApply_A_2.TabIndex = 11;
            this.btnApply_A_2.Tag = "Apply_A-2";
            this.btnApply_A_2.Text = "P2暂停点";
            this.btnApply_A_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnApply_A_2.TextPadding = 0;
            this.btnApply_A_2.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnApply_A_1
            // 
            this.btnApply_A_1.ButtonBorderWidth = 1;
            this.btnApply_A_1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnApply_A_1.HoverColor = System.Drawing.Color.Empty;
            this.btnApply_A_1.HoverImage = null;
            this.btnApply_A_1.Location = new System.Drawing.Point(291, 58);
            this.btnApply_A_1.Name = "btnApply_A_1";
            this.btnApply_A_1.NormalImage = null;
            this.btnApply_A_1.PressColor = System.Drawing.Color.Empty;
            this.btnApply_A_1.PressedImage = null;
            this.btnApply_A_1.Radius = 0;
            this.btnApply_A_1.ShowButtonBorder = true;
            this.btnApply_A_1.Size = new System.Drawing.Size(78, 54);
            this.btnApply_A_1.TabIndex = 10;
            this.btnApply_A_1.Tag = "Apply_A-1";
            this.btnApply_A_1.Text = "P1暂停点";
            this.btnApply_A_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnApply_A_1.TextPadding = 0;
            this.btnApply_A_1.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnApply_A_3
            // 
            this.btnApply_A_3.ButtonBorderWidth = 1;
            this.btnApply_A_3.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnApply_A_3.HoverColor = System.Drawing.Color.Empty;
            this.btnApply_A_3.HoverImage = null;
            this.btnApply_A_3.Location = new System.Drawing.Point(12, 58);
            this.btnApply_A_3.Name = "btnApply_A_3";
            this.btnApply_A_3.NormalImage = null;
            this.btnApply_A_3.PressColor = System.Drawing.Color.Empty;
            this.btnApply_A_3.PressedImage = null;
            this.btnApply_A_3.Radius = 0;
            this.btnApply_A_3.ShowButtonBorder = true;
            this.btnApply_A_3.Size = new System.Drawing.Size(78, 54);
            this.btnApply_A_3.TabIndex = 9;
            this.btnApply_A_3.Tag = "Apply_A-3";
            this.btnApply_A_3.Text = "P3暂停点";
            this.btnApply_A_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnApply_A_3.TextPadding = 0;
            this.btnApply_A_3.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnC_1
            // 
            this.btnC_1.BackColor = System.Drawing.Color.LightGray;
            this.btnC_1.ButtonBorderWidth = 1;
            this.btnC_1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnC_1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnC_1.HoverColor = System.Drawing.Color.Empty;
            this.btnC_1.HoverImage = null;
            this.btnC_1.Location = new System.Drawing.Point(16, 343);
            this.btnC_1.Name = "btnC_1";
            this.btnC_1.NormalImage = null;
            this.btnC_1.PressColor = System.Drawing.Color.Empty;
            this.btnC_1.PressedImage = null;
            this.btnC_1.Radius = 10;
            this.btnC_1.ShowButtonBorder = true;
            this.btnC_1.Size = new System.Drawing.Size(100, 100);
            this.btnC_1.TabIndex = 1;
            this.btnC_1.Tag = "C-1";
            this.btnC_1.Text = "C-1";
            this.btnC_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnC_1.TextPadding = 0;
            this.btnC_1.Click += new System.EventHandler(this.btnPalletizing_Click);
            // 
            // btnC_2
            // 
            this.btnC_2.BackColor = System.Drawing.Color.LightGray;
            this.btnC_2.ButtonBorderWidth = 1;
            this.btnC_2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnC_2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnC_2.HoverColor = System.Drawing.Color.Empty;
            this.btnC_2.HoverImage = null;
            this.btnC_2.Location = new System.Drawing.Point(161, 343);
            this.btnC_2.Name = "btnC_2";
            this.btnC_2.NormalImage = null;
            this.btnC_2.PressColor = System.Drawing.Color.Empty;
            this.btnC_2.PressedImage = null;
            this.btnC_2.Radius = 10;
            this.btnC_2.ShowButtonBorder = true;
            this.btnC_2.Size = new System.Drawing.Size(100, 100);
            this.btnC_2.TabIndex = 2;
            this.btnC_2.Tag = "C-2";
            this.btnC_2.Text = "C-2";
            this.btnC_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnC_2.TextPadding = 0;
            this.btnC_2.Click += new System.EventHandler(this.btnPalletizing_Click);
            // 
            // btnA_2
            // 
            this.btnA_2.BackColor = System.Drawing.Color.LightGray;
            this.btnA_2.ButtonBorderWidth = 1;
            this.btnA_2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnA_2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnA_2.HoverColor = System.Drawing.Color.Empty;
            this.btnA_2.HoverImage = null;
            this.btnA_2.Location = new System.Drawing.Point(161, 42);
            this.btnA_2.Name = "btnA_2";
            this.btnA_2.NormalImage = null;
            this.btnA_2.PressColor = System.Drawing.Color.Empty;
            this.btnA_2.PressedImage = null;
            this.btnA_2.Radius = 10;
            this.btnA_2.ShowButtonBorder = true;
            this.btnA_2.Size = new System.Drawing.Size(100, 100);
            this.btnA_2.TabIndex = 4;
            this.btnA_2.Tag = "A-2";
            this.btnA_2.Text = "A-2";
            this.btnA_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnA_2.TextPadding = 0;
            this.btnA_2.Click += new System.EventHandler(this.btnPalletizing_Click);
            // 
            // btnA_1
            // 
            this.btnA_1.BackColor = System.Drawing.Color.LightGray;
            this.btnA_1.ButtonBorderWidth = 1;
            this.btnA_1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnA_1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnA_1.HoverColor = System.Drawing.Color.Empty;
            this.btnA_1.HoverImage = null;
            this.btnA_1.Location = new System.Drawing.Point(16, 42);
            this.btnA_1.Name = "btnA_1";
            this.btnA_1.NormalImage = null;
            this.btnA_1.PressColor = System.Drawing.Color.Empty;
            this.btnA_1.PressedImage = null;
            this.btnA_1.Radius = 10;
            this.btnA_1.ShowButtonBorder = true;
            this.btnA_1.Size = new System.Drawing.Size(100, 100);
            this.btnA_1.TabIndex = 3;
            this.btnA_1.Tag = "A-1";
            this.btnA_1.Text = "A-1";
            this.btnA_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnA_1.TextPadding = 0;
            this.btnA_1.Click += new System.EventHandler(this.btnPalletizing_Click);
            // 
            // btnB_1
            // 
            this.btnB_1.BackColor = System.Drawing.Color.LightGray;
            this.btnB_1.ButtonBorderWidth = 1;
            this.btnB_1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnB_1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnB_1.HoverColor = System.Drawing.Color.Empty;
            this.btnB_1.HoverImage = null;
            this.btnB_1.Location = new System.Drawing.Point(1209, 42);
            this.btnB_1.Name = "btnB_1";
            this.btnB_1.NormalImage = null;
            this.btnB_1.PressColor = System.Drawing.Color.Empty;
            this.btnB_1.PressedImage = null;
            this.btnB_1.Radius = 10;
            this.btnB_1.ShowButtonBorder = true;
            this.btnB_1.Size = new System.Drawing.Size(100, 100);
            this.btnB_1.TabIndex = 6;
            this.btnB_1.Tag = "B-1";
            this.btnB_1.Text = "B-1";
            this.btnB_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnB_1.TextPadding = 0;
            this.btnB_1.Click += new System.EventHandler(this.btnMoveEmpty_Click);
            // 
            // btnB_2
            // 
            this.btnB_2.BackColor = System.Drawing.Color.LightGray;
            this.btnB_2.ButtonBorderWidth = 1;
            this.btnB_2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnB_2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnB_2.HoverColor = System.Drawing.Color.Empty;
            this.btnB_2.HoverImage = null;
            this.btnB_2.Location = new System.Drawing.Point(1064, 42);
            this.btnB_2.Name = "btnB_2";
            this.btnB_2.NormalImage = null;
            this.btnB_2.PressColor = System.Drawing.Color.Empty;
            this.btnB_2.PressedImage = null;
            this.btnB_2.Radius = 10;
            this.btnB_2.ShowButtonBorder = true;
            this.btnB_2.Size = new System.Drawing.Size(100, 100);
            this.btnB_2.TabIndex = 5;
            this.btnB_2.Tag = "B-2";
            this.btnB_2.Text = "B-2";
            this.btnB_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnB_2.TextPadding = 0;
            this.btnB_2.Click += new System.EventHandler(this.btnMoveEmpty_Click);
            // 
            // btnD_1
            // 
            this.btnD_1.BackColor = System.Drawing.Color.LightGray;
            this.btnD_1.ButtonBorderWidth = 1;
            this.btnD_1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnD_1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnD_1.HoverColor = System.Drawing.Color.Empty;
            this.btnD_1.HoverImage = null;
            this.btnD_1.Location = new System.Drawing.Point(1209, 343);
            this.btnD_1.Name = "btnD_1";
            this.btnD_1.NormalImage = null;
            this.btnD_1.PressColor = System.Drawing.Color.Empty;
            this.btnD_1.PressedImage = null;
            this.btnD_1.Radius = 10;
            this.btnD_1.ShowButtonBorder = true;
            this.btnD_1.Size = new System.Drawing.Size(100, 100);
            this.btnD_1.TabIndex = 8;
            this.btnD_1.Tag = "D-1";
            this.btnD_1.Text = "D-1";
            this.btnD_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnD_1.TextPadding = 0;
            this.btnD_1.Click += new System.EventHandler(this.btnMoveEmpty_Click);
            // 
            // btnD_2
            // 
            this.btnD_2.BackColor = System.Drawing.Color.LightGray;
            this.btnD_2.ButtonBorderWidth = 1;
            this.btnD_2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnD_2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnD_2.HoverColor = System.Drawing.Color.Empty;
            this.btnD_2.HoverImage = null;
            this.btnD_2.Location = new System.Drawing.Point(1064, 343);
            this.btnD_2.Name = "btnD_2";
            this.btnD_2.NormalImage = null;
            this.btnD_2.PressColor = System.Drawing.Color.Empty;
            this.btnD_2.PressedImage = null;
            this.btnD_2.Radius = 10;
            this.btnD_2.ShowButtonBorder = true;
            this.btnD_2.Size = new System.Drawing.Size(100, 100);
            this.btnD_2.TabIndex = 7;
            this.btnD_2.Tag = "D-2";
            this.btnD_2.Text = "D-2";
            this.btnD_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnD_2.TextPadding = 0;
            this.btnD_2.Click += new System.EventHandler(this.btnMoveEmpty_Click);
            // 
            // tmPLC
            // 
            this.tmPLC.Enabled = true;
            this.tmPLC.Interval = 30000;
            this.tmPLC.Tag = "PLC数据定时读取";
            this.tmPLC.Tick += new System.EventHandler(this.tmPLC_Tick);
            // 
            // tmAgv
            // 
            this.tmAgv.Interval = 5000;
            this.tmAgv.Tag = "定时查询AGV的状态";
            this.tmAgv.Tick += new System.EventHandler(this.tmAgv_Tick);
            // 
            // tmAgvTask
            // 
            this.tmAgvTask.Interval = 30000;
            this.tmAgvTask.Tag = "AGV自动发起搬运任务";
            this.tmAgvTask.Tick += new System.EventHandler(this.tmAgvTask_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dvGrid);
            this.groupBox1.Location = new System.Drawing.Point(16, 461);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(520, 236);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工作位管理";
            // 
            // dvGrid
            // 
            // 
            // 
            // 
            this.dvGrid.BackPageButton.AdaptImage = true;
            this.dvGrid.BackPageButton.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.dvGrid.BackPageButton.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.BackPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dvGrid.BackPageButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.BackPageButton.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.BackPageButton.Location = new System.Drawing.Point(386, 4);
            this.dvGrid.BackPageButton.Name = "BtnBackPage";
            this.dvGrid.BackPageButton.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.dvGrid.BackPageButton.Radius = 0;
            this.dvGrid.BackPageButton.Size = new System.Drawing.Size(50, 24);
            this.dvGrid.BackPageButton.Text = "上一页";
            this.dvGrid.BackPageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dvGrid.BackPageButton.TextRenderMode = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.dvGrid.Borders.AllColor = System.Drawing.Color.Silver;
            this.dvGrid.Borders.BottomColor = System.Drawing.Color.Silver;
            this.dvGrid.Borders.LeftColor = System.Drawing.Color.Silver;
            this.dvGrid.Borders.RightColor = System.Drawing.Color.Silver;
            this.dvGrid.Borders.TopColor = System.Drawing.Color.Silver;
            this.dvGrid.ColumnHeight = 30;
            this.dvGrid.DoubleItemsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.dvGrid.EnabledOrder = false;
            this.dvGrid.EnablePage = false;
            // 
            // 
            // 
            this.dvGrid.FirstPageButton.AdaptImage = true;
            this.dvGrid.FirstPageButton.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.dvGrid.FirstPageButton.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.FirstPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dvGrid.FirstPageButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.FirstPageButton.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.FirstPageButton.Location = new System.Drawing.Point(338, 4);
            this.dvGrid.FirstPageButton.Name = "BtnFistPage";
            this.dvGrid.FirstPageButton.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.dvGrid.FirstPageButton.Radius = 0;
            this.dvGrid.FirstPageButton.Size = new System.Drawing.Size(44, 24);
            this.dvGrid.FirstPageButton.Text = "首页";
            this.dvGrid.FirstPageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dvGrid.FirstPageButton.TextRenderMode = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // 
            // 
            this.dvGrid.GoPageButton.AdaptImage = true;
            this.dvGrid.GoPageButton.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.dvGrid.GoPageButton.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dvGrid.GoPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dvGrid.GoPageButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.GoPageButton.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.GoPageButton.Location = new System.Drawing.Point(290, 4);
            this.dvGrid.GoPageButton.Name = "BtnGoPage";
            this.dvGrid.GoPageButton.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.dvGrid.GoPageButton.Radius = 0;
            this.dvGrid.GoPageButton.Size = new System.Drawing.Size(44, 24);
            this.dvGrid.GoPageButton.Text = "跳转";
            this.dvGrid.GoPageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dvGrid.GoPageButton.TextRenderMode = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.dvGrid.GridLineColor = System.Drawing.Color.Silver;
            this.dvGrid.HeaderFont = new System.Drawing.Font("微软雅黑", 9F);
            // 
            // 
            // 
            this.dvGrid.HScrollBar.AutoSize = false;
            this.dvGrid.HScrollBar.Fillet = true;
            this.dvGrid.HScrollBar.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.HScrollBar.Location = new System.Drawing.Point(0, 56);
            this.dvGrid.HScrollBar.Name = "";
            this.dvGrid.HScrollBar.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.dvGrid.HScrollBar.ScrollBarPartitionWidth = new System.Windows.Forms.Padding(5);
            this.dvGrid.HScrollBar.Size = new System.Drawing.Size(508, 12);
            this.dvGrid.HScrollBar.Visible = false;
            // 
            // 
            // 
            this.dvGrid.LastPageButton.AdaptImage = true;
            this.dvGrid.LastPageButton.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.dvGrid.LastPageButton.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.LastPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dvGrid.LastPageButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.LastPageButton.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.LastPageButton.Location = new System.Drawing.Point(494, 4);
            this.dvGrid.LastPageButton.Name = "BtnLastPage";
            this.dvGrid.LastPageButton.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.dvGrid.LastPageButton.Radius = 0;
            this.dvGrid.LastPageButton.Size = new System.Drawing.Size(44, 24);
            this.dvGrid.LastPageButton.Text = "末页";
            this.dvGrid.LastPageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dvGrid.LastPageButton.TextRenderMode = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.dvGrid.Location = new System.Drawing.Point(6, 20);
            this.dvGrid.Name = "dvGrid";
            // 
            // 
            // 
            this.dvGrid.NextPageButton.AdaptImage = true;
            this.dvGrid.NextPageButton.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.dvGrid.NextPageButton.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.NextPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dvGrid.NextPageButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.NextPageButton.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.dvGrid.NextPageButton.Location = new System.Drawing.Point(440, 4);
            this.dvGrid.NextPageButton.Name = "BtnNextPage";
            this.dvGrid.NextPageButton.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.dvGrid.NextPageButton.Radius = 0;
            this.dvGrid.NextPageButton.Size = new System.Drawing.Size(50, 24);
            this.dvGrid.NextPageButton.Text = "下一页";
            this.dvGrid.NextPageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dvGrid.NextPageButton.TextRenderMode = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.dvGrid.SelectedItem = null;
            this.dvGrid.Size = new System.Drawing.Size(508, 210);
            this.dvGrid.TabIndex = 0;
            // 
            // 
            // 
            this.dvGrid.VScrollBar.AutoSize = false;
            this.dvGrid.VScrollBar.BitmapCache = true;
            this.dvGrid.VScrollBar.Fillet = true;
            this.dvGrid.VScrollBar.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dvGrid.VScrollBar.LargeChange = 1000;
            this.dvGrid.VScrollBar.Location = new System.Drawing.Point(495, 1);
            this.dvGrid.VScrollBar.Margin = new System.Windows.Forms.Padding(1);
            this.dvGrid.VScrollBar.Maximum = 10000;
            this.dvGrid.VScrollBar.Name = "";
            this.dvGrid.VScrollBar.ScrollBarPartitionWidth = new System.Windows.Forms.Padding(5);
            this.dvGrid.VScrollBar.Size = new System.Drawing.Size(12, 177);
            this.dvGrid.VScrollBar.SmallChange = 500;
            this.dvGrid.VScrollBar.Visible = false;
            // 
            // tmUpdate
            // 
            this.tmUpdate.Enabled = true;
            this.tmUpdate.Interval = 2000;
            this.tmUpdate.Tick += new System.EventHandler(this.tmUpdate_Tick);
            // 
            // tmReConnection
            // 
            this.tmReConnection.Tick += new System.EventHandler(this.tmReConnection_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1336, 704);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnD_1);
            this.Controls.Add(this.btnD_2);
            this.Controls.Add(this.btnB_1);
            this.Controls.Add(this.btnB_2);
            this.Controls.Add(this.btnA_2);
            this.Controls.Add(this.btnA_1);
            this.Controls.Add(this.btnC_2);
            this.Controls.Add(this.btnC_1);
            this.Controls.Add(this.dSkinPanel1);
            this.IsLayeredWindowForm = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "机械手&AGV搬运交互模拟程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.dSkinPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DSkin.Controls.DSkinPanel dSkinPanel1;
        private DSkin.Controls.DSkinButton btnC_1;
        private DSkin.Controls.DSkinButton btnC_2;
        private DSkin.Controls.DSkinButton btnA_2;
        private DSkin.Controls.DSkinButton btnA_1;
        private DSkin.Controls.DSkinButton btnB_1;
        private DSkin.Controls.DSkinButton btnB_2;
        private DSkin.Controls.DSkinButton btnD_1;
        private DSkin.Controls.DSkinButton btnD_2;
        private System.Windows.Forms.Timer tmPLC;
        private DSkin.Controls.DSkinButton btnApply_A_1;
        private DSkin.Controls.DSkinButton btnApply_A_3;
        private System.Windows.Forms.Timer tmAgv;
        private System.Windows.Forms.Timer tmAgvTask;
        private DSkin.Controls.DSkinButton btnApply_A_2;
        private System.Windows.Forms.GroupBox groupBox1;
        private DSkin.Controls.DSkinGridList dvGrid;
        private System.Windows.Forms.Timer tmUpdate;
        private System.Windows.Forms.Timer tmReConnection;
    }
}

