using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ma.Terminal.SelfService.Device.Printer
{
    public class PrinterApi
    {
        const string DLL_PATH = @"Api/PCP21CT.dll";

        //扫描打印机
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_ScanPrinter", SetLastError = true)]
        public static extern int CXCMD_ScanPrinter(ref int piSlot, ref int piID);

        //检测打印机是否准备就绪
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_TestUnitReady", SetLastError = true)]
        public static extern int CXCMD_TestUnitReady(int iSlot, int iID);

        //移动卡片到制定位置
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_LoadCard", SetLastError = true)]
        public static extern int CXCMD_LoadCard(int iSlot, int iID, int iDest, int iFlip, int iFilmlnit, int ilmmed);

        //移动卡片
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_MoveCard", SetLastError = true)]
        public static extern int CXCMD_MoveCard(int iSlot, int iID, int iDest, int iFlip, int iFilmlnit, int ilmmed);

        //初始化打印机即重启打印机
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_RezeroUnit", SetLastError = true)]
        public static extern int CXCMD_RezeroUnit(int iSlot, int iID, int iAction);

        //接触式控制
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_ICControl", SetLastError = true)]
        public static extern int CXCMD_ICControl(int iSlot, int iID, int ilcType, int iAction);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CXCMD_ModeSense", SetLastError = true)]
        public static extern int CXCMD_ModeSense(int iSlot, int iID, int iPC, int iPage, Byte[] pbyBuffer);
    }
}
