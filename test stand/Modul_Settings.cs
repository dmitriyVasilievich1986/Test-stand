using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    public partial class Modul_Settings : Form
    {
        Module_Parameters MP;
        List<Module_Setup> setup;

        public Modul_Settings(Module_Parameters module_parameters, List<Module_Setup> setup_list)
        {
            InitializeComponent();
            MP = module_parameters;
            setup = setup_list;
            Din.Text = $"{MP.using_module.din.Min} {MP.using_module.din.Max} {MP.using_module.din.None}";
            KF.Text = $"{MP.using_module.kf.Min} {MP.using_module.kf.Max} {MP.using_module.kf.None}";
            TC.Text = $"{MP.using_module.tc.Min} {MP.using_module.tc.Max} {MP.using_module.tc.None}";
            TC12V.Text = $"{MP.using_module.tc12v.Min} {MP.using_module.tc12v.Max} {MP.using_module.tc12v.None}";
            Current.Text = $"{MP.using_module.current.Min} {MP.using_module.current.Max} {MP.using_module.current.None}";
            RS485Ports.Text = $"{MP.using_module.exchange_chanel}";
            PowerPorts.Text = $"{MP.using_module.power_chanel}";
            ModuleAddres.Text = $"{MP.module.Addres}";
        }

        private void Parameters_Change(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            switch (((TextBox)sender).Name)
            {
                case "KF":
                    MP.using_module.kf.setup(KF.Text.Split());
                    break;
                case "TC":
                    MP.using_module.tc.setup(TC.Text.Split());
                    break;
                case "Din":
                    MP.using_module.din.setup(Din.Text.Split());
                    break;
                case "TC12V":
                    MP.using_module.tc12v.setup(TC12V.Text.Split());
                    break;
                case "ModuleAddres":
                    MP.module.Addres = byte.Parse(ModuleAddres.Text);
                    break;
                case "Current":
                    MP.using_module.current.setup(Current.Text.Split());
                    break;
                case "PowerPorts":
                    MP.using_module.power_chanel = int.Parse(PowerPorts.Text);
                    break;
                case "RS485Ports":
                    MP.using_module.exchange_chanel = int.Parse(RS485Ports.Text);
                    break;
            }

            setup[setup.FindIndex(x => x.name == MP.using_module.name)] = MP.using_module;
            using (StreamWriter sw = new StreamWriter(@"C:\Users\d.shcherbachenya\Desktop\projects\test stand\JSon\module_setup.txt", false, Encoding.UTF8))
                sw.Write(JsonConvert.SerializeObject(setup));
        }
    }
}
