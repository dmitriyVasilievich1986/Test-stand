using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using ModBus_Library;
using System.Data.SqlClient;

namespace test_stand
{
    static public class Data_Transit
    {
        public static int row = 0;

        public static string Data;
        public static string Name = "Nomodule";
        public static string Addres = @"C:\asd\Nomodule.xlsx";
        public static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        public static int serial_number = 0;


        public static bool EnTU = false;
        public static int port = 3;
        public static int exchange_port = 0;
        public static int TC_12V = 12;
        public static bool shift_is_down = false;
        public static bool escape = true;

        public static List<Send> Registers_Controls = new List<Send>();
        public static List<Controls_Only> controls_only = new List<Controls_Only>();
        public static List<Controls_Only> controls_module = new List<Controls_Only>();
        public static List<Send_Only> port_control_send_data = new List<Send_Only>();

        public static List<Button_Send> port_control_button = new List<Button_Send>();
        public static List<Button_Result> all_button_result = new List<Button_Result>();

        public static Dictionary<string, byte[]> Registers = new Dictionary<string, byte[]>();
        public static Dictionary<string, int[]> Module_Parameters = new Dictionary<string, int[]>
        {
            { "din",new int[4]},
            { "kf",new int[3]},
            { "tc",new int[3]}
        };
        public static Dictionary<string, float[]> Results = new Dictionary<string, float[]>
        {
            {"current", new float[2] },
            {"din", new float[32] },
            {"kf", new float[3] },
            { "tc", new float[3] },
            { "power", new float[2] },
            { "mtutu", new float[2] },
            { "temperature", new float[1] },
            {"12V", new float[2] }
        };
        public static Dictionary<string, byte[]> Registers_Module = new Dictionary<string, byte[]>
        {
            { "din", new byte[6] },
            { "din32", new byte[6] },
            { "kf", new byte[6] },
            { "tc", new byte[6] },
            { "tu", new byte[6] },
            { "power", new byte[6] },
            { "mtutu", new byte[6] },
            { "temperature", new byte[6] },
            { "entu", new byte[6] }
        };
        
        public static float Current_Norm = 0;
        public static bool Current_Check = false;
        
        public static Addres_Controls Dout_Din16 = new Addres_Controls(0x14);
        public static Addres_Controls Dout_Control = new Addres_Controls(0x13);
        public static Addres_Controls Dout_Din32 = new Addres_Controls(0x15);
        public static Addres_Controls Current_PSC = new Addres_Controls(0x11);
        public static Addres_Controls v12 = new Addres_Controls(0x10);
        public static Addres_Controls module = new Addres_Controls(0x01);

        public static ModBus_Libra PortControl = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port1);
        public static ModBus_Libra PortChanelA = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port2);
        public static ModBus_Libra PortChanelB = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port3);
        //public static Excell_Work EW = new Excell_Work();

        public static void ModuleName(string name) { Name = name; Addres = @"C:\asd\" + Name.ToString() + @".xlsx"; }

        public class Addres_Controls
        {
            private byte add;

            public Addres_Controls(byte Addres) { add = Addres; }

            public byte Addres
            {
                get { return add; }
                set { add = value; }
            }
        }

        public class Send
        {
            Addres_Controls AC;
            byte Command;
            short Start;
            short Registers;

            public Send(Addres_Controls ac, byte com, short st, short reg)
            {
                AC = ac;
                Command = com;
                Start = st;
                Registers = reg;

            }

            public void Data_Send()
            {
                Data_Transit.PortControl.Transmit(new byte[] { AC.Addres, Command, (byte)(Start >> 8), (byte)Start, (byte)(Registers >> 8), (byte)Registers });
            }
        }

        public static void Last_Row()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=All_Moduls;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT max([id]) FROM [All_Moduls].[dbo].[module]", connection);
                Data_Transit.row = (int)command.ExecuteScalar();
            }
        }

        public static void Insert_Current(List<(float, bool)> items, string name)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=All_Moduls;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO [All_Moduls].[dbo].[{name}] (iden", connection);
                for (int a = 0; a < items.Count; a++)
                {
                    command.CommandText += $",{name}{a + 1},result{a + 1}";
                }
                command.CommandText += $") VALUES ({row}";
                for (int a = 0; a < items.Count; a++)
                {
                    command.CommandText += $",@float{a},@bool{a}";
                }
                command.CommandText += ")";
                for (int a = 0; a < items.Count; a++)
                {
                    command.Parameters.AddWithValue($"@float{a}", items[a].Item1);
                    command.Parameters.AddWithValue($"@bool{a}", items[a].Item2);
                }                
                command.ExecuteNonQuery();
            }
        }

        public static bool Test_Need(string Parameters)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=All_Moduls;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT [{Parameters}] FROM [All_Moduls].[dbo].[Tests] where name='{Data_Transit.Name}'", connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                result = reader.GetBoolean(0);
            }
            return result;
        }

        public static void Insert_Module()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=All_Moduls;Integrated Security=True"))
            {
                connection.Open();
                string[] date_split = DateTime.Today.ToShortDateString().Split('.');
                SqlCommand command = new SqlCommand($"INSERT INTO [All_Moduls].[dbo].[module] (module, sn, data) VALUES ('{Data_Transit.Name}',{Data_Transit.serial_number},'{date_split[2] + "-" + date_split[1] + "-" + date_split[0]}')", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
