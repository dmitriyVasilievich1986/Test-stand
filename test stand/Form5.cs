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
        public string sx;

        public Form5()
        {
            InitializeComponent();
            Serial_Number.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await Start_TestAsync(); };
            Serial_Number.MouseClick += (s, e) => { Serial_Number.Text = ""; };
        }

        private async Task Start_TestAsync()
        {
            if (Serial_Number.Text == "" || Serial_Number.Text == "Введите серийный номер")
                { MessageBox.Show("Введите номер", "", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Test_Condition.Text = "Старт теста...";

            this.WindowState = FormWindowState.Minimized;
            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x10, 0, 0x51, 00, 02, 04, 0, 01, 0, 1 }); // подача 220

            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            Data_Transit.EW.Line_Number();
            Data_Transit.EW.Result_Save(DateTime.Now, await Data_Transit.EW.Column("Дата"));
            Data_Transit.EW.Close_Excell();

            int norm = 0;

            norm += await All_Tests.Power_Test();
            norm += await All_Tests.KF_TC_Test("din", "Проверка Din8", Data_Transit.Dout_Din16);
            norm += await All_Tests.KF_TC_Test("kf", "Проверка КФ", Data_Transit.DoutControl);
            norm += await All_Tests.KF_TC_Test("tc", "Проверка ТС", Data_Transit.DoutControl);
            norm += await All_Tests.Temperature_Test();
            norm += await All_Tests.TU_Test();
            norm += await All_Tests.TC12V_Test(12);
            norm += await All_Tests.EnTU_Test();
            norm += await All_Tests.MTU5_Power_Test();

            Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x10, 0, 0x51, 00, 02, 04, 0, 00, 0, 0 }); // отключение 220

            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            if (norm > 0)
            {
                Data_Transit.EW.Result_Save(Serial_Number.Text, Color.Red, await Data_Transit.EW.Column("Сер.№"));
                DialogResult DR = MessageBox.Show("Сохранить результат?", "Во время проверки возникли ошибки", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (DR == DialogResult.Yes) Data_Transit.EW.Result_Save(Data_Transit.EW.Line - 2, await Data_Transit.EW.Column("№"));
            }
            else
            {
                Data_Transit.EW.Result_Save(Serial_Number.Text, Color.Green, await Data_Transit.EW.Column("Сер.№"));
                DialogResult DR = MessageBox.Show("Сохранить результат?", "Проверка прошла успешно", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DR == DialogResult.Yes) Data_Transit.EW.Result_Save(Data_Transit.EW.Line - 2, await Data_Transit.EW.Column("№"));
            }

            Data_Transit.EW.Close_Excell();
            this.Close();
        }
    }
}
