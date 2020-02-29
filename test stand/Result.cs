using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Support_Class;

namespace test_stand
{
    public partial class Result : Form
    {
        List<Results_Test> list_of_result;
        Module_Parameters mp;

        public Result(List<Results_Test> al, Module_Parameters module_parameters)
        {
            mp = module_parameters;
            FormClosing += (s, e) => { Dispose(); };
            int column_count = 0;
            list_of_result = al;
            foreach (Results_Test rt in al) { foreach(Tests te in rt.All_Tests) { if (te.test.Count > column_count) column_count = te.test.Count; }}
            if (column_count < 2) column_count = 2;

            InitializeComponent();
            if(Data_Transit.escape) { return; }
            button1.Visible = Data_Transit.serial_number > 0 ? true : false;

            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { this.Close(); } };

            DataTable DT = new DataTable("result");
            DataColumn DC = new DataColumn("Проверка");
            DC.DataType = typeof(string);
            
            DT.Columns.Add(DC);

            for (int a = 0; a < column_count; a++)
            {
                DC = new DataColumn($"Тест {a + 1}");
                DC.DataType = typeof(string);
                DT.Columns.Add(DC);
            }

            DataRow DR = DT.NewRow();
            DR[0] = DateTime.Now.ToString();
            DR[1] = mp.using_module.name;
            if (Data_Transit.serial_number > 0) DR[2] = Data_Transit.serial_number.ToString();
            DT.Rows.Add(DR);

            if (al.Count > 1) 
            {
                DR = DT.NewRow();
                DR[0] = "Полная проверка:";
                DR[1] = "успешно";
                foreach (Results_Test rt in al) if (!rt.test_result) { DR[1] = "произошли ошибки"; break; }
                DT.Rows.Add(DR);
            }            

            foreach (Results_Test rt in al)
            {
                DR = DT.NewRow();
                DR["Проверка"] = rt.name;
                DT.Rows.Add(DR);                

                foreach (Tests te in rt.All_Tests)
                {
                    DR = DT.NewRow();
                    DR[0] = te.name;
                    for (int col = 0; col < te.test.Count; col++) 
                        { DR[col + 1] = te.test[col].Item1.ToString(); }
                    DT.Rows.Add(DR);
                }
            }

            dataGridView1.DataSource = DT;
            Colorized();
        }

        private void Button1_Click(object sender, EventArgs e)
        {   
            Data_Transit.Insert_Module(mp);
            Data_Transit.Last_Row();
            foreach(Results_Test rt in list_of_result)
            {
                if (rt.name == "Проверка питания") 
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test,"current"); } }
                else if (rt.name == "Проверка КФ")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "kf"); } }
                else if (rt.name == "Проверка TC")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "tc"); } }
                else if (rt.name == "Проверка Din")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "din"); } }
                else if (rt.name == "Проверка 12B TC")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "tc12v"); } }
                else if (rt.name == "Проверка ТУ")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "tu"); } }
                else if (rt.name == "Проверка питания MTU5")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "MTU5_Power"); } }
                else if (rt.name == "Проверка ток 0")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "current0"); } }
                else if (rt.name == "Проверка температуры")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "temperature"); } }
                else if (rt.name == "Проверка ТУ MTU5")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "MTU5_TU"); } }
                else if (rt.name == "Проверка EnTU")
                    { foreach (Tests te in rt.All_Tests) { Data_Transit.Insert_Current(te.test, "entu"); } }
            }            
            this.Close();
        }

        async void Colorized()
        {
            await Task.Delay(100);
            if (list_of_result.Count > 1) 
            {
                dataGridView1.Rows[1].Cells[1].Style.BackColor = Color.Green;
                foreach (Results_Test rt in list_of_result) if (!rt.test_result) { dataGridView1.Rows[1].Cells[1].Style.BackColor = Color.Red; break; }
            }

            int row = 0;
            foreach(Results_Test rt in list_of_result)
            {
                while (dataGridView1.Rows[row].Cells[0].Value != rt.name) row++;
                dataGridView1.Rows[row].Cells[0].Style.BackColor = rt.test_result ? Color.Green : Color.Red;
                foreach (Tests te in rt.All_Tests)
                {
                    row++;
                    for (int col = 0; col < te.test.Count; col++)
                        { dataGridView1.Rows[row].Cells[col + 1].Style.BackColor = te.test[col].Item2 ? Color.Green : Color.Red; }
                }
            }
        }
    }
}
