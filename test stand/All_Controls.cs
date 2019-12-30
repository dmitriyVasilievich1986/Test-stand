using ModBus_Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_stand
{
    public class Controls_Only
    {        
        public Button button;
        public Data_Transit.Addres_Controls dout;
        public ModBus_Libra ML;

        byte code;
        string text;        
        float result;
        short addres = 0x0000;

        public string name;
        public int position;

        public Controls_Only(Button using_button, Data_Transit.Addres_Controls dout_addres, ModBus_Libra ml, byte start_addres, int check_position, string using_name)
            { ML = ml; button = using_button; dout = dout_addres; code = start_addres; name = using_name; position = check_position; text = button.Text; }

        public void Data_Transmit()
        {
            byte set = 0;
            if (button.BackColor == Color.LightGray) set = 1;
            ML.Interrupt(new byte[] { dout.Addres, 0x06, 0x00, code, 0x00, set});
        }

        public void Checkout(short check)
        {
            if ((check & 1 << position) != 0) button.BackColor = Color.Red;
            else button.BackColor = Color.LightGray;
        }
        
        public float Result
        {
            get { return result; }
            set
            {
                result = (float)value;
                if (result < 1) button.Text = text + $" {result:0.000}";
                else button.Text = text + $" {result:0.0}";
            }
        }

        public short Addres
        {
            get { return addres; }
            set { addres = value; }
        }
    }

    public class Button_Result
    {
        public ModBus_Libra ML;
        public Data_Transit.Addres_Controls dout;
        public Button button;

        public short addres;
        public string name;
        int position;
        string text;
        float result;

        public Button_Result(ModBus_Libra using_port, Data_Transit.Addres_Controls using_dout, Button using_button, short using_addres, int using_position, string using_name, string button_text)
            { ML = using_port; dout = using_dout; button = using_button;addres = using_addres;position = using_position; name = using_name; text = button_text; Result = 0; }
        public Button_Result(ModBus_Libra using_port, Data_Transit.Addres_Controls using_dout, Button using_button, short using_addres, int using_position, string using_name)
        { ML = using_port; dout = using_dout; button = using_button; addres = using_addres; position = using_position; text = button.Text; name = using_name;}

        public void Checkout(ModBus_Libra port)
        {
            try
            {
                if (port.Port.PortName != ML.Port.PortName) return;
                if (port.Data_Receive[0] == dout.Addres && port.Data_Receive[1] == 0x04 &&
                    (short)((short)(port.Data_Transmit[2] << 8) | (short)port.Data_Transmit[3]) == addres)
                { Result = port.Result[position]; }
            }
            catch (Exception) { }            
        }

        public float Result
        {
            get { return (float)Math.Round(result, 3); }
            set
            {
                result = (float)value;
                if (result < .5f) button.Text = text + $" {result:0.000}";
                else button.Text = text + $" {result:0.0}";
            }
        }
    }

    public class Send_Only
    {
        public ModBus_Libra ML;
        public Data_Transit.Addres_Controls dout;

        public byte[] code;

        public Send_Only(ModBus_Libra Port, Data_Transit.Addres_Controls dout_addres, byte[] using_code)
        { ML = Port; dout = dout_addres; code = using_code; }

        public virtual void Trasmit_Data() { code[0] = dout.Addres; ML.Transmit(code); }
    }

    public class Button_Send : Send_Only
    {
        Color color;
        public Button button;

        short addres_color=0x00;
        ModBus_Libra MLC;
        Data_Transit.Addres_Controls dout_color;
        int position = 0;
        public string name;

        public Button_Send(ModBus_Libra Port, Data_Transit.Addres_Controls dout_addres, byte[] using_code, Button using_button, Color using_color, string using_name) : base(Port, dout_addres, using_code)
        { button = using_button; color = using_color; name = using_name; }

        public override void Trasmit_Data()
        {
            code[0] = dout.Addres;
            byte set = 1;
            if (button.BackColor == Color.Red) set = 0;
            if (code[1] == 0x10) { for (int a = 8; a < code.Length; a += 2) code[a] = set; }
            else code[5] = set;
            ML.Interrupt(code);
        }

        public void Reset()
        {
            code[0] = dout.Addres;
            if (code[1] == 0x10) { for (int a = 8; a < code.Length; a += 2) code[a] = 0; }
            else code[5] = 0;
            ML.Interrupt(code);
        }

        public void Initialization(short color_addres, ModBus_Libra port_color, Data_Transit.Addres_Controls using_dout_color, int using_position)
        {            
            MLC = port_color;
            position = using_position;
            addres_color = color_addres;
            dout_color = using_dout_color;
        }

        public void Checkout(ModBus_Libra port)
        {
            try
            {
                if (port.Port.PortName == MLC.Port.PortName && addres_color != 0 && port.Data_Receive[1] == 0x02 &&
                (short)((short)(port.Data_Transmit[2] << 8) | (short)port.Data_Transmit[3]) == addres_color &&
                port.Data_Receive[0] == dout_color.Addres)
                {
                    short some = 0;
                    some = port.Data_Receive[2] == 1 ? (short)port.Data_Receive[3] : (short)((short)(Data_Transit.PortControl.Data_Receive[3] << 8) | (short)Data_Transit.PortControl.Data_Receive[4]);
                    button.BackColor = (some & position) != 0 ? Color.Red : color;
                }
            }
            catch (Exception) { }            
        }
    }

    public class Results_Test
    {
        public List<Tests> All_Tests;
        int count = -1;
        public string name;
        public bool test_result;

        public Results_Test(string Name) { name = Name; All_Tests = new List<Tests>(); }            

        public void Add_Test(string Name) { All_Tests.Add(new Tests(Name)); count++; }

        public void Add_Item(float item, bool result) { All_Tests[count].Add_Item(item, result); }
    }

    public class Tests
    {
        public List<(float, bool)> test;
        public string name;
        public bool test_result = true;

        public Tests(string Name) { name = Name; test = new List<(float, bool)>(); }

        public void Add_Item(float item, bool result)
        {
            test.Add((item, result));
        }
    }
}
