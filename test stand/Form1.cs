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
            InitializeComponent();//StartTest_Click

            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.P) { Controls_Click(BtnCurent1, null); }
                if (e.KeyCode == Keys.Z) { form6.Show(); }
                if (e.KeyCode == Keys.T) { MyTest(); }
                if (e.KeyCode == Keys.Escape) { if (Active_Form != null) { Active_Form.Close(); Active_Form = null; PnlMain.Visible = true; Open_Window = "form1"; } }
                if (e.KeyCode == Keys.A) { BtnAllComPort_Click(null, null); }
                if (e.KeyCode == Keys.M) { Open_Child_Form(new Modul_Settings()); Open_Window = "form3"; }
                if (e.KeyCode == Keys.D1) { BtnComPortMenu.PerformClick(); }
                if (e.KeyCode == Keys.D2) { BtnModule.PerformClick(); }
                if (e.KeyCode == Keys.D3) { BtnParameters.PerformClick(); }
                if (e.KeyCode == Keys.D4) { StartTest.PerformClick(); }
                if (e.KeyCode == Keys.Space) { StartTest_Click(null, null); }
                if (e.KeyCode == Keys.S) { Open_Child_Form(new Settings()); Open_Window = "form4"; }
                if (e.KeyCode == Keys.D) { Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); }
            };

            this.StartPosition = FormStartPosition.CenterScreen;

            short Din_Start = 0x5f;
            byte start = 6;
            foreach (Button a in PnlKF.Controls)
            {
                Data_Transit.Controls.Add(a.Name.ToLower(), new All_Controls(a, Data_Transit.Dout_Control, 0x06, Din_Start--, start--, 0));
            }
            Din_Start = 0x57;
            start = 14;
            foreach (Button a in Pnl_TC.Controls)
            {
                Data_Transit.Controls.Add(a.Name.ToLower(), new All_Controls(a, Data_Transit.Dout_Control, 0x06, Din_Start--, start--, 0));
            }
            Din_Start = 0x58;
            start = 15;
            foreach (Button a in PnlDin8.Controls)
            {
                Data_Transit.Controls.Add(a.Name.ToLower(), new All_Controls(a, Data_Transit.Dout_Din16, 0x06, Din_Start--, start--, 0));
            }
            Din_Start = 0x5c;
            start = 3;
            foreach (Button a in PnlDin12.Controls)
            {
                Data_Transit.Controls.Add(a.Name.ToLower(), new All_Controls(a, Data_Transit.Dout_Din16, 0x06, Din_Start--, start--, 0));
            }
            Din_Start = 0x60;
            start = 7;
            foreach (Button a in PnlDin16.Controls)
            {
                Data_Transit.Controls.Add(a.Name.ToLower(), new All_Controls(a, Data_Transit.Dout_Din16, 0x06, Din_Start--, start--, 0));
            }
            Data_Transit.Controls.Add(BtnCurent1.Name.ToLower(), new All_Controls(BtnCurent1, Data_Transit.Current_PSC, 0x04, 0x010a, 0, 0x0004));
            Data_Transit.Controls.Add(BtnCurent2.Name.ToLower(), new All_Controls(BtnCurent2, Data_Transit.Current_PSC, 0x04, 0x010a, 1, 0x0004));
            Data_Transit.Controls.Add(Btn12V1.Name.ToLower(), new All_Controls(Btn12V1, Data_Transit.v12, 0x04, 0x0108, 1, 0x0004));
            Data_Transit.Controls.Add(Btn12V2.Name.ToLower(), new All_Controls(Btn12V2, Data_Transit.v12, 0x04, 0x0108, 0, 0x0004));

            Data_Transit.Registers_Controls.Add(new Data_Transit.Send(Data_Transit.Current_PSC, 0x04, 0x010a, 0x0004));
            Data_Transit.Registers_Controls.Add(new Data_Transit.Send(Data_Transit.Dout_Control, 0x02, 0x0001, 0x0010));
            Data_Transit.Registers_Controls.Add(new Data_Transit.Send(Data_Transit.Dout_Din16, 0x02, 0x0001, 0x0010));
            Data_Transit.Registers_Controls.Add(new Data_Transit.Send(Data_Transit.v12, 0x04, 0x0108, 0x0004));



            Data_Transit.PortControl.Receive_Event += PortControl_DataReceived;
            Data_Transit.PortChanelA.Receive_Event += PortChanelA_DataReceived;

            form6.Owner = this;

            PnlComPort.Visible = false;
            PnlModule.Visible = false;
            PnlTests.Visible = false;
            PnlParameters.Visible = false;
            PBMaximizeWindow.Visible = true;
            PBMinimizeWindow.Visible = false;


            new Thread(() =>
            {
                while (cycle)
                {
                    if (Data_Transit.PortControl.Port.IsOpen)
                    {
                        for (int a = 0; a < Data_Transit.Registers_Controls.Count; a++) 
                        {
                            Data_Transit.Registers_Controls[a].Data_Send(); System.Threading.Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        Data_Transit.PortControl.Exchange = false;
                        System.Threading.Thread.Sleep(200);
                    }
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
                            if (send[1] != 0) Data_Transit.PortChanelA.Transmit(send); System.Threading.Thread.Sleep(100);
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
                    if (Data_Transit.PortChanelB.Port.IsOpen && Data_Transit.Name != "Nomodule")
                    {
                        Data_Transit.PortChanelB.Transmit(new byte[] { Data_Transit.Module, 2, 0, 1, 0, 1 }); System.Threading.Thread.Sleep(200);
                    }
                    else
                    {
                        Data_Transit.PortChanelB.Exchange = false;
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }).Start();// Chanel B
        }

        #region Serial Port

        public void PortControl_DataReceived()
        {
            BeginInvoke((MethodInvoker)(() =>
            {                                
                foreach (string a in Data_Transit.Controls.Keys)
                {
                    if (Data_Transit.PortControl.Data_Receive[1] == 0x02)
                    {
                        if (Data_Transit.Dout_Control.Addres == Data_Transit.PortControl.Data_Receive[0] &&
                            (Data_Transit.PortControl.Data_Receive[4] & 15) != 0) BtnCurent1.BackColor = Color.Red;
                        else if (Data_Transit.Dout_Control.Addres == Data_Transit.PortControl.Data_Receive[0] &&
                            (Data_Transit.PortControl.Data_Receive[4] & 15) == 0) BtnCurent1.BackColor = Color.FromArgb(106, 27, 154);
                        if (Data_Transit.Controls[a].Dout.Addres == Data_Transit.PortControl.Data_Receive[0])
                        {
                            Data_Transit.Controls[a].Checkout((short)((short)(Data_Transit.PortControl.Data_Receive[3] << 8) |
                                (short)Data_Transit.PortControl.Data_Receive[4]));
                        }
                    }
                    else if(Data_Transit.PortControl.Data_Receive[1] == 0x04)
                    {
                        if (Data_Transit.Controls[a].Dout.Addres == Data_Transit.PortControl.Data_Receive[0] &&
                            Data_Transit.Controls[a].Addres == (short)((short)(Data_Transit.PortControl.Data_Transmit[2] << 8) | (short)Data_Transit.PortControl.Data_Transmit[3]))
                        {
                            Data_Transit.Controls[a].Result = Data_Transit.PortControl.Result[Data_Transit.Controls[a].Position];
                        }
                    }
                }
            }));
        }

        public void PortChanelA_DataReceived()
        {
            foreach (string rec in Data_Transit.Registers_Module.Keys)
            {
                if (Data_Transit.PortChanelA.Data_Receive[0] == Data_Transit.Registers_Module[rec][0]
                    && Data_Transit.PortChanelA.Data_Transmit[2] == Data_Transit.Registers_Module[rec][2]
                    && Data_Transit.PortChanelA.Data_Transmit[3] == Data_Transit.Registers_Module[rec][3]) 
                {
                    if (Data_Transit.PortChanelA.Data_Receive[1] == 0x04)
                    { for (int x = 0; x < Data_Transit.PortChanelA.Result.Length - 1; x++)  Data_Transit.Results[rec][x] = Data_Transit.PortChanelA.Result[x]; }
                    else if (Data_Transit.PortChanelA.Data_Receive[1] == 0x02)
                    {
                        if (rec == "entu") 
                        {
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                if ((Data_Transit.PortChanelA.Data_Receive[3] & 1) != 0) { panel12.BackColor = Color.Red; Data_Transit.EnTU = true; }
                                else { panel12.BackColor = Color.LightGray; Data_Transit.EnTU = false; }
                            }));
                        }
                        if (rec == "tu")
                        {
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                if ((Data_Transit.PortChanelA.Data_Receive[3] & 1) != 0) TU1.BackColor = Color.Red;
                                else TU1.BackColor = Color.LightGray;
                                if ((Data_Transit.PortChanelA.Data_Receive[3] & 2) != 0) TU2.BackColor = Color.Red;
                                else TU2.BackColor = Color.LightGray;
                                if ((Data_Transit.PortChanelA.Data_Receive[3] & 4) != 0) TU3.BackColor = Color.Red;
                                else TU3.BackColor = Color.LightGray;
                            }));
                        }
                    }
                }
            }  

            BeginInvoke((MethodInvoker)(() =>
            {
                int a = 8;
                foreach(Button Btn in PnlDin8.Controls)
                {
                    Btn.Text = $"Din{a.ToString()}: {Data_Transit.Results["din"][a-1]:0.0}";
                    a--;
                }//din

                a = 2;
                foreach (Button Btn in PnlKF.Controls)
                {
                    Btn.Text = $"KF{Convert.ToString((char)(65 + a))}: {Data_Transit.Results["kf"][a]:0.0}";
                    a--;
                }//kf

                a = 2;
                foreach (Button Btn in Pnl_TC.Controls)
                {
                    Btn.Text = $"KF{Convert.ToString((char)(65 + a))}: {Data_Transit.Results["tc"][a]:0.0}";
                    a--;
                }//tc

                PWR_1_MTU5.Text = $"Power U1: {Data_Transit.Results["power"][0]:0.0}";
                PWR_2_MTU5.Text = $"Power U2: {Data_Transit.Results["power"][1]:0.0}";
                
                if (Data_Transit.Name == "MTU5")
                {
                    TU1.Text = $"TU On/OFF: {Data_Transit.Results["mtutu"][0]:0}";                    
                    TU2.Text = $"TU RF: {Data_Transit.Results["mtutu"][1]:0}";
                    if (Data_Transit.Results["mtutu"][0] > 20 || Data_Transit.Results["mtutu"][1] > 20) panel8.BackColor = Color.Lime;
                    else panel8.BackColor = Color.FromArgb(106, 27, 154);
                }//tu numbers
                else
                {
                    TU1.Text = "TU On";
                    TU2.Text = "TU Off";
                }

                if (Data_Transit.Name == "PM7")
                { PWR_1_MTU5.Text = $"Temperature: {Data_Transit.Results["temperature"][0]:0.0}"; }//temperature
            }));
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
                }
            }

            //if (Data_Transit.Registers_Module["din"][5] > 0 || Data_Transit.Name == "Nomodule") PnlDin.Visible = true;
            //else PnlDin.Visible = false;
            //if (Data_Transit.Registers_Module["din"][5] > 24 || Data_Transit.Name == "Nomodule") PnlDin12.Visible = true;
            //else PnlDin12.Visible = false;
            //if (Data_Transit.Registers_Module["din"][5] > 32 || Data_Transit.Name == "Nomodule") PnlDin16.Visible = true;
            //else PnlDin16.Visible = false;
            //if (Data_Transit.Registers_Module["din32"][5] > 0 || Data_Transit.Name == "Nomodule") PnlDin32.Visible = true;
            //else PnlDin32.Visible = false;
            //if (Data_Transit.Registers_Module["kf"][5] > 0 || Data_Transit.Name == "Nomodule") PnlKF.Visible = true;
            //else PnlKF.Visible = false;
            //if (Data_Transit.Registers_Module["tc"][5] > 0 || Data_Transit.Name == "Nomodule") Pnl_TC.Visible = true;
            //else Pnl_TC.Visible = false;
            //if (Data_Transit.Registers_Module["temperature"][5] > 0 || Data_Transit.Name == "Nomodule") Pnl_Temperature.Visible = true;
            //else Pnl_Temperature.Visible = false;
            //if (Data_Transit.Registers_Module["tu"][5] > 0 || Data_Transit.Name == "Nomodule") PnlTU.Visible = true;
            //else PnlTU.Visible = false;
            //if (Data_Transit.Registers_Module["tu"][5] >= 3 || Data_Transit.Name == "Nomodule") PnlTURF.Visible = true;
            //else PnlTURF.Visible = false;
            //if (Data_Transit.Registers_Module["power"][5] >= 3 || Data_Transit.Name == "Nomodule") PnlPowerMTU5.Visible = true;
            //else PnlPowerMTU5.Visible = false;
            foreach (string rec in Data_Transit.Registers_Module.Keys) Data_Transit.Registers_Module[rec][0] = Data_Transit.Module;

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
            Data_Transit.Controls[((Button)sender).Name.ToLower()].Data_Transmit();
        }

        public void Power_Control(object sender, EventArgs e)
        {
            byte set = 1;
            if (((Button)sender).BackColor == Color.Red) set = 0;
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x59, 00, 04, 08, 0, set, 0, set, 0, set, 0, set });
        }

        private async void Tests_Click(object sender, EventArgs e)
        {
            string condition = "";
            string parameters = "";
            byte addres = 0;

            //switch (((Button)sender).Name)
            //{
            //    case "BtnTests1":
            //        condition = "Проверка КФ";
            //        parameters = "kf";
            //        addres = Data_Transit.DoutControl;
            //        break;
            //    case "BtnTests2":
            //        condition = "Проверка ТС";
            //        addres = Data_Transit.DoutControl;
            //        parameters = "tc";
            //        break;
            //    case "BtnTests3":
            //        condition = "Проверка Din8";
            //        addres = Data_Transit.Dout_Din16;
            //        parameters = "din";
            //        break;
            //}
            //if (Data_Transit.Registers_Module[parameters][5] == 0) { return; }
            //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x10, 0, 0x51, 00, 02, 04, 0, 01, 0, 1 }); // подача 220
            //if (await All_Tests.KF_TC_Test(parameters,condition,addres) == 0)
            //    MessageBox.Show("Успешно", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //else
            //    MessageBox.Show("Возникли ошибки", "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x10, 0, 0x51, 00, 02, 04, 0, 0, 0, 0 }); 
        }

        private async void BtnTests4_Click(object sender, EventArgs e)
        {
            if (await All_Tests.Power_Test() == 0)
                MessageBox.Show("Успешно", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Возникли ошибки", "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void StartTest_Click(object sender, EventArgs e)
        {
            int norm = 0;
            while (norm < 5)
            {
                await Task.Delay(50);
                norm++;
                if (Data_Transit.PortControl.Exchange) break;
            }
            if (norm >= 5) { MessageBox.Show("Нет обмена по каналу управления", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            norm = 0;
            while (norm < 5)
            {
                await Task.Delay(50);
                norm++;
                if (Data_Transit.PortChanelA.Exchange) break;
            }
            if (norm >= 5) { MessageBox.Show("Нет обмена по каналу А модуля", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            norm = 0;
            while (norm < 5)
            {
                await Task.Delay(50);
                norm++;
                if (Data_Transit.PortChanelB.Exchange) break;
            }
            if (norm >= 5) { MessageBox.Show("Нет обмена по каналу B модуля", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

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
        }
        #endregion

        private async void MyTest()
        {
            
        }
    }
}
