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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        private SerialPort mSerialPort;




        public static string StreamToString(MemoryStream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();


        }

        public void Light(string Com, string ID, string Mode)
        {

            try
            {
                mSerialPort.PortName = Com;
                mSerialPort.Open();
                MemoryStream stream = new MemoryStream();
                stream.WriteByte(Convert.ToByte("A" + ID, 0x10));
                stream.WriteByte(Convert.ToByte(Mode.ToString().PadLeft(2, '0'), 0x10));


                mSerialPort.Write(stream.ToArray(), 0, 2);

                lb_debug.Items.Add("发送串口命令：" + "A" + ID + " " + Mode.ToString().PadLeft(2, '0'));

            }
            catch
            {
                lb_debug.Items.Add("发送串口命令：" + "A" + ID + " " + Mode.ToString().PadLeft(2, '0') + "失败了");
            }
            finally
            {
                if (this.mSerialPort.IsOpen)
                {
                    this.mSerialPort.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "1", "1");
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
            mSerialPort.BaudRate = 9600;
            mSerialPort.Parity = Parity.None;
            mSerialPort.DataBits = 8;
            mSerialPort.StopBits = StopBits.One;
            mSerialPort.WriteTimeout = 1000;
            mSerialPort.ReadTimeout = 1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "1", "0");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "1", "2");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "1", "3");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "1", "4");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "2", "1");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "2", "0");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "2", "2");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "2", "3");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "2", "4");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "3", "1");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "3", "3");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "3", "0");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "3", "2");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "3", "3");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "4", "1");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "4", "0");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "4", "2");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "4", "3");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "4", "4");
        }

        private void button36_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "5", "1");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "5", "0");
        }

        private void button38_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "5", "2");
        }

        private void button39_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "5", "3");
        }

        private void button40_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "5", "4");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "6", "1");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "6", "0");
        }

        private void button33_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "6", "2");
        }

        private void button34_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "6", "3");
        }

        private void button35_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text;
            Light(Com, "6", "4");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "7", "1");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "7", "0");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "7", "2");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "7", "3");
        }

        private void button30_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "7", "4");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "8", "1");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "8", "0");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "8", "2");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "8", "3");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "8", "4");
        }

        private void button56_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "9", "1");
        }

        private void button57_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "9", "0");
        }

        private void button58_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "9", "2");
        }

        private void button59_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "9", "3");
        }

        private void button60_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "9", "4");
        }

        private void button51_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "10", "1");
        }

        private void button52_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "10", "0");
        }

        private void button53_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "10", "2");
        }

        private void button54_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "10", "3");
        }

        private void button55_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "10", "4");
        }

        private void button46_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "11", "1");
        }

        private void button47_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "11", "0");
        }

        private void button48_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "11", "2");
        }

        private void button49_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "11", "3");
        }

        private void button50_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "11", "4");
        }

        private void button41_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "12", "1");
        }

        private void button42_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "12", "0");
        }

        private void button43_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "12", "2");
        }

        private void button44_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "12", "3");
        }

        private void button45_Click(object sender, EventArgs e)
        {
            string Com = comboBox1.Text; Light(Com, "12", "4");
        }
    }
}
