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

namespace test_stand
{
    public partial class Result : Form
    {
        List<Results_Test> list_of_result;

        public Result(List<Results_Test> al)
        {
            int column_count = 0;
            list_of_result = al;
            foreach (Results_Test rt in al) { foreach(Tests te in rt.All_Tests) { if (te.test.Count > column_count) column_count = te.test.Count; }}

            InitializeComponent();
            button1.Visible = Data_Transit.serial_number > 0 ? true : false;

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
            DR[1] = Data_Transit.Name;
            if (Data_Transit.serial_number > 0) DR[2] = Data_Transit.serial_number.ToString();
            DT.Rows.Add(DR);

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
            Data_Transit.Insert_Module();
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
            }
            this.Close();
        }

        async void Colorized()
        {
            await Task.Delay(100);
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
