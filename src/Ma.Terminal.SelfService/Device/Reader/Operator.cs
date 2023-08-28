using Ma.Terminal.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Ma.Terminal.SelfService.Device.Reader
{
    public class Operator
    {
        const string DEV_NAME_STR = "USB1";
        StringBuilder DEV_NAME = new StringBuilder(DEV_NAME_STR);
        IntPtr _handler;
        Logger _logger = LogManager.GetCurrentClassLogger();

        public string LastError { get; set; }
        public ReaderApi.StatusEnum Status { get; set; }

        public bool OpenCard(out byte[] uid)
        {
            uid = new byte[4];
            _handler = ReaderApi.ICC_Reader_Open(DEV_NAME);

            if (_handler.ToInt32() <= 0)
            {
                LastError = "读卡器打开失败！";
                _logger.Error(LastError);
                return false;
            }

            //非接基本函数
            Status = ReaderApi.PICC_Reader_SetTypeA(_handler);
            if (Status != ReaderApi.StatusEnum.IFD_OK)
            {
                LastError = "设置为A卡模式失败";
                _logger.Error(LastError);
                return false;
            }

            Status = ReaderApi.PICC_Reader_Request(_handler);
            if (Status != ReaderApi.StatusEnum.IFD_OK)
            {
                LastError = "请求卡片失败";
                _logger.Error(LastError);
                return false;
            }
            
            Status = ReaderApi.PICC_Reader_anticoll(_handler, uid);
            if (Status != ReaderApi.StatusEnum.IFD_OK)
            {
                LastError = "防碰撞失败";
                _logger.Error(LastError);
                return false;
            }

            Status = ReaderApi.PICC_Reader_Select(_handler, 0x41);
            if (Status != ReaderApi.StatusEnum.IFD_OK)
            {
                LastError = "选卡失败";
                _logger.Error(LastError);
                return false;
            }

            byte[] ATR = new byte[50];
            int len = ReaderApi.PICC_Reader_PowerOnTypeA(_handler, ATR);
            if (len < 0)
            {
                Status = (ReaderApi.StatusEnum)len;
                LastError = "非接上电失败";
                _logger.Error(LastError);
                return false;
            }

            _logger.Trace($"Cpu card init success");
            LastError = "Cpu卡上电成功";
            return true;
        }

        public bool ExecuteApdu(byte[] cmd, out byte[] rsp, List<string> sws)
        {
            byte[] buff = new byte[255];

            _logger.Trace($"apdu exec --> len:{cmd.Length} rsp:[{FunTools.BytesToHexStr(cmd)}]");
            int len = ReaderApi.PICC_Reader_Application(_handler, cmd.Length, cmd, buff);
            if (len < 2)
            {
                rsp = new byte[0];
                _logger.Trace($"apdu result --> fail!");
                LastError = "APDU命令执行失败";
                return false;
            }

            rsp = new byte[len];
            Array.Copy(buff, rsp, len);

            string result = FunTools.BytesToHexStr(rsp, rsp.Length - 2, 2);
            _logger.Trace($"apdu result --> len:{len} rsp:[{result}]");

            bool ret = false;

            foreach (var item in sws)
            {
                if (item.Split(',').Length > 1)
                {
                    string[] ss = item.Split(',');
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (result.Equals(ss[i]))
                        {
                            ret = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (result.Equals(item))
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }
    }
}
