namespace test_stand
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnPower1 = new System.Windows.Forms.Button();
            this.btnPower2 = new System.Windows.Forms.Button();
            this.PnlSideMenu = new System.Windows.Forms.Panel();
            this.PnlTests = new System.Windows.Forms.Panel();
            this.BtnTests5 = new System.Windows.Forms.Button();
            this.BtnTests4 = new System.Windows.Forms.Button();
            this.BtnTests3 = new System.Windows.Forms.Button();
            this.BtnTests2 = new System.Windows.Forms.Button();
            this.BtnTests1 = new System.Windows.Forms.Button();
            this.StartTest = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.PnlParameters = new System.Windows.Forms.Panel();
            this.Settings = new System.Windows.Forms.Button();
            this.ModuleSettings = new System.Windows.Forms.Button();
            this.BtnParameters = new System.Windows.Forms.Button();
            this.PnlModule = new System.Windows.Forms.Panel();
            this.BtnModule = new System.Windows.Forms.Button();
            this.PnlComPort = new System.Windows.Forms.Panel();
            this.BtnAllComPort = new System.Windows.Forms.Button();
            this.BtnPort3 = new System.Windows.Forms.Button();
            this.BtnPort2 = new System.Windows.Forms.Button();
            this.BtnPort1 = new System.Windows.Forms.Button();
            this.BtnComPortMenu = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnHW = new System.Windows.Forms.Button();
            this.PnlTC12V = new System.Windows.Forms.Panel();
            this.PnlCurrentPSC = new System.Windows.Forms.Panel();
            this.PnlChildForm = new System.Windows.Forms.Panel();
            this.PnlMain = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.HW = new System.Windows.Forms.PictureBox();
            this.PBCloseWindow = new System.Windows.Forms.PictureBox();
            this.PBMinimizeWindow = new System.Windows.Forms.PictureBox();
            this.PBMaximizeWindow = new System.Windows.Forms.PictureBox();
            this.PnlSideMenu.SuspendLayout();
            this.PnlTests.SuspendLayout();
            this.PnlParameters.SuspendLayout();
            this.PnlComPort.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.PnlChildForm.SuspendLayout();
            this.PnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBCloseWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBMinimizeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBMaximizeWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPower1
            // 
            this.btnPower1.BackColor = System.Drawing.Color.White;
            this.btnPower1.FlatAppearance.BorderSize = 0;
            this.btnPower1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower1.Location = new System.Drawing.Point(6, 12);
            this.btnPower1.Name = "btnPower1";
            this.btnPower1.Size = new System.Drawing.Size(378, 23);
            this.btnPower1.TabIndex = 46;
            this.btnPower1.Text = "Питание 1 канал:";
            this.btnPower1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPower1.UseVisualStyleBackColor = false;
            // 
            // btnPower2
            // 
            this.btnPower2.BackColor = System.Drawing.Color.White;
            this.btnPower2.FlatAppearance.BorderSize = 0;
            this.btnPower2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower2.Location = new System.Drawing.Point(6, 35);
            this.btnPower2.Name = "btnPower2";
            this.btnPower2.Size = new System.Drawing.Size(378, 23);
            this.btnPower2.TabIndex = 47;
            this.btnPower2.Text = "Питание 2 канал:";
            this.btnPower2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPower2.UseVisualStyleBackColor = false;
            // 
            // PnlSideMenu
            // 
            this.PnlSideMenu.AutoScroll = true;
            this.PnlSideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(40)))), ((int)(((byte)(52)))));
            this.PnlSideMenu.Controls.Add(this.PnlTests);
            this.PnlSideMenu.Controls.Add(this.StartTest);
            this.PnlSideMenu.Controls.Add(this.BtnExit);
            this.PnlSideMenu.Controls.Add(this.PnlParameters);
            this.PnlSideMenu.Controls.Add(this.BtnParameters);
            this.PnlSideMenu.Controls.Add(this.PnlModule);
            this.PnlSideMenu.Controls.Add(this.BtnModule);
            this.PnlSideMenu.Controls.Add(this.PnlComPort);
            this.PnlSideMenu.Controls.Add(this.BtnComPortMenu);
            this.PnlSideMenu.Controls.Add(this.panel4);
            this.PnlSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlSideMenu.Location = new System.Drawing.Point(0, 40);
            this.PnlSideMenu.Name = "PnlSideMenu";
            this.PnlSideMenu.Size = new System.Drawing.Size(250, 828);
            this.PnlSideMenu.TabIndex = 73;
            // 
            // PnlTests
            // 
            this.PnlTests.Controls.Add(this.BtnTests5);
            this.PnlTests.Controls.Add(this.BtnTests4);
            this.PnlTests.Controls.Add(this.BtnTests3);
            this.PnlTests.Controls.Add(this.BtnTests2);
            this.PnlTests.Controls.Add(this.BtnTests1);
            this.PnlTests.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTests.Location = new System.Drawing.Point(0, 548);
            this.PnlTests.Name = "PnlTests";
            this.PnlTests.Size = new System.Drawing.Size(250, 175);
            this.PnlTests.TabIndex = 9;
            // 
            // BtnTests5
            // 
            this.BtnTests5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnTests5.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnTests5.FlatAppearance.BorderSize = 0;
            this.BtnTests5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTests5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnTests5.Location = new System.Drawing.Point(0, 140);
            this.BtnTests5.Name = "BtnTests5";
            this.BtnTests5.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnTests5.Size = new System.Drawing.Size(250, 35);
            this.BtnTests5.TabIndex = 4;
            this.BtnTests5.Text = "Full Test";
            this.BtnTests5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTests5.UseVisualStyleBackColor = false;
            this.BtnTests5.Click += new System.EventHandler(this.StartTest_Click);
            // 
            // BtnTests4
            // 
            this.BtnTests4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnTests4.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnTests4.FlatAppearance.BorderSize = 0;
            this.BtnTests4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTests4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnTests4.Location = new System.Drawing.Point(0, 105);
            this.BtnTests4.Name = "BtnTests4";
            this.BtnTests4.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnTests4.Size = new System.Drawing.Size(250, 35);
            this.BtnTests4.TabIndex = 3;
            this.BtnTests4.Text = "Power Test";
            this.BtnTests4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTests4.UseVisualStyleBackColor = false;
            this.BtnTests4.Click += new System.EventHandler(this.BtnTests4_Click);
            // 
            // BtnTests3
            // 
            this.BtnTests3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnTests3.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnTests3.FlatAppearance.BorderSize = 0;
            this.BtnTests3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTests3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnTests3.Location = new System.Drawing.Point(0, 70);
            this.BtnTests3.Name = "BtnTests3";
            this.BtnTests3.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnTests3.Size = new System.Drawing.Size(250, 35);
            this.BtnTests3.TabIndex = 2;
            this.BtnTests3.Text = "Din Test";
            this.BtnTests3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTests3.UseVisualStyleBackColor = false;
            this.BtnTests3.Click += new System.EventHandler(this.Tests_Click);
            // 
            // BtnTests2
            // 
            this.BtnTests2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnTests2.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnTests2.FlatAppearance.BorderSize = 0;
            this.BtnTests2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTests2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnTests2.Location = new System.Drawing.Point(0, 35);
            this.BtnTests2.Name = "BtnTests2";
            this.BtnTests2.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnTests2.Size = new System.Drawing.Size(250, 35);
            this.BtnTests2.TabIndex = 1;
            this.BtnTests2.Text = "TC Test";
            this.BtnTests2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTests2.UseVisualStyleBackColor = false;
            this.BtnTests2.Click += new System.EventHandler(this.Tests_Click);
            // 
            // BtnTests1
            // 
            this.BtnTests1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnTests1.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnTests1.FlatAppearance.BorderSize = 0;
            this.BtnTests1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTests1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnTests1.Location = new System.Drawing.Point(0, 0);
            this.BtnTests1.Name = "BtnTests1";
            this.BtnTests1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnTests1.Size = new System.Drawing.Size(250, 35);
            this.BtnTests1.TabIndex = 0;
            this.BtnTests1.Text = "KF Test";
            this.BtnTests1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTests1.UseVisualStyleBackColor = false;
            this.BtnTests1.Click += new System.EventHandler(this.Tests_Click);
            // 
            // StartTest
            // 
            this.StartTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.StartTest.Dock = System.Windows.Forms.DockStyle.Top;
            this.StartTest.FlatAppearance.BorderSize = 0;
            this.StartTest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.StartTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartTest.ForeColor = System.Drawing.Color.LightGray;
            this.StartTest.Location = new System.Drawing.Point(0, 503);
            this.StartTest.Name = "StartTest";
            this.StartTest.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.StartTest.Size = new System.Drawing.Size(250, 45);
            this.StartTest.TabIndex = 8;
            this.StartTest.Text = "Start Tests";
            this.StartTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StartTest.UseVisualStyleBackColor = false;
            this.StartTest.Click += new System.EventHandler(this.Button_Menu_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnExit.FlatAppearance.BorderSize = 0;
            this.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnExit.ForeColor = System.Drawing.Color.LightGray;
            this.BtnExit.Location = new System.Drawing.Point(0, 783);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.BtnExit.Size = new System.Drawing.Size(250, 45);
            this.BtnExit.TabIndex = 7;
            this.BtnExit.Text = "Exit";
            this.BtnExit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.Close_Window);
            // 
            // PnlParameters
            // 
            this.PnlParameters.Controls.Add(this.Settings);
            this.PnlParameters.Controls.Add(this.ModuleSettings);
            this.PnlParameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlParameters.Location = new System.Drawing.Point(0, 433);
            this.PnlParameters.Name = "PnlParameters";
            this.PnlParameters.Size = new System.Drawing.Size(250, 70);
            this.PnlParameters.TabIndex = 6;
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.Settings.Dock = System.Windows.Forms.DockStyle.Top;
            this.Settings.FlatAppearance.BorderSize = 0;
            this.Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Settings.Location = new System.Drawing.Point(0, 35);
            this.Settings.Name = "Settings";
            this.Settings.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.Settings.Size = new System.Drawing.Size(250, 35);
            this.Settings.TabIndex = 1;
            this.Settings.Text = "Settings";
            this.Settings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Settings.UseVisualStyleBackColor = false;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // ModuleSettings
            // 
            this.ModuleSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.ModuleSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModuleSettings.FlatAppearance.BorderSize = 0;
            this.ModuleSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ModuleSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ModuleSettings.Location = new System.Drawing.Point(0, 0);
            this.ModuleSettings.Name = "ModuleSettings";
            this.ModuleSettings.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.ModuleSettings.Size = new System.Drawing.Size(250, 35);
            this.ModuleSettings.TabIndex = 0;
            this.ModuleSettings.Text = "Module Settings";
            this.ModuleSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ModuleSettings.UseVisualStyleBackColor = false;
            this.ModuleSettings.Click += new System.EventHandler(this.Module_Settings_Click);
            // 
            // BtnParameters
            // 
            this.BtnParameters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.BtnParameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnParameters.FlatAppearance.BorderSize = 0;
            this.BtnParameters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.BtnParameters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnParameters.ForeColor = System.Drawing.Color.LightGray;
            this.BtnParameters.Location = new System.Drawing.Point(0, 388);
            this.BtnParameters.Name = "BtnParameters";
            this.BtnParameters.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.BtnParameters.Size = new System.Drawing.Size(250, 45);
            this.BtnParameters.TabIndex = 5;
            this.BtnParameters.Text = "Parameters";
            this.BtnParameters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnParameters.UseVisualStyleBackColor = false;
            this.BtnParameters.Click += new System.EventHandler(this.Button_Menu_Click);
            // 
            // PnlModule
            // 
            this.PnlModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlModule.Location = new System.Drawing.Point(0, 350);
            this.PnlModule.Name = "PnlModule";
            this.PnlModule.Size = new System.Drawing.Size(250, 38);
            this.PnlModule.TabIndex = 4;
            // 
            // BtnModule
            // 
            this.BtnModule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.BtnModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnModule.FlatAppearance.BorderSize = 0;
            this.BtnModule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.BtnModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnModule.ForeColor = System.Drawing.Color.LightGray;
            this.BtnModule.Location = new System.Drawing.Point(0, 305);
            this.BtnModule.Name = "BtnModule";
            this.BtnModule.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.BtnModule.Size = new System.Drawing.Size(250, 45);
            this.BtnModule.TabIndex = 3;
            this.BtnModule.Text = "Module";
            this.BtnModule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnModule.UseVisualStyleBackColor = false;
            this.BtnModule.Click += new System.EventHandler(this.Button_Menu_Click);
            // 
            // PnlComPort
            // 
            this.PnlComPort.Controls.Add(this.BtnAllComPort);
            this.PnlComPort.Controls.Add(this.BtnPort3);
            this.PnlComPort.Controls.Add(this.BtnPort2);
            this.PnlComPort.Controls.Add(this.BtnPort1);
            this.PnlComPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlComPort.Location = new System.Drawing.Point(0, 165);
            this.PnlComPort.Name = "PnlComPort";
            this.PnlComPort.Size = new System.Drawing.Size(250, 140);
            this.PnlComPort.TabIndex = 2;
            // 
            // BtnAllComPort
            // 
            this.BtnAllComPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnAllComPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnAllComPort.FlatAppearance.BorderSize = 0;
            this.BtnAllComPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAllComPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnAllComPort.Location = new System.Drawing.Point(0, 105);
            this.BtnAllComPort.Name = "BtnAllComPort";
            this.BtnAllComPort.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnAllComPort.Size = new System.Drawing.Size(250, 35);
            this.BtnAllComPort.TabIndex = 3;
            this.BtnAllComPort.Text = "Open All Ports";
            this.BtnAllComPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAllComPort.UseVisualStyleBackColor = false;
            this.BtnAllComPort.Click += new System.EventHandler(this.BtnAllComPort_Click);
            // 
            // BtnPort3
            // 
            this.BtnPort3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnPort3.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnPort3.FlatAppearance.BorderSize = 0;
            this.BtnPort3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPort3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnPort3.Location = new System.Drawing.Point(0, 70);
            this.BtnPort3.Name = "BtnPort3";
            this.BtnPort3.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnPort3.Size = new System.Drawing.Size(250, 35);
            this.BtnPort3.TabIndex = 2;
            this.BtnPort3.Text = "Port B";
            this.BtnPort3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPort3.UseVisualStyleBackColor = false;
            this.BtnPort3.Click += new System.EventHandler(this.Open_Com_Port_Parameters);
            // 
            // BtnPort2
            // 
            this.BtnPort2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnPort2.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnPort2.FlatAppearance.BorderSize = 0;
            this.BtnPort2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPort2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnPort2.Location = new System.Drawing.Point(0, 35);
            this.BtnPort2.Name = "BtnPort2";
            this.BtnPort2.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnPort2.Size = new System.Drawing.Size(250, 35);
            this.BtnPort2.TabIndex = 1;
            this.BtnPort2.Text = "Port A";
            this.BtnPort2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPort2.UseVisualStyleBackColor = false;
            this.BtnPort2.Click += new System.EventHandler(this.Open_Com_Port_Parameters);
            // 
            // BtnPort1
            // 
            this.BtnPort1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.BtnPort1.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnPort1.FlatAppearance.BorderSize = 0;
            this.BtnPort1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPort1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnPort1.Location = new System.Drawing.Point(0, 0);
            this.BtnPort1.Name = "BtnPort1";
            this.BtnPort1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnPort1.Size = new System.Drawing.Size(250, 35);
            this.BtnPort1.TabIndex = 0;
            this.BtnPort1.Text = "Control Port";
            this.BtnPort1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPort1.UseVisualStyleBackColor = false;
            this.BtnPort1.Click += new System.EventHandler(this.Open_Com_Port_Parameters);
            // 
            // BtnComPortMenu
            // 
            this.BtnComPortMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.BtnComPortMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnComPortMenu.FlatAppearance.BorderSize = 0;
            this.BtnComPortMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.BtnComPortMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnComPortMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnComPortMenu.ForeColor = System.Drawing.Color.LightGray;
            this.BtnComPortMenu.Location = new System.Drawing.Point(0, 120);
            this.BtnComPortMenu.Name = "BtnComPortMenu";
            this.BtnComPortMenu.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.BtnComPortMenu.Size = new System.Drawing.Size(250, 45);
            this.BtnComPortMenu.TabIndex = 1;
            this.BtnComPortMenu.Text = "Port Menu";
            this.BtnComPortMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnComPortMenu.UseVisualStyleBackColor = false;
            this.BtnComPortMenu.Click += new System.EventHandler(this.Button_Menu_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(125)))), ((int)(((byte)(137)))));
            this.panel4.Controls.Add(this.HW);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(250, 120);
            this.panel4.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DarkGray;
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.PBCloseWindow);
            this.panel5.Controls.Add(this.PBMinimizeWindow);
            this.panel5.Controls.Add(this.PBMaximizeWindow);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1298, 40);
            this.panel5.TabIndex = 74;
            this.panel5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel2_MouseDown);
            this.panel5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel2_MouseMove);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Gray;
            this.panel8.Controls.Add(this.btnHW);
            this.panel8.Controls.Add(this.PnlTC12V);
            this.panel8.Controls.Add(this.PnlCurrentPSC);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(27)))), ((int)(((byte)(154)))));
            this.panel8.Location = new System.Drawing.Point(250, 746);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1048, 122);
            this.panel8.TabIndex = 75;
            // 
            // btnHW
            // 
            this.btnHW.BackColor = System.Drawing.Color.LightGray;
            this.btnHW.FlatAppearance.BorderSize = 0;
            this.btnHW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHW.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnHW.Location = new System.Drawing.Point(448, 65);
            this.btnHW.Name = "btnHW";
            this.btnHW.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnHW.Size = new System.Drawing.Size(460, 35);
            this.btnHW.TabIndex = 3;
            this.btnHW.Text = "Power U2:";
            this.btnHW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHW.UseVisualStyleBackColor = false;
            // 
            // PnlTC12V
            // 
            this.PnlTC12V.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlTC12V.Location = new System.Drawing.Point(515, 0);
            this.PnlTC12V.Name = "PnlTC12V";
            this.PnlTC12V.Size = new System.Drawing.Size(533, 122);
            this.PnlTC12V.TabIndex = 3;
            // 
            // PnlCurrentPSC
            // 
            this.PnlCurrentPSC.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlCurrentPSC.Location = new System.Drawing.Point(0, 0);
            this.PnlCurrentPSC.Name = "PnlCurrentPSC";
            this.PnlCurrentPSC.Size = new System.Drawing.Size(515, 122);
            this.PnlCurrentPSC.TabIndex = 1;
            // 
            // PnlChildForm
            // 
            this.PnlChildForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(31)))), ((int)(((byte)(123)))));
            this.PnlChildForm.Controls.Add(this.PnlMain);
            this.PnlChildForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlChildForm.Location = new System.Drawing.Point(250, 40);
            this.PnlChildForm.Name = "PnlChildForm";
            this.PnlChildForm.Size = new System.Drawing.Size(1048, 706);
            this.PnlChildForm.TabIndex = 76;
            // 
            // PnlMain
            // 
            this.PnlMain.BackColor = System.Drawing.Color.LightGray;
            this.PnlMain.Controls.Add(this.pnlRight);
            this.PnlMain.Controls.Add(this.pnlLeft);
            this.PnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMain.Location = new System.Drawing.Point(0, 0);
            this.PnlMain.Name = "PnlMain";
            this.PnlMain.Size = new System.Drawing.Size(1048, 706);
            this.PnlMain.TabIndex = 2;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRight.Location = new System.Drawing.Point(515, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(0, 0, 0, 35);
            this.pnlRight.Size = new System.Drawing.Size(533, 706);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlLeft
            // 
            this.pnlLeft.AutoScroll = true;
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 35, 0, 35);
            this.pnlLeft.Size = new System.Drawing.Size(515, 706);
            this.pnlLeft.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(505, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            // 
            // HW
            // 
            this.HW.Image = global::test_stand.Properties.Resources.HW3;
            this.HW.Location = new System.Drawing.Point(38, 3);
            this.HW.Name = "HW";
            this.HW.Size = new System.Drawing.Size(106, 108);
            this.HW.TabIndex = 0;
            this.HW.TabStop = false;
            // 
            // PBCloseWindow
            // 
            this.PBCloseWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PBCloseWindow.BackColor = System.Drawing.Color.LightGray;
            this.PBCloseWindow.Image = ((System.Drawing.Image)(resources.GetObject("PBCloseWindow.Image")));
            this.PBCloseWindow.Location = new System.Drawing.Point(1260, 11);
            this.PBCloseWindow.Name = "PBCloseWindow";
            this.PBCloseWindow.Size = new System.Drawing.Size(20, 20);
            this.PBCloseWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBCloseWindow.TabIndex = 3;
            this.PBCloseWindow.TabStop = false;
            this.PBCloseWindow.Click += new System.EventHandler(this.Close_Window);
            // 
            // PBMinimizeWindow
            // 
            this.PBMinimizeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PBMinimizeWindow.BackColor = System.Drawing.Color.LightGray;
            this.PBMinimizeWindow.Image = ((System.Drawing.Image)(resources.GetObject("PBMinimizeWindow.Image")));
            this.PBMinimizeWindow.Location = new System.Drawing.Point(1234, 11);
            this.PBMinimizeWindow.Name = "PBMinimizeWindow";
            this.PBMinimizeWindow.Size = new System.Drawing.Size(20, 20);
            this.PBMinimizeWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBMinimizeWindow.TabIndex = 2;
            this.PBMinimizeWindow.TabStop = false;
            this.PBMinimizeWindow.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // PBMaximizeWindow
            // 
            this.PBMaximizeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PBMaximizeWindow.BackColor = System.Drawing.Color.LightGray;
            this.PBMaximizeWindow.Image = ((System.Drawing.Image)(resources.GetObject("PBMaximizeWindow.Image")));
            this.PBMaximizeWindow.Location = new System.Drawing.Point(1234, 11);
            this.PBMaximizeWindow.Name = "PBMaximizeWindow";
            this.PBMaximizeWindow.Size = new System.Drawing.Size(20, 20);
            this.PBMaximizeWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBMaximizeWindow.TabIndex = 1;
            this.PBMaximizeWindow.TabStop = false;
            this.PBMaximizeWindow.Click += new System.EventHandler(this.PBMaximizeWindow_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 868);
            this.Controls.Add(this.PnlChildForm);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.PnlSideMenu);
            this.Controls.Add(this.panel5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.PnlSideMenu.ResumeLayout(false);
            this.PnlTests.ResumeLayout(false);
            this.PnlParameters.ResumeLayout(false);
            this.PnlComPort.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.PnlChildForm.ResumeLayout(false);
            this.PnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBCloseWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBMinimizeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBMaximizeWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnPower1;
        private System.Windows.Forms.Button btnPower2;
        private System.Windows.Forms.Panel PnlSideMenu;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel PnlParameters;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Button ModuleSettings;
        private System.Windows.Forms.Button BtnParameters;
        private System.Windows.Forms.Panel PnlModule;
        private System.Windows.Forms.Button BtnModule;
        private System.Windows.Forms.Panel PnlComPort;
        private System.Windows.Forms.Button BtnPort3;
        private System.Windows.Forms.Button BtnPort2;
        private System.Windows.Forms.Button BtnPort1;
        private System.Windows.Forms.Button BtnComPortMenu;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel PnlChildForm;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Panel PnlCurrentPSC;
        private System.Windows.Forms.Button BtnAllComPort;
        private System.Windows.Forms.PictureBox PBMaximizeWindow;
        private System.Windows.Forms.PictureBox PBMinimizeWindow;
        private System.Windows.Forms.PictureBox PBCloseWindow;
        private System.Windows.Forms.Panel PnlMain;
        private System.Windows.Forms.Button StartTest;
        private System.Windows.Forms.Panel PnlTests;
        private System.Windows.Forms.Button BtnTests1;
        private System.Windows.Forms.Button BtnTests3;
        private System.Windows.Forms.Button BtnTests2;
        private System.Windows.Forms.Button BtnTests4;
        private System.Windows.Forms.Button BtnTests5;
        private System.Windows.Forms.Panel PnlTC12V;
        private System.Windows.Forms.PictureBox HW;
        private System.Windows.Forms.Button btnHW;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label label1;
    }
}

