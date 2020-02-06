using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Data.SqlClient;
using System.Collections;
using Newtonsoft.Json;
using ModBus_Library;
using Support_Class;

namespace test_stand
{
    public partial class FormMain : Form
    {
        bool cycle = true;

        List<Module_Setup> setup;
        List<My_Panel> all_panel = new List<My_Panel>();
        List<My_Button> all_button = new List<My_Button>();
        List<Send_Data> sending_data = new List<Send_Data>();

        Module_Parameters module_parameters;
        ModBus_Libra PortControl = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port1);
        ModBus_Libra PortChanelA = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port2);
        ModBus_Libra PortChanelB = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port3);

        public FormMain()
        {
            InitializeComponent();

            using (StreamReader sw = new StreamReader(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module.txt"))
                module_parameters = JsonConvert.DeserializeObject<Module_Parameters>(sw.ReadToEnd());
            using (StreamReader sw = new StreamReader(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt"))
                setup = JsonConvert.DeserializeObject<List<Module_Setup>>(sw.ReadToEnd());
            module_parameters.using_module = setup[0];
            setup.Sort((x, y) => y.name.CompareTo(x.name));

            PnlModule.Height = setup.Count * 35;
            foreach (Module_Setup ms in setup)
                PnlModule.Controls.Add(new Module_Button(ms.name, new EventHandler(Module_Selection)));

            HW.Visible = false;
            btnHW.Visible = false;

            this.KeyDown += (s, e) =>
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        all_button.Where(x => x.name.Contains("currentPSC")).ToList()[0].PerformClick();
                        break;
                    case Keys.Z:
                        if (Data_Transit.shift_is_down) { Data_Transit.shift_is_down = false; Form6 form6 = new Form6(PortControl); form6.Show(); }
                        break;
                    case Keys.X:
                        if (Data_Transit.shift_is_down) { Data_Transit.shift_is_down = false; Form6 form6 = new Form6(PortChanelA); form6.Show(); }
                        break;
                    case Keys.T:
                        if (Data_Transit.shift_is_down) MyTest();
                        break;
                    case Keys.Escape:
                        Data_Transit.escape = true;
                        PnlComPort.Visible = false;
                        PnlModule.Visible = false;
                        PnlParameters.Visible = false;
                        PnlTests.Visible = false;
                        if (Active_Form != null) { Active_Form.Close(); Active_Form = null; PnlMain.Visible = true; Open_Window = "form1"; }
                        Data_Transit.serial_number = 0;
                        break;
                    case Keys.A:
                        BtnAllComPort_Click(null, null);
                        break;
                    case Keys.M:
                        Module_Settings_Click(null, null);
                        break;
                    case Keys.D1:
                        if (Data_Transit.shift_is_down)
                        {
                            byte set = 1;
                            var co = Data_Transit.port_control_button.Where(c => c.name.Contains("din"));
                            foreach (Button_Send a in co) if (a.button.BackColor == Color.Red) set = 0;
                            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Din16.Addres, 0x10, 0, 0x51, 00, 16, 32, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set });
                        }
                        else BtnComPortMenu.PerformClick();
                        break;
                    case Keys.D2:
                        if (Data_Transit.shift_is_down)
                        {
                            byte set = 1;
                            var co = Data_Transit.port_control_button.Where(c => c.name.Contains("kf"));
                            foreach (Button_Send a in co) if (a.button.BackColor == Color.Red) set = 0;
                            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x5d, 00, 03, 06, 0, set, 0, set, 0, set });
                        }
                        else BtnModule.PerformClick();
                        break;
                    case Keys.D3:
                        if (Data_Transit.shift_is_down)
                        {
                            byte set = 1;
                            var co = Data_Transit.port_control_button.Where(c => c.name.Contains("tc"));
                            foreach (Button_Send a in co) if (a.button.BackColor == Color.Red) set = 0;
                            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x55, 00, 03, 06, 0, set, 0, set, 0, set });
                        }
                        else BtnParameters.PerformClick();
                        break;
                    case Keys.D4:
                        StartTest.PerformClick();
                        break;
                    case Keys.ShiftKey:
                        Data_Transit.shift_is_down = true;
                        break;
                    case Keys.S:
                        if (Data_Transit.shift_is_down) { StartTest_Click(null, null); }
                        else { Settings_Click(null, null); }
                        break;
                    case Keys.D:
                        PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
                        break;
                }
            };

            this.KeyUp += (s, e) => { if (e.KeyCode == Keys.ShiftKey) { Data_Transit.shift_is_down = false; } };

            this.StartPosition = FormStartPosition.CenterScreen;

            #region all_TC

            for (int a = 15; a > 7; a--)
                all_button.Add(new My_Button($"Din {a + 17}: ", "din16", Color.LightGray, Color.Black,
                    visible: new My_Control_Visible("din32", 1),
                    my_button_click: new My_Button_Click(PortControl, module_parameters.dout_din32, new byte[] { 0, 06, 0, (byte)(0x51 + a), 0, 0 }),
                    button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_din32, 1 << (a - 8))));
            for (int a = 7; a >= 0; a--)
                all_button.Add(new My_Button($"Din {a + 17}: ", "din16", Color.LightGray, Color.Black,
                    visible: new My_Control_Visible("din32", 1),
                    my_button_click: new My_Button_Click(PortControl, module_parameters.dout_din32, new byte[] { 0, 06, 0, (byte)(0x51 + a), 0, 0 }),
                    button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_din32, 1 << (a + 8))));
            for (int a = 15; a > 7; a--)
                all_button.Add(new My_Button($"Din {a + 1}: ", "din16", Color.LightGray, Color.Black,
                    visible: new My_Control_Visible("din16", (byte)((a + 1) * 2)),
                    my_button_click: new My_Button_Click(PortControl, module_parameters.dout_din16, new byte[] { 0, 06, 0, (byte)(0x51 + a), 0, 0 }),
                    button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_din16, 1 << (a - 8))));
            for (int a = 7; a >= 0; a--)
                all_button.Add(new My_Button($"Din {a + 1}: ", "din16", Color.LightGray, Color.Black,
                    visible: new My_Control_Visible("din16", (byte)((a + 1) * 2)),
                    my_button_click: new My_Button_Click(PortControl, module_parameters.dout_din16, new byte[] { 0, 06, 0, (byte)(0x51 + a), 0, 0 }),
                    button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_din16, 1 << (a + 8))));
            for (int a = 2; a >= 0; a--)
                all_button.Add(new My_Button($"TC {(char)(a + 'A')}: ", "tc", Color.LightGray, Color.Black,
                    my_button_click: new My_Button_Click(PortControl, module_parameters.dout_control, new byte[] { 0, 06, 0, (byte)(0x55 + a), 0, 0 }),
                    button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_control, 1 << (a + 12))));
            for (int a = 2; a >= 0; a--)
                all_button.Add(new My_Button($"KF {(char)(a + 'A')}: ", "kf", Color.LightGray, Color.Black,
                    my_button_click: new My_Button_Click(PortControl, module_parameters.dout_control, new byte[] { 0, 06, 0, (byte)(0x5d + a), 0, 0 }),
                    button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_control, 1 << (a + 4))));

            #endregion

            #region other_button

            all_button.Add(new My_Button($"Измерение Ток 0: ", "temperature", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("temperature", 4)));
            all_button.Add(new My_Button($"Температура: ", "temperature", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("temperature", 2)));
            all_button.Add(new My_Button($"Ток потребления канал 1: ", "currentPSC", Color.Gray, Color.LightGray,
                my_button_click: new My_Button_Click(PortControl, module_parameters.dout_control, new byte[] { 0, 0x10, 0, 0x5b, 00, 02, 04, 0, 0, 0, 0 }),
                button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_control, 12),
                result: new My_Button_Result(PortControl, module_parameters.current_psc, 0x010a, 0)));
            all_button.Add(new My_Button($"Проверка ТУ: ", "TC TU", Color.Gray, Color.LightGray,
                result: new My_Button_Result(PortControl, module_parameters.v12, 0x0108, 0)));
            all_button.Add(new My_Button($"TC 12B: ", "TC12v", Color.Gray, Color.LightGray,
                result: new My_Button_Result(PortControl, module_parameters.v12, 0x0108, 1)));

            #endregion

            all_panel.Add(new My_Panel("din", new Padding(35, 0, 0, 0), all_button.Where(a => a.name.Contains("din")).ToList()));
            all_panel.Add(new My_Panel("temperature", new Padding(35, 35, 0, 0), all_button.Where(a => a.name.Contains("temperature")).ToList(), panel_visible: new My_Control_Visible("temperature", 1)));
            all_panel.Add(new My_Panel("tc", new Padding(35, 35, 35, 0), all_button.Where(a => a.name.Contains("tc")).ToList(), panel_visible: new My_Control_Visible("tc", 1)));
            all_panel.Add(new My_Panel("kf", new Padding(35, 35, 35, 0), all_button.Where(a => a.name.Contains("kf")).ToList(), panel_visible: new My_Control_Visible("kf", 1)));
            all_panel.Add(new My_Panel("TC12v", new Padding(35, 20, 35, 0), all_button.Where(a => a.name.Contains("TC12v") || a.name.Contains("TC TU")).ToList()));
            all_panel.Add(new My_Panel("currentPSC", new Padding(35, 20, 35, 0), all_button.Where(a => a.name.Contains("currentPSC")).ToList()));

            pnlLeft.Controls.AddRange(all_panel.Where(a => a.name.Contains("din")).ToArray());
            pnlRight.Controls.AddRange(all_panel.Where(a => a.name.Contains("kf") || a.name.Contains("tc") || a.name.Contains("power3") || a.name.Contains("temperature") || a.name.Contains("test")).ToArray());
            PnlCurrentPSC.Controls.AddRange(all_panel.Where(a => a.name.Contains("currentPSC")).ToArray());
            PnlTC12V.Controls.AddRange(all_panel.Where(a => a.name.Contains("TC12v")).ToArray());


            //Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.module, null, btnHW, Color.LightGray, "hw"));
            //Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Control, 15 << 8);
            //Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Control, new byte[] { 0, 0x10, 0, 0x5b, 00, 02, 04, 0, 0, 0, 0}, BtnCurent1, Color.Gray, "current")) ;
            //Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Control, 12);
            //Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, null, BtnEnTU, Color.LightGray, "entu"));
            //Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0009, Data_Transit.PortChanelA, Data_Transit.module, 1);
            //Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, new byte[] { 0, 0x06, 0, 0x61, 0, 0 }, TU1, Color.LightGray, "tu_"));
            //Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, new byte[] { 0, 0x06, 0, 0x62, 0, 0 }, TU2, Color.LightGray, "tu_"));
            //Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, new byte[] { 0, 0x06, 0, 0x63, 0, 0 }, TU3, Color.LightGray, "tu_"));


            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.Current_PSC, BtnCurent1, 0x010a, 0, "current"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.Current_PSC, BtnCurent2, 0x010a, 1, "current"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.v12, Btn12V1, 0x0108, 1, "12v"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.v12, Btn12V2, 0x0108, 0, "tu voltage"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.v12, Btn12V3, 0x0108, 2, "12v2"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, PWR_1_MTU5, 0, 0, "power"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, PWR_2_MTU5, 0, 1, "power"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, BtnTemperature, 0, 0, "temperature"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, TU1, 0, 0, "mtu_tu"));
            //Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, TU2, 0, 1, "mtu_tu"));

            //Data_Transit.controls_module.Add(new Controls_Only(TU1, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 0, "tu"));
            //Data_Transit.controls_module.Add(new Controls_Only(TU2, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 1, "tu"));
            //Data_Transit.controls_module.Add(new Controls_Only(TU3, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 2, "tu"));

            //Data_Transit.controls_module.Add(new Controls_Only(TU1, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 0, "tu_control"));
            //Data_Transit.controls_module.Add(new Controls_Only(TU2, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 1, "tu_control"));
            //Data_Transit.controls_module.Add(new Controls_Only(TU3, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 2, "tu_control"));

            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }, PortControl, module_parameters.dout_control));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x04, 0x01, 0x0a, 0x00, 0x04 }, PortControl, module_parameters.current_psc));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }, PortControl, module_parameters.dout_din16));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }, PortControl, module_parameters.dout_din32));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x04, 0x01, 0x08, 0x00, 0x06 }, PortControl, module_parameters.v12));

            PortControl.Receive_Event += PortControl_DataReceived;
            PortChanelA.Receive_Event += PortChanelA_DataReceived;
            PortChanelB.Receive_Event += PortChanelB_DataReceived;


            PnlComPort.Visible = false;
            PnlModule.Visible = false;
            PnlTests.Visible = false;
            PnlParameters.Visible = false;
            PBMaximizeWindow.Visible = true;
            PBMinimizeWindow.Visible = false;

            #region Threading

            new Thread(() =>
            {
                while (cycle)
                {
                    if (PortControl.Port.IsOpen)
                    { foreach (Send_Data sd in sending_data) { sd.sending(); System.Threading.Thread.Sleep(100); } }
                    else
                    { PortControl.Exchange = false; System.Threading.Thread.Sleep(200); }
                }
            }).Start();// Control

            new Thread(() =>
            {
                while (cycle)
                {
                    if (PortChanelA.Port.IsOpen && module_parameters.using_module.all_registers.Values.Where(x => x[5] > 0).ToList().Count > 0)
                    {
                        foreach (byte[] send in module_parameters.using_module.all_registers.Values.Where(x => x[5] > 0).ToList())
                        { send[0] = module_parameters.module.Addres; PortChanelA.Transmit(send); System.Threading.Thread.Sleep(100); }
                    }
                    else
                    { PortChanelA.Exchange = false; System.Threading.Thread.Sleep(200); }
                }
            }).Start();// Chanel A

            new Thread(() =>
            {
                while (cycle)
                {
                    if (PortChanelB.Port.IsOpen && module_parameters.using_module.all_registers.Values.Where(x => x[5] > 0).ToList().Count > 0)
                    {
                        foreach (byte[] send in module_parameters.using_module.all_registers.Values.Where(x => x[5] > 0).ToList())
                        { send[0] = module_parameters.module.Addres; PortChanelB.Transmit(send); System.Threading.Thread.Sleep(100); }
                    }
                    else
                    { PortChanelB.Exchange = false; System.Threading.Thread.Sleep(200); }
                }
            }).Start();// Chanel B

            #endregion
        }

        #region Serial Port

        public void PortControl_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() => {
                foreach (My_Button mb in all_button) { mb.Checkout(PortControl); }
            }));
        }

        public void PortChanelA_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() => {
                foreach (My_Button mb in all_button) { mb.Checkout(PortChanelA); }
            }));
        }

        public void PortChanelB_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() => {
                foreach (My_Button mb in all_button) { mb.Checkout(PortChanelB); }
            }));
        }

        #endregion

        #region Module_Initialization

        public void Module_Selection(object sender, EventArgs e)
        {
            module_parameters.using_module = setup[setup.FindIndex(x => x.name == ((Module_Button)sender).Text)];

            foreach (My_Button mb in all_button)
                if (mb.my_button_visible != null) mb.visible(module_parameters.using_module.all_registers[mb.my_button_visible.name][5], module_parameters.using_module.name);
            foreach (My_Panel mp in all_panel) { if (mp.my_panel_visible != null) mp.visible(module_parameters.using_module.all_registers[mp.my_panel_visible.name][5], module_parameters.using_module.name); mp.width_with_buttons(); }

            foreach (string name in module_parameters.using_module.all_registers.Keys)
            {
                List<My_Button> mb = all_button.Where(x => x.name == name && x.Visible).ToList();
                mb.Reverse();
                for (int a = 0; a < mb.Count; a++)
                {
                    mb[a].button_result = new My_Button_Result(PortChanelA, module_parameters.module, (short)((short)(module_parameters.using_module.all_registers[name][2] << 8) | (short)(module_parameters.using_module.all_registers[name][3])), a, checkout_port2: PortChanelB);
                }
            }

            if (Open_Window == "form3") Open_Child_Form(new Modul_Settings(module_parameters, setup));
        }

        #endregion

        #region Control

        Point LastPoint;
        int LocX;
        int LocY;
        int SizeW;
        int SizeH;

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - LastPoint.X;
                this.Top += e.Y - LastPoint.Y;
            }
        }

        private void PBMaximizeWindow_Click(object sender, EventArgs e)
        {
            LocX = this.Location.X;
            LocY = this.Location.Y;
            SizeW = this.Size.Width;
            SizeH = this.Size.Height;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            PBMaximizeWindow.Visible = false;
            PBMinimizeWindow.Visible = true;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.Size = new Size(SizeW, SizeH);
            this.Location = new Point(LocX, LocY);
            PBMaximizeWindow.Visible = true;
            PBMinimizeWindow.Visible = false;
        }

        private void Close_Window(object sender, EventArgs e)
        {
            PortControl.Port.Close();
            PortChanelA.Port.Close();
            PortChanelB.Port.Close();
            Properties.Settings.Default.Port1 = PortControl.Name;
            Properties.Settings.Default.Port2 = PortChanelA.Name;
            Properties.Settings.Default.Port3 = PortChanelB.Name;
            Properties.Settings.Default.Save();
            cycle = false;
            System.Threading.Thread.Sleep(500);
            Application.Exit();
        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e) { LastPoint = new Point(e.X, e.Y); }

        #endregion

        #region Side Menu

        private void Open_Close_Menu(Panel panel)
        {
            if (panel.Visible)
            {
                panel.Visible = false;
                if (Active_Form != null) { Active_Form.Close(); Active_Form = null; PnlMain.Visible = true; Open_Window = "form1"; }
            }
            else
            {
                PnlComPort.Visible = false;
                PnlModule.Visible = false;
                PnlParameters.Visible = false;
                PnlTests.Visible = false;
                panel.Visible = true;
            }
        }

        private void Button_Menu_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "BtnComPortMenu":
                    Open_Close_Menu(PnlComPort); break;
                case "BtnParameters":
                    Open_Close_Menu(PnlParameters); break;
                case "BtnModule":
                    Open_Close_Menu(PnlModule); break;
                case "StartTest":
                    Open_Close_Menu(PnlTests); break;
            }
        }

        #endregion

        #region Open/Close Forms

        private Form Active_Form = null;
        private string Open_Window = "form1";

        private void Open_Child_Form(Form Child_Form)
        {
            if (Active_Form != null) Active_Form.Close();
            Active_Form = Child_Form;
            Child_Form.TopLevel = false;
            Child_Form.FormBorderStyle = FormBorderStyle.None;
            Child_Form.Dock = DockStyle.Fill;
            PnlChildForm.Controls.Add(Child_Form);
            PnlChildForm.Tag = Child_Form;
            PnlMain.Visible = false;
            Child_Form.BringToFront();
            Child_Form.Show();
        }

        private void Open_Com_Port_Parameters(object sender, EventArgs e)
        {
            Open_Window = "form2";
            switch (((Button)sender).Name)
            {
                case "BtnPort1":
                    Open_Child_Form(new Form2(PortControl)); break;
                case "BtnPort2":
                    Open_Child_Form(new Form2(PortChanelA)); break;
                case "BtnPort3":
                    Open_Child_Form(new Form2(PortChanelB)); break;
            }
        }

        private void Settings_Click(object sender, EventArgs e) { Open_Child_Form(new Settings(module_parameters, PortControl)); Open_Window = "form4"; }

        private void Module_Settings_Click(object sender, EventArgs e) { Open_Child_Form(new Modul_Settings(module_parameters, setup)); Open_Window = "form3"; }

        private async void BtnAllComPort_Click(object sender, EventArgs e)
        {
            await Task.Run(() => {
                PortControl.Open();
                PortChanelA.Open();
                PortChanelB.Open();
            });
            if (!PortControl.Port.IsOpen || !PortChanelA.Port.IsOpen || !PortChanelB.Port.IsOpen)
                MessageBox.Show("Не все порты открыты", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region All Buttons

        private void Controls_Click(object sender, EventArgs e)
        {
            var co = Data_Transit.port_control_button.Where(c => c.button.Name == ((Button)sender).Name);
            co.ToList()[0].Trasmit_Data();
        }

        private async void Tests_Click(object sender, EventArgs e)
        {
            Data_Transit.serial_number = 0;
            Min_Max_None min_max = new Min_Max_None(0, 0, 0);
            string parameters = "";
            string test_name = "";

            switch(((Button)sender).Name)
            {
                case "BtnTests1":
                    min_max = module_parameters.using_module.kf;
                    parameters = "kf";
                    test_name = "Проверка КФ";
                    break;
                case "BtnTests2":
                    min_max = module_parameters.using_module.tc;
                    parameters = "tc";
                    test_name = "Проверка TC";
                    break;
                case "BtnTests3":
                    min_max = module_parameters.using_module.din;
                    parameters = "din";
                    test_name = "Проверка Din";
                    break;
            }

            Data_Transit.escape = false;
            Result result1 = new Result(new List<Results_Test>() { await All_TC_Test(parameters, test_name, min_max) });
            if(!Data_Transit.escape) result1.Show();
        }

        private async void BtnTests4_Click(object sender, EventArgs e)
        {
            Data_Transit.serial_number = 0;
            Result result1 = new Result(new List<Results_Test>() { await Power_Test() });
            result1.Show();
        }

        private async void StartTest_Click(object sender, EventArgs e)
        {
            Data_Transit.serial_number = 0;
            if (!PortControl.Port.IsOpen || !PortChanelA.Port.IsOpen || !PortChanelB.Port.IsOpen)
                { MessageBox.Show("Не все порты открыты", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            int norm = 0;
            while (norm < 20)
            {
                await Task.Delay(50);
                norm++;
                if (PortControl.Exchange) break;
            }
            if (norm >= 20) { MessageBox.Show("Нет обмена по каналу управления", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            while (norm < 20)
            {
                await Task.Delay(50);
                norm++;
                if (PortChanelA.Exchange) break;
            }
            if (norm >= 20) { MessageBox.Show("Нет обмена по каналу А модуля", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            while (norm < 20)
            {
                await Task.Delay(50);
                norm++;
                if (PortChanelB.Exchange) break;
            }
            if (norm >= 20) { MessageBox.Show("Нет обмена по каналу B модуля", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (all_button.Find(x => x.name == "currentPSC").Result > module_parameters.using_module.current.Max) 
            {
                MessageBox.Show("Повышен ток потребления блока", "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (all_button.Find(x => x.name == "currentPSC").Result < module_parameters.using_module.current.Min)
            {
                MessageBox.Show("Низкий ток потребления блока", "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Data_Transit.shift_is_down = false;
            Form5 form5 = new Form5();
            form5.Show();

            while (form5.Visible) { await Task.Delay(500); }

            if (Data_Transit.serial_number == 0) return;

            Data_Transit.escape = false;
            List<Results_Test> result = new List<Results_Test>()
            {
                await Power_Test(),
            };
            await Task.Delay(2500);
            if (module_parameters.using_module.tests["Проверка Din"] && !Data_Transit.escape)  result.Add(await All_TC_Test("din", "Проверка Din", module_parameters.using_module.din));
            if (module_parameters.using_module.tests["Проверка KF"] && !Data_Transit.escape) result.Add(await All_TC_Test("kf", "Проверка КФ", module_parameters.using_module.kf));
            if (module_parameters.using_module.tests["Проверка TC"] && !Data_Transit.escape) result.Add(await All_TC_Test("tc", "Проверка TC", module_parameters.using_module.tc));
            if (module_parameters.using_module.tests["Проверка 12B TC"] && !Data_Transit.escape) result.Add(await TC12V_Test());
            if (module_parameters.using_module.tests["Проверка температуры"] && !Data_Transit.escape) result.Add(await Temperature_Test());
            if (module_parameters.using_module.tests["Проверка ТУ"] && !Data_Transit.escape) result.Add(await TU_Test());
            if (module_parameters.using_module.tests["Проверка питания MTU5"] && !Data_Transit.escape) result.Add(await MTU_Power_Test());
            if (module_parameters.using_module.tests["Проверка ток 0"] && !Data_Transit.escape) result.Add(await PM7_Current_Test());
            if (module_parameters.using_module.tests["Проверка ТУ MTU5"] && !Data_Transit.escape) result.Add(await MTU5_TU_Test());
            if (module_parameters.using_module.tests["Проверка EnTU"] && !Data_Transit.escape) result.Add(await EnTU_Test());

            Result result1 = new Result(result);
            if (!Data_Transit.escape) result1.Show();
        }
        #endregion

        #region All_Tests

        async Task<bool> Compar(My_Button mb, float minimum, float maximum)
        {
            int count = 0;
            while (count < 20)
            {
                await Task.Delay(100);
                if (mb.Result >= minimum && mb.Result <= maximum) { return true; }
                count++;
            }
            return false;
        }

        async Task<Results_Test> MTU_Power_Test()
        {
            Results_Test result = new Results_Test("Проверка питания MTU5");
            result.test_result = true;

            var item = Data_Transit.all_button_result.Where(c => c.name.Contains("power"));
            for (int a = 0; a < 2; a++)
            {
                result.Add_Test($"Питание MTU5");

                //if (await Compar(item.ToList()[a], 850, 1350))
                //    { result.Add_Item(item.ToList()[a].Result, true); }
                //else
                //    { result.Add_Item(item.ToList()[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            }            

            return result;
        }

        async Task<Results_Test> MTU5_TU_Test()
        {
            Results_Test result = new Results_Test("Проверка ТУ MTU5");
            result.test_result = true;

            float[] TU = { 0, 0 };

            var item = Data_Transit.all_button_result.Where(c => c.name.Contains("mtu_tu"));
            for (int a = 0; a < 10; a++)
            {
                TU[0] += item.ToList()[0].Result;
                TU[1] += item.ToList()[1].Result;
                await Task.Delay(500);
            }

            TU[0] /= 10; TU[1] /= 10;
            result.Add_Test("ТУ ON/OFF");
            if (TU[0] < 20) 
                { result.Add_Item(TU[0], true); }
            else
                { result.Add_Item(TU[0], false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            result.Add_Test("ТУ RF");
            if (TU[1] < 20)
                { result.Add_Item(TU[1], true); }
            else
                { result.Add_Item(TU[1], false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> PM7_Current_Test()
        {
            Results_Test result = new Results_Test("Проверка ток 0");
            result.test_result = true;            
            float w = 0;
            float wo = 0;

            var item = Data_Transit.all_button_result.Where(c => c.name.Contains("power"));

            result.Add_Test($"Ток без нагрузки");
            for (int a = 0; a < 10; a++) { wo += item.ToList()[0].Result; await Task.Delay(250); }
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x06, 00, 0x5a, 00, 0x01 });
            await Task.Delay(2500);
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(250);
            wo /= 10;
            result.Add_Item(wo, true);
            result.Add_Test($"Ток с нагрузкой");
            for (int a = 0; a < 10; a++) { w += item.ToList()[0].Result; await Task.Delay(250); }
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x06, 00, 0x5a, 00, 0x00 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(250);
            w /= 10;

            if (Math.Abs(w - wo) > (wo / 20))
                { result.Add_Item(w, true); }
            else
                { result.Add_Item(w, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> Temperature_Test()
        {
            Results_Test result = new Results_Test("Проверка температуры");
            result.test_result = true;

            var item = Data_Transit.all_button_result.Where(c => c.name.Contains("temperature"));
            result.Add_Test($"Температура");

            //if (await Compar(item.ToList()[0], 20, 40))
            //    { result.Add_Item(item.ToList()[0].Result, true); }
            //else
            //    { result.Add_Item(item.ToList()[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> EnTU_Test()
        {
            Results_Test result = new Results_Test("Проверка EnTU");
            result.test_result = true;

            result.Add_Test($"EnTU");

            //if (BtnEnTU.BackColor == Color.Red) 
            //{ result.Add_Item(0, true); }
            //else
            //{ result.Add_Item(0, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> TC12V_Test()
        {
            Results_Test result = new Results_Test("Проверка 12B TC");
            result.test_result = true;

            My_Button item = all_button.Find(x=>x.name== "TC12v");
            result.Add_Test($"Питание 12B");

            if (await Compar(item, module_parameters.using_module.tc12v.Min, module_parameters.using_module.tc12v.Max))
            { result.Add_Item(item.Result, true); }
            else
            { result.Add_Item(item.Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> Power_Test()
        {
            Results_Test result = new Results_Test("Проверка питания");
            result.test_result = true;

            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x59, 00, 04, 08, 0, 0, 0, 0, 0, 0, 0, 0 }); await Task.Delay(500);

            My_Button item = all_button.Find(x => x.name == "currentPSC");

            for (int a = 0; a < module_parameters.using_module.power_chanel; a++)
            {
                result.Add_Test($"Питание {a + 1} канала");
                PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x06, 0, (byte)(0x5c - a), 0, 1 }); await Task.Delay(1000);

                if (await Compar(item, module_parameters.using_module.current.Min, module_parameters.using_module.current.Max))
                    { result.Add_Item(item.Result, true); }
                else
                    { result.Add_Item(item.Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

                PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x06, 0, (byte)(0x5C - a), 0, 0 }); await Task.Delay(500);
            }

            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x59, 00, 04, 08, 0, 0, 0, 0, 0, 1, 0, 1 }); await Task.Delay(500);

            return result;
        }

        async Task<Results_Test> All_TC_Test(string parameters, string test, Min_Max_None min_max)
        {
            Results_Test result = new Results_Test(test);
            result.test_result = true;

            List<My_Button> using_button = all_button.Where(x => x.name.Contains(parameters) && x.Visible).ToList();
            foreach (My_Button mb in using_button) { mb.Reset(); await Task.Delay(200); }
            await Task.Delay(500);

            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 1, 0, 1 }); await Task.Delay(200);

             for (int kf = 0; kf < using_button.Count; kf++)
             {
                await Task.Delay(250);
                using_button[kf].PerformClick(); await Task.Delay(200);

                result.Add_Test($"{test} {kf + 1}");
                for (int check = 0; check < using_button.Count; check++)
                {                    
                    if (check == kf)
                    {
                        if (await Compar(using_button[check], min_max.Min, min_max.Max))
                            { result.Add_Item(using_button[check].Result, true); }
                        else
                        { result.Add_Item(using_button[check].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
                    }
                    else
                    {
                        if (await Compar(using_button[check], 0, min_max.None))
                        { result.Add_Item(using_button[check].Result, true); }
                        else
                        { result.Add_Item(using_button[check].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
                    }
                }
                using_button[kf].Reset(); await Task.Delay(200);
                if (Data_Transit.escape)
                {
                    await Task.Delay(250);
                    PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); await Task.Delay(200);
                    return result;
                }
            }
            await Task.Delay(250);
            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); await Task.Delay(200);
            return result;
        }

        async Task<Results_Test> TU_Test()
        {
            Results_Test result = new Results_Test("Проверка ТУ");
            result.test_result = true;

            var co = Data_Transit.port_control_button.Where(c => c.name.Contains("tu_"));
            var cr = Data_Transit.all_button_result.Where(c => c.name.Contains("tu voltage"));

            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 1, 0, 1 }); await Task.Delay(1000);
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);

            foreach (Button_Send a in co) { if (a.button.BackColor == Color.Red) { while (a.button.BackColor == Color.Red) { a.Reset(); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(200); } } }
            await Task.Delay(1000);

            for (int kf = 0; kf < Data_Transit.Registers_Module["tu"][5]; kf++)
            {
                //co.ToList()[kf].Trasmit_Data(); await Task.Delay(200); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(1000);
                co.ToList()[kf].Trasmit_Data(); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(200);
                result.Add_Test($"Тест ТУ {kf + 1}");

                //if (await Compar(cr.ToList()[0], 200, 250))
                //{ result.Add_Item(cr.ToList()[0].Result, true); }
                //else
                //{ result.Add_Item(cr.ToList()[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

                await Task.Delay(1000);
                //while (co.ToList()[kf].button.BackColor == Color.Red) { co.ToList()[kf].Reset(); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(1000); }
                co.ToList()[kf].Reset(); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(200);
                if (Data_Transit.escape)
                {
                    Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
                    while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
                    return result;
                }
            }
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); await Task.Delay(100);
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
            return result;
        }

        #endregion

        private async void MyTest()
        {
            //label1.Text = "qwe";
            //byte[] data = all_button[15].click.data;
            //data[0] = 20;
            //data[5] = 1;
            //PortControl.Interrupt(data);
            
        }        
    }
}
