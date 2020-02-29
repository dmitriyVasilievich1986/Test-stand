using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Support_Class;

namespace test_stand
{
    public partial class Module_Setup_Form : Form
    {
        Module_Parameters mp;

        public class my_text_box :TextBox
        {
            public string my_name;

            public my_text_box(byte[] data, string register_name)
            {
                my_name = register_name;
                Text = "";
                foreach (byte b in data)
                { Text += b.ToString("X2") + " "; }
                TextAlign = HorizontalAlignment.Center;
                Click += (s, e) => { Clear(); };
            }

            public byte[] string_to_byte()
            {
                byte[] output = new byte[6];
                for (int a = 0; a < 6; a++)
                { output[a] = Convert.ToByte(Text.Split()[a], 16); }

                return output;
            }
        }

        public class my_combobox: ComboBox
        {
            public string my_name;

            public my_combobox(string name_text, bool using_text)
            {
                my_name = name_text;
                Items.AddRange(new string[] { "True", "False"});
                Text = using_text ? "True" : "False";
            }

            public bool return_value()
            {
                return Text == "True" ? true : false;
            }
        }

        public class my_panel_with_textbox : Panel
        {
            public my_panel_with_textbox()
            {
                Dock = DockStyle.Fill;
                Width = 250;
            }

            public void add_my_textbox(List<my_text_box> my_tb)
            {
                for (int x = 0; x < my_tb.Count; x++)
                {
                    Controls.Add(my_tb[x]);
                    Controls[Controls.Count - 1].Top = x * 35 + 100;
                    Controls[Controls.Count - 1].Left = 200;
                    Controls[Controls.Count - 1].Width = 250;

                    Controls.Add(new Label());
                    Controls[Controls.Count - 1].Top = x * 35 + 102;
                    Controls[Controls.Count - 1].Left = 100;
                    Controls[Controls.Count - 1].Text = my_tb[x].my_name;
                }
            }

            public void add_my_combobox(List<my_combobox> my_cb)
            {
                for (int x = 0; x < my_cb.Count; x++)
                {
                    Controls.Add(my_cb[x]);
                    Controls[Controls.Count - 1].Top = x * 35 + 100;
                    Controls[Controls.Count - 1].Left = 700;
                    Controls[Controls.Count - 1].Width = 250;

                    Controls.Add(new Label());
                    Controls[Controls.Count - 1].Top = x * 35 + 104;
                    Controls[Controls.Count - 1].Left = 500;
                    Controls[Controls.Count - 1].Width = 250;
                    Controls[Controls.Count - 1].Text = my_cb[x].my_name;
                }
            }
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

        void initial()
        {

        }

        List<string> admin = new List<string>();
        bool cntrl_pressed = false;

        public Module_Setup_Form(Module_Parameters module_parameters, List<Module_Setup> setup)
        {
            mp = module_parameters;
            InitializeComponent();
            label2.Text += mp.using_module.name;

            this.KeyDown += async (s, e) =>
            {
                switch (e.KeyCode)
                {
                    case Keys.ControlKey:
                        cntrl_pressed = true;
                        break;
                    case Keys.N:
                        if(cntrl_pressed) admin.Add("n");
                        break;
                    case Keys.D:
                        if(cntrl_pressed) admin.Add("d");
                        break;
                    case Keys.L:
                        if(cntrl_pressed) admin.Add("l");
                        break;
                    case Keys.T:
                        if(cntrl_pressed) admin.Add("t");
                        break;
                    case Keys.E:
                        if (cntrl_pressed) admin.Add("e");
                        //if (String.Join("", admin.ToArray()) == "delete")
                        //{
                        //    Dictionary<string, byte[]> new_dict = new Dictionary<string, byte[]>();
                        //    foreach(string a in module_parameters.using_module.all_registers.Keys)
                        //    {
                        //        new_dict.Add(a, module_parameters.using_module.all_registers[a]);
                        //    }
                            
                        //    string new_position = Convert.ToString(await open_input_form("Введите название адреса для удаления", "название адреса"));

                        //    module_parameters.using_module.all_registers = new Dictionary<string, byte[]>();
                        //    //foreach(string a in new_dict.Keys)
                        //    //{
                        //    //    if (a != new_position) module_parameters.using_module.all_registers.Add(a, new_dict[a]);
                        //    //}                            
                        //}
                        break;
                    case Keys.W:
                        if (cntrl_pressed) admin.Add("w");
                        if(String.Join("", admin.ToArray()) == "new")
                        {
                            string new_position = Convert.ToString(await open_input_form("Введите название новой посылки адреса", "название адреса"));
                            foreach(Module_Setup ms in setup)
                                { if(!ms.all_registers.ContainsKey(new_position)) ms.all_registers.Add(new_position, new byte[6]); }
                            //module_parameters.using_module.all_registers.Add(new_position, new byte[6]);
                        }
                        break;
                    default:
                        admin.Clear();
                        break;
                }
            };

            this.KeyUp+= (s, e) =>
            {
                switch (e.KeyCode)
                {
                    case Keys.ControlKey:
                        cntrl_pressed = false;
                        admin.Clear();
                        break;
                }
            };



            List<my_text_box> my_tb = new List<my_text_box>();
            List<my_combobox> my_cb = new List<my_combobox>();
            foreach(string name_text in mp.using_module.all_registers.Keys)
            {
                my_tb.Add(new my_text_box(mp.using_module.all_registers[name_text], name_text));
            }
            foreach (string name_text in mp.using_module.tests.Keys)
            {
                my_cb.Add(new my_combobox(name_text, mp.using_module.tests[name_text]));
            }

            my_panel_with_textbox my_panel = new my_panel_with_textbox();
            my_panel.add_my_textbox(my_tb);
            my_panel.add_my_combobox(my_cb);
            Controls.Add(my_panel);

            FormClosing += (s, e) =>
            {
                foreach(my_text_box mtb in my_tb)
                {
                    setup.Find(x => x.name == mp.using_module.name).all_registers[mtb.my_name] = mtb.string_to_byte();
                }
                foreach (my_combobox mcb in my_cb)
                {
                    setup.Find(x => x.name == mp.using_module.name).tests[mcb.my_name] = mcb.return_value();
                }

                using (StreamWriter sw = new StreamWriter(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt", false, Encoding.UTF8))
                    sw.Write(JsonConvert.SerializeObject(setup));
                Dispose();
            };
        }
    }
}
