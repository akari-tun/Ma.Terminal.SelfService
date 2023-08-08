using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Device.Light
{
    public class Operator : UartBase
    {
        public void Light(int index)
        {
            byte passage = 0;

            //  1~20 用第一货道  亮灯一
            // 21~40 用第四货到  亮登三
            // 41~60 用第二货道  亮灯一
            // 61~80 用第三货道  亮灯三
            if (index >= 1 && index <= 20)
            {
                passage = 0xA0 + 1;
            }
            else if (index >= 21 && index <= 40)
            {
                passage = 0xA0 + 3;
            }
            else if (index >= 41 && index <= 60)
            {
                passage = 0xA0 + 1;
            }
            else if (index >= 61 && index <= 80)
            {
                passage = 0xA0 + 3;
            }

            _command = new byte[] { passage, 0x01 };

            SendCommand();

            _command = null;
        }
    }
}
