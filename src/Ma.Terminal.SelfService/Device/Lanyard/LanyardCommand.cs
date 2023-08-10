using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Device.Lanyard
{
    public class LanyardCommand
    {
        public byte[] Head { get; set; } = new byte[] { 0x0D, 0x24 };
        public byte[] Len { get; set; } = new byte[] { 0x28, 0x00 };
        public byte FunCode { get; set; } = 0x60;
        public byte AddressCode { get; set; } = 0x00;
        public byte Passage { get; set; }
        public byte CheckTime { get; set; } = 0x05;
        public byte LessItemCheckTime { get; set; } = 0x03;
        public string OrderId { get; set; }
        public byte[] Reserver { get; set; } = new byte[14];
        public byte CRC { get; set; } = 0x4F;
        public byte[] End { get; set; } = new byte[] { 0x0D, 0x0A };

        public byte[] ToArray()
        {
            byte[] data = null;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ms.Write(Head, 0, Head.Length);
                ms.Write(Len, 0, Len.Length);
                ms.WriteByte(FunCode);
                ms.WriteByte(AddressCode);
                ms.WriteByte(Passage);
                ms.WriteByte(CheckTime);
                ms.WriteByte(LessItemCheckTime);

                byte[] orderId = Encoding.ASCII.GetBytes(OrderId);
                if (orderId.Length >= 14)
                {
                    ms.Write(orderId, 0, 14);
                }
                else
                {
                    ms.Write(orderId, 0, orderId.Length);
                    ms.Write(new byte[14 - orderId.Length], 0, 14 - orderId.Length);
                }

                ms.Write(Reserver, 0, Reserver.Length);
                ms.WriteByte(CRC);
                ms.Write(End, 0, Head.Length);
                data = ms.ToArray();
            }

            return data;
        }
    }
}
