using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 货到DEMO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void flush_serial_port()
        {
            RegistryKey keyComm = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyComm != null)
            {
                string[] subKeys = keyComm.GetValueNames();
                comboBox1.Items.Clear();
                foreach (string name in subKeys)
                {
                    string value = (string)keyComm.GetValue(name);
                    comboBox1.Items.Add(value);
                }
                comboBox1.SelectedIndex = subKeys.Length - 1;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            flush_serial_port();
            components = null;
            mSerialPort = null;
            mSerialPort = new SerialPort();
            mSerialPort.BaudRate = 115200;
            mSerialPort.Parity = Parity.None;
            mSerialPort.DataBits = 8;
            mSerialPort.StopBits = StopBits.One;
            mSerialPort.WriteTimeout = 1000;
            mSerialPort.ReadTimeout = 1000;
        }

        private SerialPort mSerialPort;


        public static string StreamToString(MemoryStream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();


        }

        private byte[] Str2Hex(string Str)
        {
            int i = 0;
            string HexStr = Str.Replace(" ", "");
            int len = HexStr.Length;
            byte[] Return_Arr = new byte[len / 2];
            string TempStr;

            while (len > 1)
            {
                len -= 2;
                TempStr = HexStr.Substring(i * 2, 2);
                Return_Arr[i++] = Convert.ToByte(TempStr, 16);
            }
            return Return_Arr;
        }

        private long Comm_Send_Bytes = 0;                           //发送数据长度
        public void Send_Simple(string ID)
        {


            string Com = comboBox1.Text;
            string aa = "0D2428006000"+ ID.ToString().PadLeft(2, '0') + "0508323032333036313431303038353700000000000000000000000000004f0D0A";

            MessageBox.Show(aa);

            try
            {
                mSerialPort.PortName = Com;
                mSerialPort.Open();


                byte[] Serial_SendBuff = Str2Hex(aa);
                Comm_Send_Bytes += Serial_SendBuff.Length;

                mSerialPort.Write(Serial_SendBuff, 0, Serial_SendBuff.Length);



            }
            catch (Exception ex)
            {
                MessageBox.Show("发送数据时发生错误！ " + ex.ToString(), "错误提示");
                return;
            }
            finally
            {
                if (this.mSerialPort.IsOpen)
                {
                    this.mSerialPort.Close();
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Send_Simple("1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = comboBox2.Text;
            Send_Simple(id);
        }
    }
}
