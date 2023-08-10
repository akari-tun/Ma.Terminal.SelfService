using Ma.Terminal.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Device.Lanyard
{
    public class Operator : UartBase
    {
        public void RollLanyard(int index, string orderId)
        {
            byte passage = 0;

            //  1~20 用第一货道  亮灯一
            // 21~40 用第四货到  亮登三
            // 41~60 用第二货道  亮灯一
            // 61~80 用第三货道  亮灯三
            if (index >= 1 && index <= 20)
            { 
                passage = 1;
            }
            else if (index >= 21 && index <= 40)
            {
                passage = 4;
            }
            else if (index >= 41 && index <= 60)
            {
                passage = 2;
            }
            else if (index >= 61 && index <= 80)
            {
                passage = 3;
            }

            var cmd = new LanyardCommand() { Passage = passage, OrderId = orderId };

            _command = cmd.ToArray();
            var aa = FunTools.BytesToHexStr(_command);
            SendCommand();

            _command = null;
        }

    }
}
