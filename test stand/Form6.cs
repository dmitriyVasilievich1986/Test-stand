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
using ModBus_Library;

namespace test_stand
{
    public partial class Form6 : Form
    {
        List<string> Data_Update = new List<string>();
        public Form6(ModBus_Libra port)
        {
            InitializeComponent();

            FormClosing += (s, e) => { Dispose(); };

            port.Receive_Event += () =>
            {
                if (this.Visible)
                {
                    Data_Update.Add(port.Receive_Array);
                    while (Data_Update.Count > 250) Data_Update.RemoveAt(0);
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        PortCondition.Lines = Data_Update.ToArray();
                        PortCondition.SelectionStart = PortCondition.Text.Length;
                        PortCondition.ScrollToCaret();
                    }));
                }
            };
            port.Transmit_Event += () =>
            {
                if (this.Visible)
                {
                    Data_Update.Add(port.Transmit_Array);
                    while (Data_Update.Count > 250) Data_Update.RemoveAt(0);
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
