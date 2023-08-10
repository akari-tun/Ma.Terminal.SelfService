using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Device
{
    public abstract class UartBase : IUart
    {
        System.IO.Ports.SerialPort mSerialPort = null;

        protected byte[] _command;

        public int Port { get; set; }
        public int Baudrate { get; set; }

        public void Init()
        {
            mSerialPort = new System.IO.Ports.SerialPort
            {
                BaudRate = Baudrate,
                Parity = System.IO.Ports.Parity.None,
                DataBits = 0x08,
                StopBits = System.IO.Ports.StopBits.One,
                PortName = "COM" + Port,
                WriteTimeout = 1000,
                ReadTimeout = 1000
            };
        }

        public void SendCommand()
        {
            if (_command != null && _command.Length > 0)
            {
                try
                {
                    mSerialPort.Open();
                    mSerialPort.Write(_command, 0, _command.Length);
                }
                catch (Exception)
                {

                }
                finally
                {
                    mSerialPort.Close();
                }
            }
        }
    }
}
