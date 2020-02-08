using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_stand
{
    public partial class Input : Form
    {
        bool escape_code = true;

        public Input(string label_text, string textbox_text)
        {
            InitializeComponent();

            label1.Text = label_text;
            textBox1.Text = textbox_text;

            textBox1.Click += (s, e) => { textBox1.Clear(); };
        }

        public async Task<string> return_result()
        {
            this.Show();
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) escape_code = false; };
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { textBox1.Clear(); escape_code = false; } };

            while (escape_code)
            {
                await Task.Delay(100);
            }

            return textBox1.Text;
        }
    }
}
