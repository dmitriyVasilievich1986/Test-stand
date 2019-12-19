using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using ModBus_Library;

namespace test_stand
{
    static public class Data_Transit
    {
        public static string Data;
        public static string Name = "Nomodule";
        public static string Addres;
        public static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";

        public static bool EnTU = false;
        public static int port = 1;

        public static Dictionary<string, byte[]> Registers = new Dictionary<string, byte[]>();
        public static Dictionary<string, int[]> Module_Parameters = new Dictionary<string, int[]>
        {
            { "din",new int[3]},
            { "kf",new int[3]},
            { "tc",new int[3]}
        };
        public static Dictionary<string, byte[]> Registers_Controls = new Dictionary<string, byte[]>
        {
            {"Current_PSC", new byte[]{ 0x11, 0x04, 0x01, 0x0A, 0x00, 0x04 } },
            {"DoutControl", new byte[]{ 0x13, 0x02, 0x00, 0x01, 0x00, 0x10 } },
            {"Dout_Din16", new byte[]{ 0x14, 0x02, 0x00, 0x01, 0x00, 0x10 } },
            {"12V", new byte[]{ 0x10, 0x04, 0x01, 0x08, 0x00, 0x04 } }
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

        public static byte Dout_Din16 = 0x14;
        public static byte Dout_Din32 = 0x15;
        public static byte DoutControl = 0x13;
        public static byte Current_PSC = 0x11;
        public static byte Module = 1;
        public static byte v12 = 0x10;
                
        public static ModBus_Libra PortControl = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port1);
        public static ModBus_Libra PortChanelA = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port2);
        public static ModBus_Libra PortChanelB = new ModBus_Libra(new SerialPort(), Properties.Settings.Default.Port3);
        public static Excell_Work EW = new Excell_Work();

        public static void ModuleName(string name) { Name = name; Addres = @"C:\asd\" + Name.ToString() + @".xlsx"; }
    }
}
