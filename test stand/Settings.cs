using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModBus_Library;

namespace test_stand
{
    public partial class Settings : Form
    {

        public Settings()
        {
            InitializeComponent();
            Dout1.Text = Data_Transit.Dout_Din16.Addres.ToString();
            Dout2.Text = Data_Transit.Dout_Control.Addres.ToString();
            TBMTU5.Text = Data_Transit.v12.Addres.ToString();
            TBPSC.Text = Data_Transit.Current_PSC.Addres.ToString();

            On.Click += new System.EventHandler(this.Power_On);
            Off.Click += new System.EventHandler(this.Power_Off);
            Dout1.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { Data_Transit.Dout_Din16.Addres = Convert.ToByte(Dout1.Text); } };
            Dout2.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { Data_Transit.Dout_Control.Addres = Convert.ToByte(Dout2.Text); } };
            TBPSC.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { Data_Transit.Current_PSC.Addres = Convert.ToByte(TBPSC.Text); } };
            TBMTU5.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { Data_Transit.v12.Addres = Convert.ToByte(TBMTU5.Text); } };
            CBCurrent_Check.TextChanged += (s, e) =>
            {
                if (CBCurrent_Check.Text == "Выключена") Data_Transit.Current_Check = false;
                else Data_Transit.Current_Check = true;
            };
        }

        private void Power_On(object sender, EventArgs e)
        {
            if (!Data_Transit.PortControl.Port.IsOpen) return;
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 01, 0, 1 });
        }

        private void Power_Off(object sender, EventArgs e)
        {
            if (!Data_Transit.PortControl.Port.IsOpen) return;
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.Dout_Control.Addres, 0x10, 0, 0x51, 00, 02, 04, 0, 00, 0, 0 });
        }
    }
}
