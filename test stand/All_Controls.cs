using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_stand
{
    public class All_Controls
    {
        Button butt;
        string text;
        public Data_Transit.Addres_Controls Dout;
        public short Addres;
        short Register;
        public int Position;
        byte Command;
        float result;
        short Register_Check;

        public All_Controls(Button b, Data_Transit.Addres_Controls dout, byte com, short add, int pos, short reg)
        {
            butt = b;
            text = b.Text;
            Dout = dout;
            Addres = add;
            Position = pos;
            Register = reg;
            Command = com;
            Register_Check = rc;
        }

        public All_Controls(Data_Transit.Addres_Controls dout, byte com, short add, byte pos, short reg)
        {
            Dout = dout;
            Addres = add;
            Position = pos;
            Register = reg;
            Command = com;
        }

        public void Data_Transmit()
        {
            if(Command==0x06)
            {
                byte Set = 0;
                if (butt.BackColor == Color.LightGray) Set = 1;
                Data_Transit.PortControl.Interrupt(new byte[] { Dout.Addres, Command, (byte)(Addres >> 8), (byte)Addres, 0, Set });
            }
            else
            {
                Data_Transit.PortControl.Transmit(new byte[] { Dout.Addres, Command, (byte)(Addres >> 8), (byte)Addres, (byte)(Register >> 8), (byte)Register });
            }            
        }

        public void Checkout(short check)
        {
            if ((check & 1 << Position) != 0) butt.BackColor = Color.Red;
            else butt.BackColor = Color.LightGray;
        }

        public float Result
        {
            get { return result; }
            set
            {
                result = (float)value;
                if (result < 1) butt.Text = text + $" {result:0.000}";
                else butt.Text = text + $" {result:0.0}";
            }
        }
    }

    public class All_Data
    {
        public Data_Transit.Addres_Controls Dout;
        short Addres;
        short Register;
        byte Position;
        byte Command;

        public All_Data(Data_Transit.Addres_Controls dout, short add, byte pos, byte com, short reg)
        {
            Dout = dout;
            Addres = add;
            Position = pos;
            Command = com;
            Register = reg;
        }

        public void Data_Transmit()
        {
            Data_Transit.PortControl.Transmit(new byte[] { Dout.Addres, Command, (byte)(Addres >> 8), (byte)Addres, (byte)(Register >> 8), (byte)Register });
        }
    }
}
