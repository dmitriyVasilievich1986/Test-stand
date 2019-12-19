using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace test_stand
{    
    public class Excell_Work : IDisposable
    {
        Excel.Application xlApp = new Excel.Application(); //Excel
        Excel.Workbook xlWB; //рабочая книга откуда будем копировать лист  
        Excel.Worksheet xlSht; //лист Excel

        public int Line;
        public string Addres;

        public void Dispose()
        {
            
        }
        
        public void Open_Excell(string addres)
        {
            xlWB = xlApp.Workbooks.Open(addres);
            xlSht = xlWB.Worksheets["Лист1"];
            Addres = addres;
        }

        public void Close_Excell() { xlWB.Save(); xlWB.Close(true); }

        public dynamic Module_Parameters(string Parameters, string Module)
        {
            int Column = 1;
            int Row = 1;

            while (Column < 50 && xlSht.Cells[1, Column].Text != Module) Column++;
            while (Row < 50 && xlSht.Cells[Row, 1].Text != Parameters) Row++;

            if (Column == 50 || Row == 50) return "0";

            return xlSht.Cells[Row, Column].value;
        }

        public void Module_Parameters_Save(string Parameters, string Module, object New_Param)
        {
            int Column = 1;
            int Row = 1;

            while (Column < 50 && xlSht.Cells[1, Column].Text != Module) Column++;
            while (Row < 50 && xlSht.Cells[Row, 1].Text != Parameters) Row++;

            xlSht.Cells[Row, Column] = New_Param;
        }

        public async Task<Int32> Column(string Parameters)
        {
            Int32 Value = 1;

            await Task.Run(() => {
                while (Value < 250 && xlSht.Cells[1, Value].Text != Parameters) Value++;
            });

            return Value;
        }

        public async void Line_Number()
        {
            Line = 3;
            int a = await Column("№");
            while (xlSht.Cells[Line, a].text != "") Line++;
        }

        public void Result_Save(object text, Color color, int Column)
        {
            xlSht.Cells[Line, Column] = text;
            if (color == Color.Gray) return;
            (xlSht.Cells[Line, Column] as Microsoft.Office.Interop.Excel.Range).Interior.Color = color;            
        }

        public void Result_Save(object text, int Column)
        {
            xlSht.Cells[Line, Column] = text;
        }
    }
}
