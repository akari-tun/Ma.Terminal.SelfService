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
        public bool IsWaiting { get; set; }

        public bool IsReady(bool needReset = false)
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
                case -17010688:
                    LastError = "卡色带，正在重启打印机";
                    Task.Run(() =>
                    {
                        PrinterApi.CXCMD_RezeroUnit(piSlot, piID, 0);
                    });
                    break;
                default:
                    LastError = GetResultMsg((uint)Math.Abs(result));
                    break;
            }

            _logger.Trace(LastError);

            return result == 0;
        }

        public bool MoveToRfPosition()
        {
            int result = -1;

            if (IsReady())
            {
                result = PrinterApi.CXCMD_LoadCard(piSlot, piID, 0, 0, 0, 0);

                if (result != 0)
                {
                    LastError = GetResultMsg((uint)Math.Abs(result)); ;
                    _logger.Trace(LastError);
                    return false;
                }

                LastError = "成功移动卡片到读卡位";
                _logger.Trace(LastError);
            }

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
                LastError = GetResultMsg((uint)Math.Abs(result));
                _logger.Trace(LastError);
            }
        }

        public async Task<bool> WaitPrintEnd(int timeout)
        {
            IsWaiting = true;
            bool isSuccess = false;
            int successCount = 10;

            while (timeout > 0 && !isSuccess && IsWaiting)
            {
                if (timeout % 1000 == 0)
                {
                    _logger.Trace($"Wait printed timeout {timeout / 1000} ");
                }

                if (PrinterApi.CXCMD_TestUnitReady(piSlot, piID) == 0)
                {
                    successCount--;
                    isSuccess = successCount <= 0;
                }
                else
                {
                    successCount = 10;
                }

                timeout -= 200;
                await Task.Delay(200);
            }

            _logger.Trace($"Wait printed is {(isSuccess ? "success" : "timeout")}");
            return isSuccess;
        }

 
        private string GetResultMsg(uint nres)
        {
            string str = "";
            switch (nres)
            {
                case 0x00000000: str = "操作成功"; break;
                case 0x00000002: str = "正在进卡"; break; 
                case 0x00000004: str = "正在转印中"; break;
                case 0x00000005: str = "正在打印中"; break;
                case 0x02010082: str = "打印机未连接.请检测打印机连接是否正常"; break;
                case 0x02000006: str = "未扫描到打印机.请检测打印机连接是否正常"; break;
                case 0x0102D000: str = "No Card_进卡盒无卡.请放卡！"; break;
                case 0x0102D100: str = "Door Open_打印机门打开,或清洁辊未安装."; break;
                case 0x0102D300: str = "Busy of Transporting_打印机忙于传输卡或编码。注意：函数不会返回此错误。控制功能返回正值为BUSY."; break;
                case 0x0102D400: str = "Busy of Printing_打印机忙于打印。注意：此错误不会从函数中返回。控制功能返回正值为BUSY."; break;
                case 0x0102D500: str = "Busy of Transporting and Printing_打印机忙于传输和打印卡，或者正在转印。注意：此错误不会从函数中返回。控制功能返回正值为BUSY。"; break;
                case 0x0102DA00: str = "Preheating_打印机正在加热中.请稍后！"; break;
                case 0x0102DB00: str = "Initializing_打印机正在初始化.请稍后！"; break;
                case 0x0102DC00: str = "Testing or Cleaning_打印机正在进行离线测试或清洁."; break;
                case 0x0102DD00: str = "On Setting or Transport Mode_打印机处于设置模式或传输模式."; break;
                case 0x0102DE00: str = "Not Ready for Download_由于打印机未处于下载模式,因此无法进行固件下载."; break;
                case 0x0102FD00: str = "Sleeping_打印机处于省电模式.注意：如果要退出该模式，需要初始化打印机，即在RESET按钮后按ENTER按钮或发送REZERO命令."; break;
                case 0x0102FE00: str = "Password Error_密码认证未完成."; break;

                case 0x01039000: str = "Jam(Hopper)_进卡失败.重新放置卡片,然后初始化打印机，即在RESET按钮后按ENTER按钮或发送REZERO命令"; break;
                case 0x01039100: str = "Jam(TurnOver)_卡在翻转模块位置."; break;
                case 0x01039200: str = "Jam(MG)_卡在写磁模块位置."; break;
                case 0x01039300: str = "Jam(Transfer)_卡在转印位置."; break;
                case 0x01039400: str = "Jam(Discharge)_卡在出卡口位置."; break;
                case 0x0103A100: str = "Media Search_打印机查找转印膜失败."; break;
                case 0x0103AD00: str = "MG Write Error_打印机写磁失败."; break;
                case 0x0103AE00: str = "MG Read Error_打印机读磁失败."; break;
                case 0x0103B000: str = "Ink Error_打印机不支持该色带."; break;
                case 0x0103B100: str = "Ink Search_打印机查找色带失败."; break;

                case 0x01044400: str = "Hardware (Printing)_打印机超时."; break;
                case 0x0104AB00: str = "MG Mechanical_写磁模块发生机械故障."; break;
                case 0x0104AC00: str = "MG Hardware_写磁模块发送硬件错误."; break;
                case 0x0104BF00: str = "EXT2. Communicate_更新覆膜机固件时发生错误."; break;
                case 0x0104C100: str = "Cam Error_加热辊轴发生错误."; break;
                case 0x0104D800: str = "Hardware (Initializing)_初始化时检测到电路故障，或者在更新覆膜机固件写入失败."; break;
                case 0x0104F000: str = "TR Overheat_再转印加热辊温度过高."; break;
                case 0x0104F100: str = "TR Heater_再转印加热辊故障."; break;
                case 0x0104F200: str = "TR Thermister_再转印加热辊热敏电阻故障."; break;
                case 0x0104F300: str = "RR Overheat_弯曲校正加热辊温度过高."; break;
                case 0x0104F400: str = "RR Heater_弯曲矫正加热辊出现故障."; break;
                case 0x0104F500: str = "RR Thermister_弯曲校正加热辊发生热敏电阻故障."; break;
                case 0x0104F600: str = "Overcool_打印机的温度太低."; break;
                case 0x0104F800: str = "Head Overheat_打印头温度过高."; break;

                case 0x01051A00: str = "Parameter List Length Error_命令无效.CDB或Page Data中的参数列表长度值无效."; break;
                case 0x01052000: str = "Invalid Command Operation Code_命令无效.CDB中的操作代码无效."; break;
                case 0x01052400: str = "Illegal Field in CDB_命令的内容无效.CDB中的数据无效"; break;
                case 0x01052600: str = "Invalid Field in Parameter List_命令的内容无效.页面数据中的数据无效."; break;
                case 0x01052700: str = "Invalid Color Code in CDB_指定的色带无效."; break;
                case 0x01052A00: str = "Command Sequence Error_发送命令顺序不对."; break;
                case 0x01052B00: str = "MG Data Error_发送读写磁数据无效."; break;
                case 0x01052C00: str = "IC Encoder not installed_打印机未安装IC模块."; break;
                case 0x01052D00: str = "MG Encoder not installed_打印机未安装读写磁模块."; break;
                case 0x0105FB00: str = "Invalid Download Data_从主机下载数据无效."; break;

                case 0x01062800: str = "Medium Changed_打印机已通过按下RESET按钮进行初始化."; break;
                case 0x01062900: str = "Power On or Bus Device Reset Occurred_打印机已通过打开打印机电源进行初始化."; break;


                case 0x0142A200: str = "Media Run Out_转印膜用完."; break;
                case 0x0142B200: str = "Ink Run Out_色带用完."; break;

                case 0x0104C200: str = "HR Overheat_弯曲校正加热辊或再转印加热辊过热."; break;
                case 0x0103A800: str = "MG Write Error in Self Test_写磁自检发生错误."; break;

                case 0x01052E00: str = "Option Not Installed_模块未安装."; break;
                case 0x01052100: str = "Security Key is already set_无法注册新的安全密钥，因为该密钥已设置."; break;
                case 0x01052300: str = "Security key is not set_安全密钥未注册."; break;
                case 0x01052200: str = "Invalid Security Key_安全密钥无效."; break;

                case 0x0104C300: str = "Detect Power Interrupt_24V电源中断."; break;

                case 0x01039500: str = "Jam(Retransfer)_转印时发生卡卡错误."; break;

                default: str = "发生其他错误." + nres.ToString("X6"); ; break;


            }
            
            return str;
        }
    }
}
