using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using ModBus_Library;

namespace test_stand
{
    public partial class Form2 : Form
    {
        ModBus_Libra PD;

        
        public Form2(ModBus_Libra pd)
        {
            PD = pd;
            InitializeComponent();
            CBoxPort.Items.AddRange(SerialPort.GetPortNames());
            CBoxPort.Text = PD.Port.PortName;
            TBoxBaudRate.Text = PD.Port.BaudRate.ToString();
            if (PD.Port.IsOpen) { BtnClose.Visible = true; BtnOpen.Visible = false; }
            else { BtnClose.Visible = false; BtnOpen.Visible = true; }
        }

        private async void OpenPort(object sender, EventArgs e)
        {
            await Task.Run(() => {
                if (PD.Port.IsOpen) PD.Close();
                else
                {
                    Exception Err = PD.Open();
                    if (Err != null)
                        { BeginInvoke((MethodInvoker)(() => { MessageBox.Show(Err.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error); })); }
                }
                BeginInvoke((MethodInvoker)(() =>
                {
                    if (PD.Port.IsOpen) { BtnClose.Visible = true; BtnOpen.Visible = false; }
                    else { BtnClose.Visible = false; BtnOpen.Visible = true; }
                }));                
            });            
        }

        private void CBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PD.Port.IsOpen) { PD.Port.Close(); PD.Name = CBoxPort.Text; OpenPort(null, null); }
            else PD.Name = CBoxPort.Text;
        }

        private void TBoxBaudRate_TextChanged(object sender, EventArgs e)
        {
            if (PD.Port.IsOpen) { PD.Port.Close(); PD.Speed = Convert.ToInt32(TBoxBaudRate.Text); OpenPort(null, null); }
            else PD.Speed = Convert.ToInt32(TBoxBaudRate.Text);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            await Task.Delay(250);
            BtnOpen.Click += new System.EventHandler(this.OpenPort);
            BtnClose.Click += new System.EventHandler(this.OpenPort);
            TBoxBaudRate.TextChanged += (s, a) => { PD.Speed = Convert.ToInt32(TBoxBaudRate.Text); };
            CBoxPort.TextChanged += (s, a) => { PD.Name = CBoxPort.Text; };
        }
    }
}
