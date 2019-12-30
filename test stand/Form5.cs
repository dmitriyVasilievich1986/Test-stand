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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            Serial_Number.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) Start_TestAsync(); };
            Serial_Number.MouseClick += (s, e) => { Serial_Number.Text = ""; };
        }

        private void Start_TestAsync()
        {
            if (Serial_Number.Text == "" || Serial_Number.Text == "Введите серийный номер")
                { MessageBox.Show("Введите номер", "", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Data_Transit.serial_number = Convert.ToInt32(Serial_Number.Text);
            this.Close();
        }
    }
}
