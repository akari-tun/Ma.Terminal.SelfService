﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Device
{
    public interface IUart
    {
        public int Port { get; set; }
        public int Baudrate { get; set; }

        public void SendCommand();
    }
}
