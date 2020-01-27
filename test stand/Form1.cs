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

public enum Operation
{
    KF,
    TC,
    Din,    
    Temperature,    
    Power,
    Current,
    TUNumber,
    ENTU,
    TU,
    Power_Ports,
    U12
}

namespace test_stand
{
    public partial class FormMain : Form
    {   
        Form6 form6 = new Form6();
        
        bool cycle = true;

        public FormMain()
        {
            InitializeComponent();
            HW.Visible = false;
            btnHW.Visible = false;

            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.P) { Controls_Click(BtnCurent1, null); }
                if (e.KeyCode == Keys.Z && Data_Transit.shift_is_down)  { form6.Show(); }
                if (e.KeyCode == Keys.T && Data_Transit.shift_is_down)  { MyTest(); }
                if (e.KeyCode == Keys.Escape)
                {
                    Data_Transit.escape = true;
                    PnlComPort.Visible = false;
                    PnlModule.Visible = false;
                    PnlParameters.Visible = false;
                    PnlTests.Visible = false;
                    if (Active_Form != null) { Active_Form.Close(); Active_Form = null; PnlMain.Visible = true; Open_Window = "form1";  }
                    Data_Transit.serial_number = 0;
                    //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Din16.Addres, 0x10, 0, 0x51, 00, 16, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Din32.Addres, 0x10, 0, 0x51, 00, 16, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x5d, 00, 03, 06, 0, 0, 0, 0, 0, 0 });
                    //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x55, 00, 03, 06, 0, 0, 0, 0, 0, 0 });
                    //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
                }
                if (e.KeyCode == Keys.A) { BtnAllComPort_Click(null, null); }
                if (e.KeyCode == Keys.M) { Open_Child_Form(new Modul_Settings()); Open_Window = "form3"; }
                if (e.KeyCode == Keys.D1 && Data_Transit.shift_is_down)
                {
                    byte set = 1;
                    var co = Data_Transit.port_control_button.Where(c => c.name.Contains("din"));
                    foreach(Button_Send a in co) if (a.button.BackColor == Color.Red) set = 0;
                    Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Din16.Addres, 0x10, 0, 0x51, 00, 16, 32, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set, 0, set });
                }
                else if (e.KeyCode == Keys.D1) { BtnComPortMenu.PerformClick(); }
                if (e.KeyCode == Keys.D2 && Data_Transit.shift_is_down)
                {
                    byte set = 1;
                    var co = Data_Transit.port_control_button.Where(c => c.name.Contains("kf"));
                    foreach (Button_Send a in co) if (a.button.BackColor == Color.Red) set = 0;
                    Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x5d, 00, 03, 06, 0, set, 0, set, 0, set });
                }
                else if (e.KeyCode == Keys.D2) { BtnModule.PerformClick(); }
                if (e.KeyCode == Keys.D3 && Data_Transit.shift_is_down)
                {
                    byte set = 1;
                    var co = Data_Transit.port_control_button.Where(c => c.name.Contains("tc"));
                    foreach (Button_Send a in co) if (a.button.BackColor == Color.Red) set = 0;
                    Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x55, 00, 03, 06, 0, set, 0, set, 0, set });
                }
                else if (e.KeyCode == Keys.D3) { BtnParameters.PerformClick(); }
                if (e.KeyCode == Keys.D4) { StartTest.PerformClick(); }
                if (e.KeyCode == Keys.ShiftKey) { Data_Transit.shift_is_down = true; }
                if (e.KeyCode == Keys.S && Data_Transit.shift_is_down)  { StartTest_Click(null, null); }
                else if (e.KeyCode == Keys.S) { Open_Child_Form(new Settings()); Open_Window = "form4"; }
                if (e.KeyCode == Keys.D) { Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); }
            };

            this.KeyUp += (s, e) =>
            {
                if (e.KeyCode == Keys.ShiftKey) { Data_Transit.shift_is_down = false; }
            };

            this.StartPosition = FormStartPosition.CenterScreen;

            for (int a = 2; a >= 0; a--)
            {
                Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Control, new byte[] { 0, 06, 0, (byte)(0x5d + a), 0, 0 }, (Button)PnlKF.Controls[a], Color.LightGray, "kf"));
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Control, 1 << (a + 4));
                Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlKF.Controls[a], 0, a, "kf"));
            }
            for (int a = 2; a >= 0; a--)
            {
                Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Control, new byte[] { 0, 06, 0, (byte)(0x55 + a), 0, 0 }, (Button)Pnl_TC.Controls[a], Color.LightGray, "tc"));
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Control, 1 << (a + 12));
                Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)Pnl_TC.Controls[a], 0, a, "tc"));
            }
            for (int a = 31; a > 23; a--)
            {
                PnlDin.Controls[a].Text = $"Din {32 - a} :";
                Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Din16, new byte[] { 0, 06, 0, (byte)(0x70 - a), 0, 0 }, (Button)PnlDin.Controls[a], Color.LightGray, "din16"));
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Din16, 1 << (39 - a));
                Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlDin.Controls[a], 0, 31 - a, "din16"));
            }
            for (int a = 23; a >= 16; a--)
            {
                PnlDin.Controls[a].Text = $"Din {32 - a} :";
                Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Din16, new byte[] { 0, 06, 0, (byte)(0x70 - a), 0, 0 }, (Button)PnlDin.Controls[a], Color.LightGray, "din16"));
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Din16, 1 << (23 - a));
                Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlDin.Controls[a], 0, 31 - a, "din16"));
            }
            for (int a = 15; a > 7; a--)
            {
                PnlDin.Controls[a].Text = $"Din {32 - a} :";
                Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Din32, new byte[] { 0, 06, 0, (byte)(0x60 - a), 0, 0 }, (Button)PnlDin.Controls[a], Color.LightGray, "din32"));
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Din32, 1 << (23 - a));
                Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlDin.Controls[a], 0, 15 - a, "din32"));
            }
            for (int a = 7; a >= 0; a--)
            {
                PnlDin.Controls[a].Text = $"Din {32 - a} :";
                Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Din32, new byte[] { 0, 06, 0, (byte)(0x60 - a), 0, 0 }, (Button)PnlDin.Controls[a], Color.LightGray, "din32"));
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Din32, 1 << (7 - a));
                Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlDin.Controls[a], 0, 15 - a, "din32"));
            }
            Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.module, null, btnHW, Color.LightGray, "hw"));
            Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Control, 15 << 8);
            Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortControl, Data_Transit.Dout_Control, new byte[] { 0, 0x10, 0, 0x5b, 00, 02, 04, 0, 0, 0, 0}, BtnCurent1, Color.Gray, "current")) ;
            Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0001, Data_Transit.PortControl, Data_Transit.Dout_Control, 12);
            Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, null, BtnEnTU, Color.LightGray, "entu"));
            Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(0x0009, Data_Transit.PortChanelA, Data_Transit.module, 1);
            Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, new byte[] { 0, 0x06, 0, 0x61, 0, 0 }, TU1, Color.LightGray, "tu_"));
            Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, new byte[] { 0, 0x06, 0, 0x62, 0, 0 }, TU2, Color.LightGray, "tu_"));
            Data_Transit.port_control_button.Add(new Button_Send(Data_Transit.PortChanelA, Data_Transit.module, new byte[] { 0, 0x06, 0, 0x63, 0, 0 }, TU3, Color.LightGray, "tu_"));
            

            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.Current_PSC, BtnCurent1, 0x010a, 0, "current"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.Current_PSC, BtnCurent2, 0x010a, 1, "current"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.v12, Btn12V1, 0x0108, 1, "12v"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.v12, Btn12V2, 0x0108, 0, "tu voltage"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortControl, Data_Transit.v12, Btn12V3, 0x0108, 2, "12v2"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, PWR_1_MTU5, 0, 0, "power"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, PWR_2_MTU5, 0, 1, "power"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, BtnTemperature, 0, 0, "temperature"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, TU1, 0, 0, "mtu_tu"));
            Data_Transit.all_button_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, TU2, 0, 1, "mtu_tu"));

            Data_Transit.controls_module.Add(new Controls_Only(TU1, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 0, "tu"));
            Data_Transit.controls_module.Add(new Controls_Only(TU2, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 1, "tu"));
            Data_Transit.controls_module.Add(new Controls_Only(TU3, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 2, "tu"));

            Data_Transit.controls_module.Add(new Controls_Only(TU1, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 0, "tu_control"));
            Data_Transit.controls_module.Add(new Controls_Only(TU2, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 1, "tu_control"));
            Data_Transit.controls_module.Add(new Controls_Only(TU3, Data_Transit.Dout_Control, Data_Transit.PortChanelA, 0x01, 2, "tu_control"));

           

            Data_Transit.port_control_send_data.Add(new Send_Only(Data_Transit.PortControl, Data_Transit.Dout_Control, new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }));
            Data_Transit.port_control_send_data.Add(new Send_Only(Data_Transit.PortControl, Data_Transit.Current_PSC, new byte[] { 0x00, 0x04, 0x01, 0x0a, 0x00, 0x04 }));
            Data_Transit.port_control_send_data.Add(new Send_Only(Data_Transit.PortControl, Data_Transit.Dout_Din16, new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }));
            Data_Transit.port_control_send_data.Add(new Send_Only(Data_Transit.PortControl, Data_Transit.Dout_Din32, new byte[] { 0x00, 0x02, 0x00, 0x01, 0x00, 0x10 }));
            Data_Transit.port_control_send_data.Add(new Send_Only(Data_Transit.PortControl, Data_Transit.v12, new byte[] { 0x00, 0x04, 0x01, 0x08, 0x00, 0x06 }));

            Data_Transit.PortControl.Receive_Event += PortControl_DataReceived;
            Data_Transit.PortChanelA.Receive_Event += PortChanelA_DataReceived;

            form6.Owner = this;

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
                    if (Data_Transit.PortControl.Port.IsOpen)
                        { foreach (Send_Only a in Data_Transit.port_control_send_data) { a.Trasmit_Data(); System.Threading.Thread.Sleep(100); } }
                    else
                        { Data_Transit.PortControl.Exchange = false; System.Threading.Thread.Sleep(200); }
                }
            }).Start();// Control

            new Thread(() =>
            {
                while (cycle)
                {
                    if (Data_Transit.PortChanelA.Port.IsOpen && Data_Transit.Name != "Nomodule")
                    {
                        foreach (byte[] send in Data_Transit.Registers_Module.Values)
                        {
                            if (send[1] != 0) { Data_Transit.PortChanelA.Transmit(send); System.Threading.Thread.Sleep(100); }
                        }
                    }
                    else
                    {
                        Data_Transit.PortChanelA.Exchange = false;
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }).Start();// Chanel A

            new Thread(() =>
            {
                while (cycle)
                {
                    if (Data_Transit.PortChanelB.Port.IsOpen && Data_Transit.Name != "Nomodule" && Data_Transit.exchange_port > 1)
                    {
                        Data_Transit.PortChanelB.Transmit(new byte[] { Data_Transit.module.Addres, 2, 0, 1, 0, 1 }); System.Threading.Thread.Sleep(200);
                    }
                    else
                    {
                        Data_Transit.PortChanelB.Exchange = false;
                        if (Data_Transit.exchange_port <= 1) Data_Transit.PortChanelB.Exchange = true;
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }).Start();// Chanel B
            
            #endregion
        }

        #region Serial Port

        public void PortControl_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() => {
                if (btnHW.BackColor == Color.Red) HW.Visible = true;
                else HW.Visible = false;
                foreach (Button_Send a in Data_Transit.port_control_button) a.Checkout(Data_Transit.PortControl);
                foreach (Button_Result a in Data_Transit.all_button_result) a.Checkout(Data_Transit.PortControl); }));
        }

        public void PortChanelA_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() => {
                foreach (Button_Send a in Data_Transit.port_control_button) a.Checkout(Data_Transit.PortChanelA);
                foreach (Button_Result a in Data_Transit.all_button_result) a.Checkout(Data_Transit.PortChanelA); }));
        }

        #endregion

        #region Module_Initialization

        public void Module_Selection(object sender, EventArgs e)
        {
            Data_Transit.ModuleName(((Button)sender).Text.Replace(" ", string.Empty));

            using (SqlConnection connection = new SqlConnection(Data_Transit.connectionString))
            {
                string sqlExpression = "SELECT * FROM [User].[dbo].[Module_Parameters] where module=@name";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter nameParam = new SqlParameter("@name", Data_Transit.Name);
                command.Parameters.Add(nameParam);
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();

                object[] x = new object[reader.FieldCount];
                reader.GetValues(x);

                for (int a = 0; a < 3; a++)
                {
                    Data_Transit.Module_Parameters["din"][a] = (int)x[a + 2];
                    Data_Transit.Module_Parameters["kf"][a] = (int)x[a + 5];
                    Data_Transit.Module_Parameters["tc"][a] = (int)x[a + 8];
                }
                Data_Transit.Current_Norm = Convert.ToSingle(x[11]);
                Data_Transit.port = Convert.ToInt16(x[12]);
                Data_Transit.exchange_port= Convert.ToInt16(x[14]);
                Data_Transit.Module_Parameters["din"][3] = (int)x[13];
                reader.Close();

                sqlExpression = "SELECT * FROM [User].[dbo].[Module_Registers] where name=@name";
                command = new SqlCommand(sqlExpression, connection);
                nameParam = new SqlParameter("@name", Data_Transit.Name);
                command.Parameters.Add(nameParam);
                reader = command.ExecuteReader();
                reader.Read();
                x = new object[reader.FieldCount];
                reader.GetValues(x);
                for (int a = 0; a < 6; a++)
                {
                    Data_Transit.Registers_Module["din"][a] = Convert.ToByte(x[a + 1]);
                    Data_Transit.Registers_Module["kf"][a] = Convert.ToByte(x[a + 7]);
                    Data_Transit.Registers_Module["tc"][a] = Convert.ToByte(x[a + 13]);
                    Data_Transit.Registers_Module["tu"][a] = Convert.ToByte(x[a + 19]);
                    Data_Transit.Registers_Module["entu"][a] = Convert.ToByte(x[a + 26]);
                    Data_Transit.Registers_Module["power"][a] = Convert.ToByte(x[a + 32]);
                    Data_Transit.Registers_Module["mtutu"][a] = Convert.ToByte(x[a + 38]);
                    Data_Transit.Registers_Module["temperature"][a] = Convert.ToByte(x[a + 44]);
                    Data_Transit.Registers_Module["din32"][a] = Convert.ToByte(x[a + 50]);
                }
                foreach(Button_Result a in Data_Transit.all_button_result)
                {
                    if (a.name == "kf") a.addres = Convert.ToInt16((Convert.ToInt16(x[9]) << 8) | (Convert.ToInt16(x[10])));
                    else if (a.name == "tc") a.addres = Convert.ToInt16((Convert.ToInt16(x[15]) << 8) | (Convert.ToInt16(x[16])));
                    else if (a.name == "din16") a.addres = Convert.ToInt16((Convert.ToInt16(x[3]) << 8) | (Convert.ToInt16(x[4])));
                    else if (a.name == "din32") a.addres = Convert.ToInt16((Convert.ToInt16(x[52]) << 8) | (Convert.ToInt16(x[53])));
                    else if (a.name == "power") a.addres = Convert.ToInt16((Convert.ToInt16(x[34]) << 8) | (Convert.ToInt16(x[35])));
                    else if (a.name == "temperature") a.addres = Convert.ToInt16((Convert.ToInt16(x[46]) << 8) | (Convert.ToInt16(x[47])));
                    else if (a.name == "mtu_tu") a.addres = Convert.ToInt16((Convert.ToInt16(x[40]) << 8) | (Convert.ToInt16(x[41])));
                }
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 1].Initialization(Convert.ToInt16((Convert.ToInt16(x[21]) << 8) | (Convert.ToInt16(x[22]))), Data_Transit.PortChanelA, Data_Transit.module, 4);
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 2].Initialization(Convert.ToInt16((Convert.ToInt16(x[21]) << 8) | (Convert.ToInt16(x[22]))), Data_Transit.PortChanelA, Data_Transit.module, 2);
                Data_Transit.port_control_button[Data_Transit.port_control_button.Count - 3].Initialization(Convert.ToInt16((Convert.ToInt16(x[21]) << 8) | (Convert.ToInt16(x[22]))), Data_Transit.PortChanelA, Data_Transit.module, 1);
                
                //for (int a = 2; a >= 0; a--)
                //    Data_Transit.all_button_module_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlKF.Controls[a], Convert.ToInt16((Convert.ToInt16(x[9]) << 8) | (Convert.ToInt16(x[10]))), a, "kf", $"KF{(char)(65+a)}: "));
                //for (int a = 2; a >= 0; a--)
                //    Data_Transit.all_button_module_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)Pnl_TC.Controls[a], Convert.ToInt16((Convert.ToInt16(x[15]) << 8) | (Convert.ToInt16(x[16]))), a, "tc", $"TC{(char)(65 + a)}: "));
                //for (int a = 15; a > 7; a--)
                //    Data_Transit.all_button_module_result.Add(new Button_Result(Data_Transit.PortChanelA, Data_Transit.module, (Button)PnlDin.Controls[a], Convert.ToInt16((Convert.ToInt16(x[3]) << 8) | (Convert.ToInt16(x[4]))), 15 - a, "din16", $"Din {16-a}: "));
            }
            
            var items = Data_Transit.port_control_button.Where(z => z.name.ToLower().Contains("din"));
            int count = 0;
            foreach(Button_Send BS in items)
            {
                if (Data_Transit.Module_Parameters["din"][3] > 0 || Data_Transit.Name == "Nomodule") PnlDin.Visible = true;
                else PnlDin.Visible = false;
                if (count >= Data_Transit.Module_Parameters["din"][3]) BS.button.Visible = false;
                else BS.button.Visible = true;
                count++;
            }
            if (Data_Transit.Registers_Module["kf"][5] > 0 || Data_Transit.Name == "Nomodule") PnlKF.Visible = true;
            else PnlKF.Visible = false;
            if (Data_Transit.Registers_Module["tc"][5] > 0 || Data_Transit.Name == "Nomodule") Pnl_TC.Visible = true;
            else Pnl_TC.Visible = false;
            if (Data_Transit.Registers_Module["temperature"][5] > 0 || Data_Transit.Name == "Nomodule") Pnl_Temperature.Visible = true;
            else Pnl_Temperature.Visible = false;
            if (Data_Transit.Registers_Module["tu"][5] > 0 || Data_Transit.Name == "Nomodule") PnlTU.Visible = true;
            else PnlTU.Visible = false;
            if (Data_Transit.Registers_Module["tu"][5] >= 3 || Data_Transit.Name == "Nomodule") PnlTURF.Visible = true;
            else PnlTURF.Visible = false;
            if (Data_Transit.Registers_Module["power"][5] >= 3 || Data_Transit.Name == "Nomodule") PnlPowerMTU5.Visible = true;
            else PnlPowerMTU5.Visible = false;
            foreach (string rec in Data_Transit.Registers_Module.Keys) Data_Transit.Registers_Module[rec][0] = Data_Transit.module.Addres;

            if (Data_Transit.Name == "PM7")
            {
                PnlPowerMTU5.Visible = true;
                PWR_2_MTU5.Visible = false;
            }
            else
            {
                PWR_2_MTU5.Visible = true;
            }

            if (Open_Window == "form3") Open_Child_Form(new Modul_Settings());
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
            Data_Transit.PortControl.Port.Close();
            Data_Transit.PortChanelA.Port.Close();
            Data_Transit.PortChanelB.Port.Close();
            Properties.Settings.Default.Port1 = Data_Transit.PortControl.Name;
            Properties.Settings.Default.Port2 = Data_Transit.PortChanelA.Name;
            Properties.Settings.Default.Port3 = Data_Transit.PortChanelB.Name;
            Properties.Settings.Default.Save();
            cycle = false;
            System.Threading.Thread.Sleep(150);
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
                    Open_Child_Form(new Form2(Data_Transit.PortControl)); break;
                case "BtnPort2":
                    Open_Child_Form(new Form2(Data_Transit.PortChanelA)); break;
                case "BtnPort3":
                    Open_Child_Form(new Form2(Data_Transit.PortChanelB)); break;
            }
        }

        private void Settings_Click(object sender, EventArgs e) { Open_Child_Form(new Settings()); Open_Window = "form4"; }

        private void Module_Settings_Click(object sender, EventArgs e) { Open_Child_Form(new Modul_Settings()); Open_Window = "form3"; }

        private async void BtnAllComPort_Click(object sender, EventArgs e)
        {
            await Task.Run(() => {
                Data_Transit.PortControl.Open();
                Data_Transit.PortChanelA.Open();
                Data_Transit.PortChanelB.Open();
            });
            if (!Data_Transit.PortControl.Port.IsOpen || !Data_Transit.PortChanelA.Port.IsOpen || !Data_Transit.PortChanelB.Port.IsOpen)
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
            string parameters = "";
            string test_name = "";

            switch(((Button)sender).Name)
            {
                case "BtnTests1":
                    parameters = "kf";
                    test_name = "Проверка КФ";
                    break;
                case "BtnTests2":
                    parameters = "tc";
                    test_name = "Проверка TC";
                    break;
                case "BtnTests3":
                    parameters = "din";
                    test_name = "Проверка Din";
                    break;
            }

            Data_Transit.escape = false;
            Result result1 = new Result(new List<Results_Test>() { await All_TC_Test(parameters, test_name) });
            if(!Data_Transit.escape) result1.Show();
        }

        private async void BtnTests4_Click(object sender, EventArgs e)
        {            
            Result result1 = new Result(new List<Results_Test>() { await Power_Test() });
            result1.Show();
        }

        private async void StartTest_Click(object sender, EventArgs e)
        {
            if (!Data_Transit.PortControl.Port.IsOpen || !Data_Transit.PortChanelA.Port.IsOpen || !Data_Transit.PortChanelB.Port.IsOpen)
                { MessageBox.Show("Не все порты открыты", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            int norm = 0;
            while (norm < 20)
            {
                await Task.Delay(50);
                norm++;
                if (Data_Transit.PortControl.Exchange) break;
            }
            if (norm >= 20) { MessageBox.Show("Нет обмена по каналу управления", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            while (norm < 20)
            {
                await Task.Delay(50);
                norm++;
                if (Data_Transit.PortChanelA.Exchange) break;
            }
            if (norm >= 20) { MessageBox.Show("Нет обмена по каналу А модуля", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            while (norm < 20)
            {
                await Task.Delay(50);
                norm++;
                if (Data_Transit.PortChanelB.Exchange) break;
            }
            if (norm >= 20) { MessageBox.Show("Нет обмена по каналу B модуля", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (Data_Transit.Results["current"][0] > Data_Transit.Current_Norm + .1)
            {
                MessageBox.Show("Повышен ток потребления блока", "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Data_Transit.Results["current"][0] < Data_Transit.Current_Norm - .1)
            {
                MessageBox.Show("Низкий ток потребления блока", "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
            if (Data_Transit.Test_Need("din") && !Data_Transit.escape)  result.Add(await All_TC_Test("din", "Проверка Din"));
            if (Data_Transit.Test_Need("kf") && !Data_Transit.escape) result.Add(await All_TC_Test("kf", "Проверка КФ"));
            if (Data_Transit.Test_Need("tc") && !Data_Transit.escape) result.Add(await All_TC_Test("tc", "Проверка TC"));
            if (Data_Transit.Test_Need("v12") && !Data_Transit.escape) result.Add(await TC12V_Test());
            if (Data_Transit.Test_Need("temperature") && !Data_Transit.escape) result.Add(await Temperature_Test());
            if (Data_Transit.Test_Need("tu") && !Data_Transit.escape) result.Add(await TU_Test());
            if (Data_Transit.Test_Need("mtu5_power") && !Data_Transit.escape) result.Add(await MTU_Power_Test());
            if (Data_Transit.Test_Need("PM7_Current") && !Data_Transit.escape) result.Add(await PM7_Current_Test());
            if (Data_Transit.Test_Need("MTU5_TU") && !Data_Transit.escape) result.Add(await MTU5_TU_Test());
            if (Data_Transit.Test_Need("entu") && !Data_Transit.escape) result.Add(await EnTU_Test());

            Result result1 = new Result(result);
            if (!Data_Transit.escape) result1.Show();
        }
        #endregion

        #region All_Tests

        async Task<bool> Compar(Button_Result BR, float minimum, float maximum)
        {
            int count = 0;
            while (count < 20)
            {
                await Task.Delay(100);
                if (BR.Result >= minimum && BR.Result <= maximum) { return true; }
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

                if (await Compar(item.ToList()[a], 850, 1350))
                    { result.Add_Item(item.ToList()[a].Result, true); }
                else
                    { result.Add_Item(item.ToList()[a].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
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

            if (await Compar(item.ToList()[0], 20, 40))
                { result.Add_Item(item.ToList()[0].Result, true); }
            else
                { result.Add_Item(item.ToList()[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> EnTU_Test()
        {
            Results_Test result = new Results_Test("Проверка EnTU");
            result.test_result = true;

            result.Add_Test($"EnTU");

            if (BtnEnTU.BackColor == Color.Red) 
            { result.Add_Item(0, true); }
            else
            { result.Add_Item(0, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> TC12V_Test()
        {
            Results_Test result = new Results_Test("Проверка 12B TC");
            result.test_result = true;

            var item = Data_Transit.all_button_result.Where(c => c.name.Contains("12v"));
            result.Add_Test($"Питание 12B");

            if (await Compar(item.ToList()[0], Data_Transit.TC_12V - 2, Data_Transit.TC_12V + 2))
                { result.Add_Item(item.ToList()[0].Result, true); }
            else
                { result.Add_Item(item.ToList()[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

            return result;
        }

        async Task<Results_Test> Power_Test()
        {
            Results_Test result = new Results_Test("Проверка питания");
            result.test_result = true;

            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x59, 00, 04, 08, 0, 0, 0, 0, 0, 0, 0, 0 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(500);

            var item = Data_Transit.all_button_result.Where(c => c.name.Contains("current"));

            for (int a = 0; a < Data_Transit.port; a++)
            {
                result.Add_Test($"Питание {a + 1} канала");
                Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x06, 0, (byte)(0x5c - a), 0, 1 });
                while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(1000);

                if (await Compar(item.ToList()[0], (float)(Data_Transit.Current_Norm - .01), (float)(Data_Transit.Current_Norm + .01)))
                { result.Add_Item(item.ToList()[0].Result, true); }
                else
                { result.Add_Item(item.ToList()[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

                Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x06, 0, (byte)(0x5C - a), 0, 0 });
                while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(500);
            }

            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x59, 00, 04, 08, 0, 0, 0, 0, 0, 1, 0, 1 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(500);

            return result;
        }

        async Task<Results_Test> All_TC_Test(string parameters, string test)
        {
            Results_Test result = new Results_Test(test);
            result.test_result = true;
            int PCount = Data_Transit.Name.ToLower() == "din32" ? 32 : Data_Transit.Registers_Module[parameters][5] / 2;

            var co = Data_Transit.port_control_button.Where(c => c.name.Contains(parameters));
            var cr = Data_Transit.all_button_result.Where(c => c.name.Contains(parameters));

            foreach (Button_Send a in co) { if (a.button.BackColor == Color.Red) { while (a.button.BackColor == Color.Red) { a.Reset(); while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200); } } }
            await Task.Delay(500);

            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 1, 0, 1 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);

             for (int kf = 0; kf < PCount; kf++)
            {
                co.ToList()[kf].Trasmit_Data(); while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
                result.Add_Test($"{test} {kf + 1}");
                for (int check = 0; check < PCount; check++)
                {                    
                    if (check == kf)
                    {
                        if (await Compar(cr.ToList()[check], Data_Transit.Module_Parameters[parameters][0], Data_Transit.Module_Parameters[parameters][1]))
                        { result.Add_Item(cr.ToList()[check].Result, true); }
                        else
                        { result.Add_Item(cr.ToList()[check].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
                    }
                    else
                    {
                        if (await Compar(cr.ToList()[check], 0, Data_Transit.Module_Parameters[parameters][2]))
                        { result.Add_Item(cr.ToList()[check].Result, true); }
                        else
                        { result.Add_Item(cr.ToList()[check].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }
                    }
                }
                while (co.ToList()[kf].button.BackColor == Color.Red) { co.ToList()[kf].Reset(); while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200); }
                if (Data_Transit.escape)
                {
                    Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
                    while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
                    return result;
                }
            }
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
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
                co.ToList()[kf].Trasmit_Data(); await Task.Delay(200); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(1000);
                result.Add_Test($"Тест ТУ {kf + 1}");

                if (await Compar(cr.ToList()[0], 200, 250))
                { result.Add_Item(cr.ToList()[0].Result, true); }
                else
                { result.Add_Item(cr.ToList()[0].Result, false); result.test_result = false; result.All_Tests[result.All_Tests.Count - 1].test_result = false; }

                await Task.Delay(1000);
                while (co.ToList()[kf].button.BackColor == Color.Red) { co.ToList()[kf].Reset(); while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(1000); }
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
            Result result1 = new Result(new List<Results_Test>() { await EnTU_Test() });
            result1.Show();
        }        
    }
}
