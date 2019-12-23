using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_stand
{
    static public class All_Tests
    {
        static async private Task<bool> Comparison(string parameters, int addres, int Min, int Max)
        {
            int cycle = 0;
            while (cycle < 5)
            {
                cycle++;
                await Task.Delay(500);
                if (Data_Transit.Results[parameters][addres] > Min && Data_Transit.Results[parameters][addres] < Max) cycle += 5;
            }
            if (Data_Transit.Results[parameters][addres] > Min && Data_Transit.Results[parameters][addres] < Max) return true;
            return false;
        }

        static async public Task<int> KF_TC_Test(string parameters, string test, byte dout)
        {
            int norm = 0;

            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            Data_Transit.EW.Line_Number();
            int column = await Data_Transit.EW.Column(test);
            if (column >= 250) { Data_Transit.EW.Close_Excell(); return 0; }

            for (int kf = 0; kf < (Data_Transit.Registers_Module[parameters][5] / 2); kf++)
            {
                Data_Transit.PortControl.Interrupt(new byte[] { dout, 0x06, 0, Data_Transit.Registers[parameters][kf], 0, 1 });
                while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
                for (int check = 0; check < (Data_Transit.Registers_Module[parameters][5] / 2); check++)
                {
                    if (check == kf)
                    {
                        if (await Comparison(parameters, check, Data_Transit.Module_Parameters[parameters][0], Data_Transit.Module_Parameters[parameters][1]))
                        { Data_Transit.EW.Result_Save(Data_Transit.Results[parameters][check], Color.Green, column + 1 + check + kf * (Data_Transit.Registers_Module[parameters][5] / 2)); }

                        else
                        {
                            Data_Transit.EW.Result_Save(Data_Transit.Results[parameters][check], Color.Red, column + 1 + check + kf * (Data_Transit.Registers_Module[parameters][5] / 2));
                            norm++;
                        }
                    }
                    else
                    {
                        if (await Comparison(parameters, check, 0, Data_Transit.Module_Parameters[parameters][2]))
                        { Data_Transit.EW.Result_Save(Data_Transit.Results[parameters][check], Color.Green, column + 1 + check + kf * (Data_Transit.Registers_Module[parameters][5] / 2)); }
                        else
                        {
                            Data_Transit.EW.Result_Save(Data_Transit.Results[parameters][check], Color.Red, column + 1 + check + kf * (Data_Transit.Registers_Module[parameters][5] / 2));
                            norm++;
                        }
                    }
                }
                Data_Transit.PortControl.Interrupt(new byte[] { dout, 0x06, 0, Data_Transit.Registers[parameters][kf], 0, 0 });
                while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(200);
            }
            if (norm == 0) { Data_Transit.EW.Result_Save("OK", Color.Green, column); }
            else { Data_Transit.EW.Result_Save("Not OK", Color.Red, column); }
            Data_Transit.EW.Close_Excell();
            return norm;
        }

        static async public Task<int> Power_Test()
        {
            int norm = 0;
            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            Data_Transit.EW.Line_Number();
            int column = await Data_Transit.EW.Column("Ток потребления");
            if (column >= 250) return 0;
            
            //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x10, 0, Data_Transit.Registers["current"][3], 0, 04, 8, 0, 0, 0, 0, 0, 0, 0, 0 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(500);

            for (int a = 0; a < Data_Transit.port; a++)
            {
                //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x06, 0, Data_Transit.Registers["current"][a], 0, 1 });
                while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(1000);

                if (Data_Transit.Results["current"][0] > Data_Transit.Current_Norm + .1 ||
                    Data_Transit.Results["current"][0] < Data_Transit.Current_Norm - .1)  norm++;
                Data_Transit.EW.Result_Save(Data_Transit.Results["current"][0], column);                     

                //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x06, 0, Data_Transit.Registers["current"][a], 0, 0 });
                while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(500);
            }

            //Data_Transit.PortControl.Interrupt(new byte[] { Data_Transit.DoutControl, 0x10, 0, Data_Transit.Registers["current"][3], 0, 04, 8, 0, 1, 0, 1, 0, 1, 0, 1 });
            while (Data_Transit.PortControl.Data_Interrupt != null) await Task.Delay(500);
            Data_Transit.EW.Close_Excell();

            return norm;
        }

        static async public Task<int> Temperature_Test()
        {
            int norm = 0;
            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            Data_Transit.EW.Line_Number();
            int column = await Data_Transit.EW.Column("Проверка температуры");
            if (column >= 250) { Data_Transit.EW.Close_Excell(); return 0; }

            if (!await Comparison("temperature", 0, 20, 35)) norm++;
            
            if (norm == 0) { Data_Transit.EW.Result_Save(Data_Transit.Results["temperature"][0], Color.Green, column); }
            else { Data_Transit.EW.Result_Save(Data_Transit.Results["temperature"][0], Color.Red, column); }
            Data_Transit.EW.Close_Excell();
            return norm;
        }

        static async public Task<int> TU_Test()
        {
            int norm = 0;
            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            int column = await Data_Transit.EW.Column("Проверка ТУ");
            if (column >= 250) { Data_Transit.EW.Close_Excell(); return 0; }

            for (int tu = 0; tu < Data_Transit.Results["12V"][0]; tu++)
            {
                Data_Transit.PortChanelA.Interrupt(new byte[] { Data_Transit.Module, 0x06, 0, Data_Transit.Registers["tu"][tu], 0, 1 });
                while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(1000);

                if (await Comparison("12V", 0, 150, 250))
                    Data_Transit.EW.Result_Save("OK", Color.Green, column + 1 + tu);
                else { Data_Transit.EW.Result_Save("Not OK", Color.Red, column + 1 + tu); norm++; }

                Data_Transit.PortChanelA.Interrupt(new byte[] { Data_Transit.Module, 0x06, 0, Data_Transit.Registers["tu"][tu], 0, 0 });
                while (Data_Transit.PortChanelA.Data_Interrupt != null) await Task.Delay(1000);
            }

            if (norm == 0) { Data_Transit.EW.Result_Save("OK", Color.Green, column); }
            else { Data_Transit.EW.Result_Save("Not OK", Color.Red, column); }
            Data_Transit.EW.Close_Excell();
            return norm;
        }

        static async public Task<int> TC12V_Test(int u12)
        {
            int norm = 0;
            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            int column = await Data_Transit.EW.Column("Проверка 12В ТС");
            if (column >= 250) { Data_Transit.EW.Close_Excell(); return 0; }

            if (!await Comparison("12V", 1, (u12 - 2), (u12 + 2))) norm++;

            if (norm == 0) { Data_Transit.EW.Result_Save(Data_Transit.Results["12V"][1], Color.Green, column); }
            else { Data_Transit.EW.Result_Save(Data_Transit.Results["12V"][1], Color.Red, column); }
            Data_Transit.EW.Close_Excell();
            return norm;
        }

        static async public Task<int> EnTU_Test()
        {
            int norm = 0;
            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            int column = await Data_Transit.EW.Column("Проверка EnTU");
            if (column >= 250) { Data_Transit.EW.Close_Excell(); return 0; }

            while (norm < 5)
            {
                await Task.Delay(150);
                norm++;
                if (Data_Transit.EnTU) { norm = 0; break; }
            }

            if (norm == 0) { Data_Transit.EW.Result_Save("OK", Color.Green, column); }
            else Data_Transit.EW.Result_Save("Not OK", Color.Red, column);
            Data_Transit.EW.Close_Excell();
            return norm;
        }

        static async public Task<int> MTU5_Power_Test()
        {
            int norm = 0;
            Data_Transit.EW.Open_Excell(Data_Transit.Addres);
            int column = await Data_Transit.EW.Column("Power 1");

            if (column >= 250) { Data_Transit.EW.Close_Excell(); return 0; }

            for (int a = 0; a < 2; a++)
            {
                if (Data_Transit.Results["power"][a] < 800 || Data_Transit.Results["power"][a] > 1350)
                    { Data_Transit.EW.Result_Save(Data_Transit.Results["power"][a], Color.Red, (column + a)); norm++; }
                else
                    { Data_Transit.EW.Result_Save(Data_Transit.Results["power"][a], Color.Green, (column + a)); }
            }

            Data_Transit.EW.Close_Excell();
            return norm;
        }
    }
}
