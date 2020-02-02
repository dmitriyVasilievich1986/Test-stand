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
using Support_Class;
using System.IO;
using Newtonsoft.Json;

namespace test_stand
{
    public partial class Settings : Form
    {
        public Settings(Module_Parameters module_parameters)
        {
            InitializeComponent();
            Dout_Din16.Text = module_parameters.dout_din16.Addres.ToString();
            Dout_Din32.Text = module_parameters.dout_din32.Addres.ToString();
            Dout_Control.Text = module_parameters.dout_control.Addres.ToString();
            TBMTU5.Text = Data_Transit.v12.Addres.ToString();
            TBPSC.Text = Data_Transit.Current_PSC.Addres.ToString();

            On.Click += new System.EventHandler(this.Power_On);
            Off.Click += new System.EventHandler(this.Power_Off);
            Dout_Din16.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { module_parameters.dout_din16.Addres = Convert.ToByte(Dout_Din16.Text); JSon_Save(module_parameters); } };
            Dout_Din32.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { module_parameters.dout_din32.Addres = Convert.ToByte(Dout_Din32.Text); JSon_Save(module_parameters); } };
            Dout_Control.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { module_parameters.dout_control.Addres = Convert.ToByte(Dout_Control.Text); JSon_Save(module_parameters); } };
            TBPSC.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { module_parameters.current_psc.Addres = Convert.ToByte(TBPSC.Text); JSon_Save(module_parameters); } };
            TBMTU5.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { module_parameters.v12.Addres = Convert.ToByte(TBMTU5.Text); JSon_Save(module_parameters); } };
            CBCurrent_Check.TextChanged += (s, e) =>
            {
                if (CBCurrent_Check.Text == "Выключена") Data_Transit.Current_Check = false;
                else Data_Transit.Current_Check = true;
            };
        }

        void JSon_Save(Module_Parameters module_parameters)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Test\Test-stand\json\module.txt", false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(module_parameters));
            }
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
