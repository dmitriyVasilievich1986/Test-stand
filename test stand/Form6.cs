using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace test_stand
{
    public partial class Form6 : Form
    {
        List<string> Data_Update = new List<string>();
        public Form6()
        {
            InitializeComponent();
            FormMain fm = this.Owner as FormMain;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            Data_Transit.PortChanelA.Receive_Event += ()=>
            {
                if(this.Visible)
                {
                    Data_Update.Add(Data_Transit.PortChanelA.Receive_Array);
                    while (Data_Update.Count > 50) Data_Update.RemoveAt(0);
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        PortCondition.Lines = Data_Update.ToArray();
                        PortCondition.SelectionStart = PortCondition.Text.Length;
                        PortCondition.ScrollToCaret();
                    }));
                }                
            };
            Data_Transit.PortChanelA.Transmit_Event += () =>
            {
                if(this.Visible)
                {
                    Data_Update.Add(Data_Transit.PortChanelA.Transmit_Array);
                    while (Data_Update.Count > 50) Data_Update.RemoveAt(0);
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        PortCondition.Lines = Data_Update.ToArray();
                        PortCondition.SelectionStart = PortCondition.Text.Length;
                        PortCondition.ScrollToCaret();
                    }));
                }
                
            };
        }
    }
}
