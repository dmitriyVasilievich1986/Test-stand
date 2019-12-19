using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
struct ByteToFloat
{
    [FieldOffset(0)]
    private float Float1;
    [FieldOffset(0)]
    private byte b2;
    [FieldOffset(1)]
    private byte b1;
    [FieldOffset(2)]
    private byte b4;
    [FieldOffset(3)]
    private byte b3;
    public float Out(byte a1, byte a2, byte a3, byte a4)
    {
        b1 = a1;
        b2 = a2;
        b3 = a3;
        b4 = a4;
        return Float1;
    }
}

namespace test_stand
{


    public class Port_Data
    {
        public SerialPort Port;
        public string Name = "COM1";
        public int Speed = 115200;
        public byte[] Data_Receive;
        public byte[] Data_Transmit;
        public byte[] Now = null;
        public int Register_Count;
        public float[] Result;
        public int Status = 1;
        public bool Exchange = false;
        string[] Condition;

        ByteToFloat BTF = new ByteToFloat();

        public delegate void Receive_Handler();
        public event Receive_Handler Receive_Event;

        

        public Port_Data(SerialPort port) { Port = port; Port.PortName = Name; Port.BaudRate = Speed; Port.DataBits = 8; Port.DataReceived += new SerialDataReceivedEventHandler(this.Receive_Data); }

        public Port_Data(SerialPort port, string name) { Port = port; Name = name; Port.PortName = Name; Port.BaudRate = Speed; Port.DataBits = 8; Port.DataReceived += new SerialDataReceivedEventHandler(this.Receive_Data); }

        public Port_Data(SerialPort port, Int32 speed) { Port = port; Speed = speed; Port.PortName = Name; Port.BaudRate = Speed; Port.DataBits = 8; Port.DataReceived += new SerialDataReceivedEventHandler(this.Receive_Data); }

        public Port_Data(SerialPort port, string name, Int32 speed) { Port = port; Speed = speed; Name = name; Port.PortName = Name; Port.BaudRate = Speed; Port.DataBits = 8; Port.DataReceived += new SerialDataReceivedEventHandler(this.Receive_Data); }

        public void Open_Close_Port()
        {
            if (Port.IsOpen) Port.Close();
            else
            {
                try
                {
                    Port.PortName = Name;
                    Port.BaudRate = Speed;
                    Port.DataBits = 8;
                    Port.Open();
                }
                catch (Exception err) { }
                Now = null;
            }
        }

        public void Receive_Data(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Exchange = true;
            Data_Receive = new byte[Port.BytesToRead];
            Result = new float[Register_Count];

            Port.Read(Data_Receive, 0, Port.BytesToRead);

            if (ModRTU_CRC(Data_Receive)[Data_Receive.Length - 2] != Data_Receive[Data_Receive.Length - 2] || ModRTU_CRC(Data_Receive)[Data_Receive.Length - 1] != Data_Receive[Data_Receive.Length - 1]) return;

            if (Data_Receive[1] == 0x04)
            {
                for (int x = 0; x < Register_Count; x++)
                { Result[x] = BTF.Out(Data_Receive[3 + x * 4], Data_Receive[4 + x * 4], Data_Receive[5 + x * 4], Data_Receive[6 + x * 4]); }
            }            

            if (Now != null && Data_Receive[1] == Now[1]) Now = null;

            Receive_Event?.Invoke();
        }

        public void Transmit_Data(byte[] data)
        {
            Exchange = false;
            if (Now != null) { data = Now; }
            Data_Transmit = data;
            switch(Data_Transmit[1])
            {
                case 0x02:
                    Port.ReceivedBytesThreshold = (Data_Transmit[5] - 1) / 8 + 6;
                    break;
                case 0x04:
                    Register_Count = Data_Transmit[5] / 2;
                    Port.ReceivedBytesThreshold = Data_Transmit[5] * 2 + 5;
                    break;
                case 0x06:
                    Register_Count = 1;
                    Port.ReceivedBytesThreshold = 8;
                    break;
            }
            if (Port.IsOpen) Port.Write(data, 0, data.Length);            
        }

        #region CRC

        private byte[] ModRTU_CRC(byte[] data)
        {
            ulong crc = 0xFFFF;
            for (int pos = 0; pos < (data.Length - 2); pos++)
            {
                crc ^= (ulong)data[pos];
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 1) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else crc >>= 1;
                }
            }

            data[data.Length - 2] = (byte)crc;
            data[data.Length - 1] = (byte)(crc >> 8);
            return data;
        }

        public void Read_Analog(byte AddresModule, short StartAddres, byte RegisterCount)
        {
            byte[] addres = { AddresModule, 0x04, (byte)(StartAddres >> 8), (byte)(StartAddres), (byte)(RegisterCount >> 8), (RegisterCount), 0, 0 };
            Transmit_Data(ModRTU_CRC(addres));
        }

        public byte[] Read_Analog_CRC(byte AddresModule, short StartAddres, short RegisterCount)
        {
            byte[] addres = { AddresModule, 0x04, (byte)(StartAddres >> 8), (byte)(StartAddres), (byte)(RegisterCount >> 8), (byte)(RegisterCount), 0, 0 };
            return ModRTU_CRC(addres);
        }

        public void Send_Analog(byte AddresModule, short StartAddres, short RegisterState)
        {
            byte[] addres = { AddresModule, 0x06, (byte)(StartAddres >> 8), (byte)(StartAddres), (byte)(RegisterState >> 8), (byte)(RegisterState), 0, 0 };
            Transmit_Data(ModRTU_CRC(addres));
        }

        public byte[] Send_Analog_CRC(byte AddresModule, short StartAddres, short RegisterState)
        {
            byte[] addres = { AddresModule, 0x06, (byte)(StartAddres >> 8), (byte)(StartAddres), (byte)(RegisterState >> 8), (byte)(RegisterState), 0, 0 };
            return ModRTU_CRC(addres);
        }

        public void Send_Analog_Now(byte AddresModule, short StartAddres, short RegisterState)
        {
            byte[] addres = { AddresModule, 0x06, (byte)(StartAddres >> 8), (byte)(StartAddres), (byte)(RegisterState >> 8), (byte)(RegisterState), 0, 0 };
            Now = new byte[25];
            Now = ModRTU_CRC(addres);
        }

        public void Read_Discret(byte AddresModule, short StartAddres, byte RegisterCount)
        {
            byte[] addres = { AddresModule, 0x02, (byte)(StartAddres >> 8), (byte)(StartAddres), (byte)(RegisterCount >> 8), (RegisterCount), 0, 0 };
            Transmit_Data(ModRTU_CRC(addres));
        }

        #endregion
    }
}
