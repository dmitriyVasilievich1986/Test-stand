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
            toolTip1.SetToolTip(panel5, "hello");

            using (StreamReader sw = new StreamReader(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module.txt"))
                module_parameters = JsonConvert.DeserializeObject<Module_Parameters>(sw.ReadToEnd());
            all_module_buttons();
            module_parameters.using_module = setup.Exists(x => x.name.ToLower() == "no module") ? setup.Find(x => x.name.ToLower() == "no module") : setup[0];

            HW.Visible = false;
            btnHW.Visible = false;

            this.KeyDown += async (s, e) =>
            {
                switch (e.KeyCode)
                {
                    case Keys.ControlKey:
                        Data_Transit.control_button = true;
                        break;
                    case Keys.N:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("n"); }
                        break;
                    case Keys.O:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("o"); }
                        break;
                    case Keys.Y:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("y"); }
                        if (String.Join("", Data_Transit.admin_string.ToArray()) == "copy")
                        {
                            Data_Transit.control_button = false;
                            Data_Transit.admin_string.Clear();
                            string new_module = Convert.ToString(await open_input_form("Введите название нового модуля", "новый модуль"));
                            if (new_module != "" && new_module != "новый модуль" && !setup.Exists(x => x.name == new_module))
                            {
                                setup.Add(new Module_Setup(new_module, 0,0,new Min_Max_None(0,0), new Min_Max_None(0,0), new Min_Max_None(0,0), new Min_Max_None(0,0), new Min_Max_None(0,0)));
                                foreach(string a in module_parameters.using_module.all_registers.Keys) { setup[setup.Count - 1].all_registers[a] = module_parameters.using_module.all_registers[a]; }
                                foreach(string a in module_parameters.using_module.tests.Keys) { setup[setup.Count - 1].tests[a] = module_parameters.using_module.tests[a]; }
                                using (StreamWriter sw = new StreamWriter(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt", false, Encoding.UTF8))
                                    sw.Write(JsonConvert.SerializeObject(setup));
                                all_module_buttons();
                            }
                        }
                        break;                    
                    case Keys.U:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("u"); }
                        break;
                    case Keys.L:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("l"); }
                        break;
                    case Keys.E:
                        if (Data_Transit.control_button)
                        {
                            Data_Transit.admin_string.Add("e");
                            if (String.Join("", Data_Transit.admin_string.ToArray()) == "delete")
                            {
                                Data_Transit.control_button = false;
                                Data_Transit.admin_string.Clear();
                                string delete_module = Convert.ToString(await open_input_form("Введите название модуля для удаления", "название модуля"));
                                if (delete_module != "" && delete_module != "название модуля" && setup.Exists(x => x.name == delete_module))
                                {
                                    setup.Remove(setup.Find(x => x.name == delete_module));
                                    using (StreamWriter sw = new StreamWriter(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt", false, Encoding.UTF8))
                                        sw.Write(JsonConvert.SerializeObject(setup));
                                    all_module_buttons();
                                }
                            }
                        }
                        break;
                    case Keys.W:
                        if (Data_Transit.control_button)
                        {
                            Data_Transit.admin_string.Add("w");
                            if (String.Join("", Data_Transit.admin_string.ToArray()) == "new")
                            {
                                Data_Transit.control_button = false;
                                Data_Transit.admin_string.Clear();
                                string new_module = Convert.ToString(await open_input_form("Введите название нового модуля", "новый модуль"));
                                if (new_module != "" && new_module != "новый модуль" && !setup.Exists(x => x.name == new_module)) 
                                {
                                    setup.Add(new Module_Setup(new_module, 0, 0, new Min_Max_None(0, 0), new Min_Max_None(0, 0), new Min_Max_None(0, 0), new Min_Max_None(0, 0), new Min_Max_None(0, 0)));
                                    using (StreamWriter sw = new StreamWriter(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt", false, Encoding.UTF8))
                                        sw.Write(JsonConvert.SerializeObject(setup));
                                    all_module_buttons();
                                }
                            }
                        }
                        break;
                    case Keys.P:
                        if (Data_Transit.control_button)
                        {
                            Data_Transit.admin_string.Add("p");
                            if (String.Join("", Data_Transit.admin_string.ToArray()) == "setup")
                            {
                                Module_Setup_Form setup_form = new Module_Setup_Form(module_parameters, setup);
                                Data_Transit.control_button = false;
                                Data_Transit.admin_string.Clear();
                                setup_form.Show();
                            }
                        }
                        all_button.Find(x => x.name=="currentPSC").PerformClick();
                        break;
                    case Keys.Z:
                        if (Data_Transit.shift_is_down) { Data_Transit.shift_is_down = false; Form6 form6 = new Form6(PortControl); form6.Show(); }
                        break;
                    case Keys.X:
                        if (Data_Transit.shift_is_down) { Data_Transit.shift_is_down = false; Form6 form6 = new Form6(PortChanelA); form6.Show(); }
                        break;
                    case Keys.C:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("c"); }
                        if (Data_Transit.shift_is_down) { Data_Transit.shift_is_down = false; Form6 form6 = new Form6(PortChanelB); form6.Show(); }
                        break;
                    case Keys.T:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("t"); }
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
                        if (Data_Transit.shift_is_down)
                        {
                            Data_Transit.shift_is_down = false;
                            module_parameters.module.Addres = byte.Parse(Convert.ToString(await open_input_form("Введите адрес модуля", module_parameters.module.Addres.ToString())));
                        }
                        else BtnAllComPort_Click(null, null);
                        break;
                    case Keys.M:
                        Module_Settings_Click(null, null);
                        break;
                    case Keys.D1:
                        if (Data_Transit.shift_is_down)
                        {
                            byte set = 1;
                            List<My_Button> item = all_button.FindAll(x => x.name=="din16" && x.Visible);
                            List<byte> sending_data = new List<byte>() { module_parameters.dout_din16.Addres, 0x10, 0x00, 0x51, 00, (byte)(item.Count), (byte)(item.Count*2)};
                            foreach (My_Button mb in item)  { if (mb.BackColor == Color.Red) { set = 0; break; } }
                            foreach (My_Button mb in item)  { sending_data.Add(0x00);sending_data.Add(set); }
                            PortControl.Interrupt(sending_data.ToArray());
                            Thread.Sleep(200);
                            //item = all_button.FindAll(x => x.name=="din32" && x.Visible);
                            //sending_data = new List<byte>() { module_parameters.dout_din32.Addres, 0x10, 0x00, 0x51, 00, (byte)(item.Count), (byte)(item.Count*2)};
                            //foreach (My_Button mb in item)  { if (mb.BackColor == Color.Red) { set = 0; break; } }
                            //foreach (My_Button mb in item)  { sending_data.Add(0x00);sending_data.Add(set); }
                            //PortControl.Interrupt(sending_data.ToArray());
                            //foreach (My_Button mb in item)  { mb.click.send_data(set); Thread.Sleep(200); }
                        }
                        else BtnComPortMenu.PerformClick();
                        break;
                    case Keys.D2:
                        if (Data_Transit.shift_is_down)
                        {
                            byte set = 1;
                            List<My_Button> item = all_button.FindAll(x => x.name == "kf");
                            foreach (My_Button mb in item) if (mb.BackColor == Color.Red) set = 0;
                            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x5d, 00, 03, 06, 0, set, 0, set, 0, set });
                        }
                        else BtnModule.PerformClick();
                        break;
                    case Keys.D3:
                        if (Data_Transit.shift_is_down)
                        {
                            byte set = 1;
                            List<My_Button> item = all_button.FindAll(x => x.name == "tc");
                            foreach (My_Button mb in item) if (mb.BackColor == Color.Red) set = 0;
                            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x55, 00, 03, 06, 0, set, 0, set, 0, set });
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
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("s"); }
                        if (Data_Transit.shift_is_down) { StartTest_Click(null, null); }
                        else { Settings_Click(null, null); }
                        break;
                    case Keys.D:
                        if (Data_Transit.control_button) { Data_Transit.admin_string.Add("d"); }
                        PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
                        break;
                    default:
                        Data_Transit.admin_string.Clear();
                        break;
                }
            };

            this.KeyUp += (s, e) => 
            {
                if (e.KeyCode == Keys.ShiftKey) { Data_Transit.shift_is_down = false; }
                if (e.KeyCode == Keys.ControlKey)
                {
                    Data_Transit.control_button = false;
                    Data_Transit.admin_string.Clear();
                    label1.Text = String.Join(" ", Data_Transit.admin_string.ToArray());
                }
            };

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

            for (int a = 11; a > 7; a--)
            {
                all_button.Add(new My_Button($"Din {a + 1}", "dinlight", Color.LightGray, Color.Black,
                button_color: new My_Button_Colorized(PortChanelA, 0x0001, module_parameters.module, 1 << (a - 8), checkout_port2: PortChanelB),
                visible: new My_Control_Visible("light", 1),
                my_button_click: all_button.FindAll(x => x.name == "din16")[31 - a].click
                ));
            }
            for (int a = 7; a > -1; a--)
            {
                all_button.Add(new My_Button($"Din {a+1}", "dinlight", Color.LightGray, Color.Black,
                button_color: new My_Button_Colorized(PortChanelA, 0x0001, module_parameters.module, 1 << (8+a), checkout_port2: PortChanelB),
                visible: new My_Control_Visible("light", 1),
                my_button_click: all_button.FindAll(x => x.name == "din16")[31-a].click
                ));
            }

            all_button.Add(new My_Button($"Напряжение заряда аккумулятора: ", "BatteryPSC", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("BatteryPSC", 1)));
            all_button.Add(new My_Button($"Ток заряда аккумулятора: ", "BatteryPSC", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("BatteryPSC", 1)));
            all_button.Add(new My_Button($"Ток выхода канал 2: ", "ExitPSC", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortControl, module_parameters.v12, new byte[] { 0x01, 0x06, 00, 0x63, 00, 0x01 }),
                button_color: new My_Button_Colorized(PortControl, 0x000f, module_parameters.v12, 1),
                visible: new My_Control_Visible("ExitPSC", 1)));
            all_button.Add(new My_Button($"Ток выхода канал 1: ", "ExitPSC", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortControl, module_parameters.v12, new byte[] { 0x01, 0x06, 00, 0x63, 00, 0x01 }),
                button_color: new My_Button_Colorized(PortControl, 0x000f, module_parameters.v12, 1),
                visible: new My_Control_Visible("ExitPSC", 1)));
            all_button.Add(new My_Button($"Напряжение выход канал 2: ", "ExitPSC", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("ExitPSC", 1)));
            all_button.Add(new My_Button($"Напряжение выход канал 1: ", "ExitPSC", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("ExitPSC", 1)));
            all_button.Add(new My_Button($"Питание батареи: ", "PowerPSC", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortControl, module_parameters.dout_control, new byte[] { 0, 0x06, 0, 0x59, 00, 00}),
                button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_control, 1),
                visible: new My_Control_Visible("PowerPSC", 1)));
            all_button.Add(new My_Button($"Питание канал 2: ", "PowerPSC", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortControl, module_parameters.dout_control, new byte[] { 0, 0x06, 0, 0x5c, 00, 00}),
                button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_control, 8),
                visible: new My_Control_Visible("PowerPSC", 1)));
            all_button.Add(new My_Button($"Питание канал 1: ", "PowerPSC", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortControl, module_parameters.dout_control, new byte[] { 0, 0x06, 0, 0x5b, 00, 00}),
                button_color: new My_Button_Colorized(PortControl, 0x0001, module_parameters.dout_control, 4),
                visible: new My_Control_Visible("PowerPSC", 1)));
            all_button.Add(new My_Button($"ТУ ON/OFF: ", "mtu5tu", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("mtu5tu", 4)));
            all_button.Add(new My_Button($"ТУ RF: ", "mtu5tu", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("mtu5tu", 4)));
            all_button.Add(new My_Button($"Питание канала B: ", "power", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("power", 4)));
            all_button.Add(new My_Button($"Питание канала А: ", "power", Color.LightGray, Color.Black,
                visible: new My_Control_Visible("power", 4)));
            all_button.Add(new My_Button($"ТУ RF", "ТУ модуля", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortChanelA, module_parameters.module, new byte[] { 0, 0x06, 0, 0x63, 0, 0 }),
                visible: new My_Control_Visible("tu", 3)));
            all_button.Add(new My_Button($"ТУ OFF", "ТУ модуля", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortChanelA, module_parameters.module, new byte[] { 0, 0x06, 0, 0x62, 0, 0 }),
                visible: new My_Control_Visible("tu", 2)));
            all_button.Add(new My_Button($"ТУ ON", "ТУ модуля", Color.LightGray, Color.Black,
                my_button_click: new My_Button_Click(PortChanelA, module_parameters.module, new byte[] { 0, 0x06, 0, 0x61, 0, 0 }),
                visible: new My_Control_Visible("tu", 2)));
            all_button.Add(new My_Button($"EnTU", "Entu модуля", Color.LightGray, Color.Black,                
                visible: new My_Control_Visible("entu", 1)));
            all_button.Add(new My_Button($"Измерение Ток 0: ", "temperature", Color.LightGray, Color.Black,
                my_button_click:new My_Button_Click(PortControl, module_parameters.v12, new byte[] { 0x01, 0x06, 00, 0x63, 00, 0x01 }),
                button_color: new My_Button_Colorized(PortControl, 0x000f, module_parameters.v12, 1),
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

            all_panel.Add(new My_Panel("din", new Padding(35, 0, 35, 0), Color.LightGray, all_button.FindAll(a => a.name.Contains("din"))));
            all_panel.Add(new My_Panel("PowerPSC", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "PowerPSC")));
            all_panel.Add(new My_Panel("ExitPSC", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "ExitPSC")));
            all_panel.Add(new My_Panel("ТУ модуля", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "ТУ модуля" || a.name == "Entu модуля"|| a.name == "mtu5tu"), panel_visible: new My_Control_Visible("entu", 1)));
            all_panel.Add(new My_Panel("temperature", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "temperature"), panel_visible: new My_Control_Visible("temperature", 1)));
            all_panel.Add(new My_Panel("BatteryPSC", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "BatteryPSC"), panel_visible: new My_Control_Visible("BatteryPSC", 1)));
            all_panel.Add(new My_Panel("power", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "power"), panel_visible: new My_Control_Visible("power", 4)));
            all_panel.Add(new My_Panel("tc", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "tc"), panel_visible: new My_Control_Visible("tc", 1)));
            all_panel.Add(new My_Panel("kf", new Padding(35, 35, 35, 0), Color.LightGray, all_button.FindAll(a => a.name == "kf"), panel_visible: new My_Control_Visible("kf", 1)));
            all_panel.Add(new My_Panel("TC12v", new Padding(35, 20, 35, 0), Color.Gray, all_button.FindAll(a => a.name == "TC12v" || a.name == "TC TU")));
            all_panel.Add(new My_Panel("currentPSC", new Padding(35, 20, 35, 0), Color.Gray, all_button.FindAll(a => a.name == "currentPSC")));

            pnlLeft.Controls.AddRange(all_panel.FindAll(a => a.name == "din" || a.name == "PowerPSC" || a.name == "ExitPSC").ToArray());
            pnlRight.Controls.AddRange(all_panel.FindAll(a => a.name == "kf" || a.name == "tc" || a.name == "power" || a.name == "BatteryPSC" || a.name == "mtu5tu" || a.name == "temperature" || a.name == "ТУ модуля" || a.name.Contains("test")).ToArray());
            PnlCurrentPSC.Controls.AddRange(all_panel.FindAll(a => a.name == "currentPSC").ToArray());
            PnlTC12V.Controls.AddRange(all_panel.FindAll(a => a.name == "TC12v").ToArray());

            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }, PortControl, module_parameters.dout_control));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x04, 0x01, 0x0a, 0x00, 0x04 }, PortControl, module_parameters.current_psc));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }, PortControl, module_parameters.dout_din16));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }, PortControl, module_parameters.dout_din32));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x04, 0x01, 0x08, 0x00, 0x06 }, PortControl, module_parameters.v12));
            sending_data.Add(new Send_Data(new byte[] { 0x00, 0x02, 0x00, 0x0f, 0x00, 0x01 }, PortControl, module_parameters.v12));

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

            Task.Run(async () => {
                while (cycle)
                {
                    if (PortControl.Port.IsOpen)
                    { foreach (Send_Data sd in sending_data) { sd.sending(); await Task.Delay(100); } }
                    else
                    { PortControl.Exchange = false; await Task.Delay(200); }
                }
            });

            //new Thread(() =>
            //{
            //    while (cycle)
            //    {
            //        if (PortControl.Port.IsOpen)
            //        { foreach (Send_Data sd in sending_data) { sd.sending(); System.Threading.Thread.Sleep(100); } }
            //        else
            //        { PortControl.Exchange = false; System.Threading.Thread.Sleep(200); }
            //    }
            //}).Start();// Control

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

            foreach(My_Button mb in all_button)
            {
                mb.MouseDown += (object sender, MouseEventArgs e) =>
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            ((My_Button)sender).set_button();
                            break;
                        case MouseButtons.Right:
                            ((My_Button)sender).BackColor = Color.White;
                            break;
                    }
                };
            }

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
            BeginInvoke((MethodInvoker)(() => {
                foreach (My_Panel myp in all_panel) { myp.Checkout(PortChanelA); }
            }));
        }

        public void PortChanelB_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() => {
                foreach (My_Button mb in all_button) { mb.Checkout(PortChanelB); }
            }));
            BeginInvoke((MethodInvoker)(() => {
                foreach (My_Panel myp in all_panel) { myp.Checkout(PortChanelB); }
            }));
        }

        #endregion

        #region Module_Initialization

        public void Module_Selection(object sender, EventArgs e)
        {
            module_parameters.using_module = setup[setup.FindIndex(x => x.name == ((Module_Button)sender).Text)];

            foreach (My_Button mb in all_button)
                if (mb.my_button_visible != null && module_parameters.using_module.all_registers.ContainsKey(mb.my_button_visible.name)) mb.visible(module_parameters.using_module.all_registers[mb.my_button_visible.name][5], module_parameters.using_module.name);
            foreach (My_Panel mp in all_panel) { if (mp.my_panel_visible != null && module_parameters.using_module.all_registers.ContainsKey(mp.my_panel_visible.name)) mp.visible(module_parameters.using_module.all_registers[mp.my_panel_visible.name][5], module_parameters.using_module.name); mp.width_with_buttons(); }

            foreach (string name in module_parameters.using_module.all_registers.Keys)
            {
                List<My_Button> mb = all_button.FindAll(x => x.name == name && x.Visible);
                mb.Reverse();
                for (int a = 0; a < mb.Count; a++)
                {
                    mb[a].button_result = new My_Button_Result(PortChanelA, module_parameters.module, (short)((short)(module_parameters.using_module.all_registers[name][2] << 8) | (short)(module_parameters.using_module.all_registers[name][3])), a, checkout_port2: PortChanelB);
                }
            }

            List<My_Button> mbc = all_button.FindAll(x => x.name == "ТУ модуля" && x.Visible);
            mbc.Reverse();
            for (int a = 0; a < mbc.Count; a++)
                { mbc[a].colorized = new My_Button_Colorized(PortChanelA, (short)((short)(module_parameters.using_module.all_registers["tu"][2] << 8) | (short)(module_parameters.using_module.all_registers["tu"][3])), module_parameters.module, 1 << a, checkout_port2: PortChanelB); }
            all_button.Find(x => x.name == "Entu модуля").colorized = new My_Button_Colorized(PortChanelA, (short)((short)(module_parameters.using_module.all_registers["entu"][2] << 8) | (short)(module_parameters.using_module.all_registers["entu"][3])), module_parameters.module, 1, checkout_port2: PortChanelB);
            
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
            Result result1 = new Result(new List<Results_Test>() { await All_TC_Test(parameters, test_name, min_max) }, module_parameters);
            if(!Data_Transit.escape) result1.Show();
        }

        private async void BtnTests4_Click(object sender, EventArgs e)
        {
            Data_Transit.serial_number = 0;
            Result result1 = new Result(new List<Results_Test>() { await Power_Test() }, module_parameters);
            result1.Show();
        }

        private async void StartTest_Click(object sender, EventArgs e)
        {
            string serial_number = Convert.ToString(await open_input_form("Введите серийный номер платы", "серийный номер"));
            if (serial_number == "0" || serial_number == "серийный номер" || serial_number == "") return;
            Data_Transit.serial_number = int.Parse(serial_number);

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
                if (PortChanelB.Exchange || module_parameters.using_module.exchange_chanel < 2) break;
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
            Data_Transit.escape = false;

            List<Results_Test> result = new List<Results_Test>()
            {
                await Power_Test(),
            };
            await Task.Delay(2500);
            if (module_parameters.using_module.name == "PSC 24V 10A") result.Add(await PSC_Battary_Test());
            if (module_parameters.using_module.name == "PSC 24V 10A") result.Add(await PSC_Current_Test());
            if (module_parameters.using_module.tests["Проверка Din"] && !Data_Transit.escape)  result.Add(await All_TC_Test("din", "Проверка Din", module_parameters.using_module.din));
            if (module_parameters.using_module.tests.ContainsKey("Проверка light"))  if (module_parameters.using_module.tests["Проверка light"] && !Data_Transit.escape)  result.Add(await RTU7_Din_Test());
            if (module_parameters.using_module.tests["Проверка KF"] && !Data_Transit.escape) result.Add(await All_TC_Test("kf", "Проверка КФ", module_parameters.using_module.kf));
            if (module_parameters.using_module.tests["Проверка TC"] && !Data_Transit.escape) result.Add(await All_TC_Test("tc", "Проверка TC", module_parameters.using_module.tc));
            if (module_parameters.using_module.tests["Проверка 12B TC"] && !Data_Transit.escape) result.Add(await TC12V_Test());
            if (module_parameters.using_module.tests["Проверка температуры"] && !Data_Transit.escape) result.Add(await Temperature_Test());
            if (module_parameters.using_module.tests["Проверка ТУ"] && !Data_Transit.escape) result.Add(await TU_Test());
            if (module_parameters.using_module.tests["Проверка питания MTU5"] && !Data_Transit.escape) result.Add(await MTU_Power_Test());
            if (module_parameters.using_module.tests["Проверка ток 0"] && !Data_Transit.escape) result.Add(await PM7_Current_Test());
            if (module_parameters.using_module.tests["Проверка ТУ MTU5"] && !Data_Transit.escape) result.Add(await MTU5_TU_Test());
            if (module_parameters.using_module.tests["Проверка EnTU"] && !Data_Transit.escape) result.Add(await EnTU_Test());

            Result result1 = new Result(result, module_parameters);
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

        async Task<bool> Compar_Color(My_Button mb, Color comp)
        {
            int count = 0;
            while (count < 20)
            {
                await Task.Delay(100);
                if (mb.BackColor == comp) { return true; }
                count++;
            }
            return false;
        }

        async Task<Results_Test> MTU_Power_Test()
        {
            Results_Test result = new Results_Test("Проверка питания MTU5");
            result.test_result = true;

            List<My_Button> item = all_button.FindAll(x => x.name == "power");

            for (int a = 0; a < 2; a++)
            {
                result.Add_Test($"Питание MTU5");

                if (await Compar(item[a], 850, 1350))
                { result.Add_Item(item[a].Result, true); }
                else
                { result.Add_Item(item[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            }            

            return result;
        }

        async Task<Results_Test> PSC_Battary_Test()
        {
            Results_Test result = new Results_Test("Проверка зарядки батареи");
            result.test_result = true;

            List<My_Button> item = all_button.FindAll(x => x.name == "PowerPSC");
            List<My_Button> bat = all_button.FindAll(x => x.name == "BatteryPSC");
            List<float> curr = new List<float>();

            for (int a = 1; a < item.Count; a++)
            {
                result.Add_Test($"Напряжение канал {a}: ");
                if (await Compar(item[a], 22, 26))
                { result.Add_Item(item[a].Result, true); }
                else
                { result.Add_Item(item[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            }

            item[0].set_button(); await Task.Delay(1000);

            result.Add_Test($"Напряжение батареи: ");
            if (await Compar(item[0], 8, 26))
            { result.Add_Item(item[0].Result, true); }
            else
            { result.Add_Item(item[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            for (int a = 0; a < 20; a++)
            {
                curr.Add(bat[1].Result);
                await Task.Delay(1000);
            }

            result.Add_Test($"Ток заряда батареи"); //0x01, 0x06, 00, 0x63, 00, 0x01
            result.Add_Item(curr[0], true);

            result.Add_Test($"Ток заряда батареи через 20 секунд");
            if ((curr[curr.Count - 1] - curr[curr.Count - 1] / 10) > curr[0]) result.Add_Item(curr[curr.Count - 1], true);
            else result.Add_Item(curr[curr.Count-1], false);

            item[0].Reset(); await Task.Delay(250);

            return result;
        }

        async Task<Results_Test> PSC_Current_Test()
        {
            Results_Test result = new Results_Test("Проверка тока потребления");
            result.test_result = true;

            List<My_Button> item = all_button.FindAll(x => x.name == "ExitPSC");
            //List<My_Button> bat = all_button.FindAll(x => x.name == "BatteryPSC");

            for (int a = 2; a < item.Count; a++)
            {
                result.Add_Test($"Напряжение канал{a-1}: ");

                if (await Compar(item[a], 22, 26))
                { result.Add_Item(item[a].Result, true); }
                else
                { result.Add_Item(item[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            }

            for (int a = 0; a < 2; a++)
            {
                result.Add_Test($"Ток отребления на выходе, без нагрузки, канал {a + 1}: ");
                if (await Compar(item[a], 0, 0.01f))
                { result.Add_Item(item[a].Result, true); }
                else
                { result.Add_Item(item[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            }

            PortControl.Interrupt(new byte[] { module_parameters.v12.Addres, 0x06, 00, 0x63, 00, 0x01 }); await Task.Delay(500);

            for (int a = 0; a < 2; a++)
            {
                result.Add_Test($"Ток отребления на выходе, с нагрузкой, канал {a + 1}: ");
                if (await Compar(item[a], 0.08f, 0.15f))
                { result.Add_Item(item[a].Result, true); }
                else
                { result.Add_Item(item[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            }

            PortControl.Interrupt(new byte[] { module_parameters.v12.Addres, 0x06, 00, 0x63, 00, 0x00 }); await Task.Delay(500);

            return result;
        }

        async Task<Results_Test> MTU5_TU_Test()
        {
            Results_Test result = new Results_Test("Проверка ТУ MTU5");
            result.test_result = true;

            List<float> TU1 = new List<float>();
            List<float> TU2 = new List<float>();

            List<My_Button> item = all_button.FindAll(x => x.name == "mtu5tu");

            for (int a = 0; a < 25; a++)
            {
                TU1.Add(item[0].Result);
                TU2.Add(item[1].Result);
                await Task.Delay(200);
            }

            result.Add_Test("ТУ ON/OFF");
            if (TU1.Max() < 20f)
                { result.Add_Item(TU1.Max(), true); }
            else
                { result.Add_Item(TU1.Max(), false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
            result.Add_Test("ТУ RF");
            if (TU2.Max() < 20)
                { result.Add_Item(TU2.Max(), true); }
            else
                { result.Add_Item(TU2.Max(), false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> PM7_Current_Test()
        {
            Results_Test result = new Results_Test("Проверка ток 0");
            result.test_result = true;            
            float w = 0;
            float wo = 0;

            My_Button item = all_button.Find(x => x.name == "temperature");

            result.Add_Test($"Ток без нагрузки");
            for (int a = 0; a < 10; a++) { wo += item.Result; await Task.Delay(250); }
            PortControl.Interrupt(new byte[] { module_parameters.v12.Addres, 0x06, 00, 0x63, 00, 0x01 });
            await Task.Delay(2500);
            wo /= 10;
            result.Add_Item(wo, true);
            result.Add_Test($"Ток с нагрузкой");
            for (int a = 0; a < 10; a++) { w += item.Result; await Task.Delay(250); }
            PortControl.Interrupt(new byte[] { module_parameters.v12.Addres, 0x06, 00, 0x63, 00, 0x00 });
            await Task.Delay(250);
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

            My_Button item = all_button.FindLast(x => x.name == "temperature");
            result.Add_Test($"Температура");

            if (await Compar(item, 20, 40))
            { result.Add_Item(item.Result, true); }
            else
            { result.Add_Item(item.Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> EnTU_Test()
        {
            Results_Test result = new Results_Test("Проверка EnTU");
            result.test_result = true;

            My_Button item = all_button.Find(x => x.name == "Entu модуля");

            result.Add_Test($"EnTU");

            if (item.BackColor == Color.Red)
            { result.Add_Item(0, true); }
            else
            { result.Add_Item(0, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

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

        async Task<Results_Test> RTU7_Din_Test()
        {
            Results_Test result = new Results_Test("Проверка Din");
            result.test_result = true;

            List<My_Button> item = all_button.FindAll(x => x.name == "dinlight" && x.Visible);

            foreach (My_Button mb in item) { mb.Reset(); await Task.Delay(200); }

            for (int kf = 0; kf < item.Count; kf++)
            {
                await Task.Delay(250);
                item[kf].click.send_data(1); await Task.Delay(200);

                result.Add_Test($"Din {kf + 1}");
                for (int check = 0; check < item.Count; check++)
                {
                    if (Data_Transit.escape) break;
                    if (check == kf)
                    {
                        if (await Compar_Color(item[check], Color.Red))
                        { result.Add_Item(0, true); }
                        else
                        { result.Add_Item(0, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
                    }
                    else
                    {
                        if (await Compar_Color(item[check], Color.LightGray))
                        { result.Add_Item(0, true); }
                        else
                        { result.Add_Item(0, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
                    }
                }
                item[kf].Reset(); await Task.Delay(100);
                if (Data_Transit.escape)
                {
                    return result;
                }
            }
            await Task.Delay(250);
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
                using_button[kf].set_button(); await Task.Delay(200);

                result.Add_Test($"{test} {kf + 1}");
                for (int check = 0; check < using_button.Count; check++)
                {
                    if (Data_Transit.escape) break;
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
                using_button[kf].Reset();
                while (using_button[kf].BackColor == Color.Red) { await Task.Delay(100); }
                await Task.Delay(100);
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

            List<My_Button> using_button = all_button.FindAll(x => x.name == "ТУ модуля" && x.Visible);
            My_Button item = all_button.Find(x => x.name == "TC TU");

            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 1, 0, 1 }); await Task.Delay(1000);

            foreach (My_Button mb in using_button) { mb.Reset(); await Task.Delay(250); }
            await Task.Delay(1000);

            for (int kf = 0; kf < using_button.Count; kf++)
            {
                using_button[kf].set_button(); await Task.Delay(1000);
                result.Add_Test($"Тест ТУ {kf + 1}");

                if (await Compar(item, 200, 250))
                { result.Add_Item(item.Result, true); }
                else
                { result.Add_Item(item.Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }


                using_button[kf].Reset();
                while (using_button[kf].BackColor == Color.Red) { await Task.Delay(100); }
                await Task.Delay(1000);
                if (Data_Transit.escape)
                {
                    PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
                    await Task.Delay(200);
                    return result;
                }
            }
            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); await Task.Delay(200);
            return result;
        }

        #endregion

        private void all_module_buttons()
        {
            using (StreamReader sw = new StreamReader(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt"))
                setup = JsonConvert.DeserializeObject<List<Module_Setup>>(sw.ReadToEnd());
            setup.Sort((x, y) => y.name.CompareTo(x.name));

            PnlModule.Height = setup.Count * 35;
            foreach (Module_Setup ms in setup)
                PnlModule.Controls.Add(new Module_Button(ms.name, new EventHandler(Module_Selection)));
        }

        private async Task<string> open_input_form(string label_text, string textbox_text)
        {
            Input input = new Input(label_text, textbox_text);
            Data_Transit.shift_is_down = false;
            this.Enabled = false;
            string output_string = Convert.ToString(await input.return_result());
            this.Enabled = true;
            input.Dispose();
            return output_string;
        }

        private async void MyTest()
        {
            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 1, 0, 1 });
            await Task.Delay(10000);
            PortControl.Interrupt(new byte[] { module_parameters.dout_control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
        }

        private void ToolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void ToolTip1_Popup_1(object sender, PopupEventArgs e)
        {
            //toolTip1.SetToolTip(panel5, "hello");
        }
    }
}
