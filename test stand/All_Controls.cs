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
        public Button butt;
        string text;
        public Data_Transit.Addres_Controls Dout;
        public byte Addres;
        byte position;

        public All_Controls(Button b, Data_Transit.Addres_Controls dout, byte add, byte pos)
        {
            butt = b;
            text = b.Text;
            Dout = dout;
            Addres = add;
            position = pos;
        }

        public void Data_Transmit()
        {
            byte Set = 0;
            if (butt.BackColor == Color.LightGray) Set = 1;
            Data_Transit.PortControl.Interrupt(new byte[] { Dout.Addres, 0x06, 0, Addres, 0, Set });
        }

        public void Checkout(short check)
        {
            if ((check & 1 << position) != 0) butt.BackColor = Color.Red;
            else butt.BackColor = Color.LightGray;
        }
    }
}
