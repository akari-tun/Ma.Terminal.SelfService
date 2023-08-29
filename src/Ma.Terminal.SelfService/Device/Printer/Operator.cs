using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfService.Device.Printer
{
    public class Operator
    {
        int piSlot = 1, piID = 2;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public string LastError { get; set; }

        public bool MoveToRfPosition()
        {
            int result = -1;

            result = PrinterApi.CXCMD_ScanPrinter(ref piSlot, ref piID);//扫描打印机，返回值为0，返回参数piSlot,piID

            if (result != 0)
            {
                LastError = "连接打印机错误";
                _logger.Error(LastError);
                return false;
            }

            _logger.Trace("找到打印机器");

            result = PrinterApi.CXCMD_TestUnitReady(piSlot, piID);

            switch (result)
            {
                case 0:
                    LastError = "准备就绪";
                    break;
                case -33620098:
                    LastError = "打印机未连接";
                    break;
                case -16964352:
                    LastError = "机器初始化，请等待";
                    break;
                case -16964096:
                    LastError = "加热中，请等待";
                    break;
                case -16961536:
                    LastError = "无卡，请放卡";
                    break;
                case -16961792:
                    LastError = "门开启，请重启";
                    break;
                default:
                    LastError = $"错误码：{result}";
                    break;
            }

            _logger.Trace(LastError);

            result = PrinterApi.CXCMD_LoadCard(piSlot, piID, 0, 0, 0, 0);

            if (result != 0)
            {
                LastError = "移动卡片到RF位置失败";
                _logger.Trace(LastError);
                return false;
            }

            LastError = "成功移动卡片到读卡位";
            _logger.Trace(LastError);
            return true;
        }

        public void ExitCard()
        {
            int result = PrinterApi.CXCMD_MoveCard(piSlot, piID, 4, 0, 0, 0);//出卡
            if (result == 0)
            {
                LastError = "排卡成功";
                _logger.Trace(LastError);
            }
            else
            {
                LastError = "排卡失败";
                _logger.Trace(LastError);
            }
        }

        public async Task<bool> WaitPrintEnd(int timeout, Action<int> notify)
        {
            bool isSuccess = false;

            while (timeout > 0 && !isSuccess)
            {
                if (timeout % 1000 == 0) notify?.Invoke(timeout / 1000);

                isSuccess = PrinterApi.CXCMD_TestUnitReady(piSlot, piID) == 0;
                timeout -= 200;
                await Task.Delay(200);
            }

            return isSuccess;
        }
    }
}
