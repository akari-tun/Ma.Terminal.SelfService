using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Ma.Terminal.SelfService.Device.Reader
{
    public class ReaderApi
    {
        public enum StatusEnum
        {
            [Description("执行成功")]
            IFD_OK = 0,
            [Description("卡片类型不对")]
            IFD_ICC_TypeError =	-1,
            [Description("无卡")]
            IFD_ICC_NoExist = -2,
            [Description("有卡未上电")]
            IFD_ICC_NoPower = -3,	    
            [Description("卡片无应答")]
            IFD_ICC_NoResponse = -4,	    
            [Description("读卡器连接错")]
            IFD_ConnectError = -11,
            [Description("未建立连接(没有执行打开设备函数)")]
            IFD_UnConnected = -12,
            [Description("不支持该命令")]
            IFD_BadCommand = -13,
            [Description("命令参数错")]
            IFD_ParameterError = -14,
            [Description("信息校验和出错")]
            IFD_CheckSumError =	-15
        }

        const string DLL_PATH = @"Api/HDSSSE32.dll";

        /// <summary>
        /// 打开指定的电脑接口
        /// </summary>
        /// <param name="dev_Name"> 以 0 结尾的字符串用于描述接口名字,如”USB1”</param>
        /// <returns>
        /// 其值为电脑接口句柄,大于 0 表示连接设备成功,否则表示连接失败(值无意 义).
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "ICC_Reader_Open")]
        public static extern IntPtr ICC_Reader_Open(StringBuilder dev_Name);

        /// <summary>
        /// 关闭已打开的电脑接口
        /// </summary>
        /// <param name="ReaderHandle">接口句柄</param>
        /// <returns>
        /// 等于 0 表示关闭设备成功,否则表示关闭失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "ICC_Reader_Close")]
        public static extern StatusEnum ICC_Reader_Close(IntPtr ReaderHandle);

        //读磁条卡
        [DllImport(DLL_PATH, EntryPoint = "Rcard")]
        public static extern StatusEnum Rcard(IntPtr ReaderHandle, byte ctime, int track, byte[] rlen, StringBuilder data);

        /// <summary>
        /// 蜂鸣
        /// </summary>
        /// <param name="ReaderHandle">接口句柄</param>
        /// <param name="time">蜂鸣的时间,以毫秒为单位</param>
        /// <returns>
        /// 等于 0 表示执行成功,否则表示执行失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "ICC_PosBeep")]
        public static extern StatusEnum ICC_PosBeep(IntPtr ReaderHandle, byte time);

        /// <summary>
        /// 设置读写器为读 typeA 卡
        /// </summary>
        /// <param name="ReaderHandle">设备句柄</param>
        /// <returns>
        /// 等于 0 表示执行成功,否则表示执行失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_SetTypeA")]
        public static extern StatusEnum PICC_Reader_SetTypeA(IntPtr ReaderHandle);

        /// <summary>
        /// 选择卡片
        /// </summary>
        /// <param name="ReaderHandle">设备句柄</param>
        /// <param name="cardtype">卡片类型（0x41 表示 typeA 及M1 卡，0x42 表示 typeB卡） </param>
        /// <returns>
        /// 等于 0 表示执行成功,否则表示执行失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_Select")]
        public static extern StatusEnum PICC_Reader_Select(IntPtr ReaderHandle, byte cardtype);

        /// <summary>
        /// typea & M1 请求卡片
        /// </summary>
        /// <param name="ReaderHandle">设备句柄</param>
        /// <returns>
        /// 等于 0 表示执行成功,否则表示执行失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_Request")]
        public static extern StatusEnum PICC_Reader_Request(IntPtr ReaderHandle);


        /// <summary>
        /// 防碰撞 typea M1卡片
        /// </summary>
        /// <param name="ReaderHandle">设备句柄</param>
        /// <param name="uid">输出参数（卡片 序列号）</param>
        /// <returns>
        /// 等于 0 表示执行成功,否则表示执行失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_anticoll")]
        public static extern StatusEnum PICC_Reader_anticoll(IntPtr ReaderHandle, byte[] uid);

        //注意：输入的是12位的密钥，例如12个f，但是password必须是6个字节的密钥，需要用StrToHex函数处理。
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_Authentication_Pass")]
        public static extern StatusEnum PICC_Reader_Authentication_Pass(IntPtr ReaderHandle,
                                                                    byte Mode,
                                                                    byte SecNr,
                                                                    byte[] PassWord);
        //读卡
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_Read")]
        public static extern StatusEnum PICC_Reader_Read(IntPtr ReaderHandle, byte Addr, byte[] Data);
        //写卡
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_Write")]
        public static extern StatusEnum PICC_Reader_Write(IntPtr ReaderHandle, byte Addr, byte[] Data);

        //将字符命令流转为16进制流
        [DllImport(DLL_PATH, EntryPoint = "StrToHex")]
        public static extern StatusEnum StrToHex(StringBuilder strIn, int len, Byte[] HexOut);

        //将16进制流命令转为字符流
        [DllImport(DLL_PATH, EntryPoint = "HexToStr")]
        public static extern StatusEnum HexToStr(Byte[] strIn, int inLen, StringBuilder strOut);


        //接触CPU
        [DllImport(DLL_PATH, EntryPoint = "ICC_Reader_pre_PowerOn")]
        public static extern int ICC_Reader_pre_PowerOn(IntPtr ReaderHandle, byte SLOT, byte[] Response);//上电 返回数据长度 失败小于0

        [DllImport(DLL_PATH, EntryPoint = "ICC_Reader_Application")]
        public static extern int ICC_Reader_Application(IntPtr ReaderHandle, byte SLOT, int Lenth_of_Command_APDU, byte[] Command_APDU, byte[] Response_APDU);  //type a/b执行apdu命令 返回数据长度 失败小于0

        /// <summary>
        /// 非接TypeA CPU卡上电复位
        /// </summary>
        /// <param name="ReaderHandle">设备句柄</param>
        /// <param name="Response">输出上电成功返回的卡片复位信息(ATR)</param>
        /// <returns>
        /// 返回数据长度 失败小于0
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_PowerOnTypeA")]
        public static extern int PICC_Reader_PowerOnTypeA(IntPtr ReaderHandle, byte[] Response);

        /// <summary>
        /// TypeA/B 非接 CPU 卡执行 apdu 命令 
        /// </summary>
        /// <param name="ReaderHandle">设备句柄</param>
        /// <param name="Lenth_of_Command_APDU">apdu命令长度</param>
        /// <param name="Command_APDU">apdu命令数据</param>
        /// <param name="Response_APDU">apdu 命令执行后,响应的数据</param>
        /// <returns>
        /// 大于 0 表示执行成功,其值为 Response_APDU 的数据长度.否则表示执行失败
        /// </returns>
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_Application")]
        public static extern int PICC_Reader_Application(IntPtr ReaderHandle, int Lenth_of_Command_APDU, byte[] Command_APDU, byte[] Response_APDU);  //type a/b执行apdu命令 返回数据长度 失败小于0


        //社保卡
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_SICARD")]
        public static extern StatusEnum PICC_Reader_SICARD(IntPtr ReaderHandle,
                                                           StringBuilder sbkh,
                                                           StringBuilder xm,
                                                           StringBuilder xb, StringBuilder mz, StringBuilder csrq,
                                                           StringBuilder shbzhm, StringBuilder fkrq, StringBuilder kyxq,
                                                           StringBuilder err);

        //银行卡
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_CardInfo")]
        public static extern StatusEnum PICC_Reader_CardInfo(IntPtr ReaderHandle,
                                                             byte[] sn,
                                                             byte[] date,
                                                             byte[] kh,
                                                             byte[] kh_len,
                                                             int iType);



        //身份证
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_ReadIDMsg")]
        public static extern StatusEnum PICC_Reader_ReadIDMsg(IntPtr RHandle, StringBuilder pBmpFile, StringBuilder pName, StringBuilder pSex, StringBuilder pNation, StringBuilder pBirth, StringBuilder pAddress, StringBuilder pCertNo, StringBuilder pDepartment, StringBuilder pEffectData, StringBuilder pExpire, StringBuilder pErrMsg);


        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_ID_ReadUID")]
        public static extern int PICC_Reader_ID_ReadUID(IntPtr ReaderHandle, StringBuilder UID);//上电 返回数据长度 失败小于0


        // 读身份证 
        [DllImport(DLL_PATH, EntryPoint = "PICC_Reader_ReadIDCard")]
        public static extern StatusEnum PICC_Reader_ReadIDCard(IntPtr ReaderHandle, StringBuilder err);

        // 获取证件类型
        [DllImport(DLL_PATH, EntryPoint = "GetCardType",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetCardType();

        // 姓名(类型为1时表示：外国人中文姓名)
        [DllImport(DLL_PATH, EntryPoint = "GetName",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetName(StringBuilder name);

        // 性别
        [DllImport(DLL_PATH, EntryPoint = "GetSex",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetSex(StringBuilder sex);

        // 民族
        [DllImport(DLL_PATH, EntryPoint = "GetNation",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetNation(StringBuilder Nation);

        // 出生日期
        [DllImport(DLL_PATH, EntryPoint = "GetBirth",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetBirth(StringBuilder Birth);

        // 住址
        [DllImport(DLL_PATH, EntryPoint = "GetAddress",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetAddress(StringBuilder Address);

        // 公民身份证号码(类型为1时表示：外国人居留证号码)
        [DllImport(DLL_PATH, EntryPoint = "GetCertNo",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetCertNo(StringBuilder CertNo);

        // 签发机关
        [DllImport(DLL_PATH, EntryPoint = "GetDepartemt",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetDepartemt(StringBuilder Departemt);

        // 有效起始日期
        [DllImport(DLL_PATH, EntryPoint = "GetEffectDate",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetEffectDate(StringBuilder EffectDate);

        // 有效截止日期
        [DllImport(DLL_PATH, EntryPoint = "GetExpireDate",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetExpireDate(StringBuilder ExpireDate);


        // 生成照片
        [DllImport(DLL_PATH, EntryPoint = "GetBmpFile",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetBmpFile(StringBuilder pBmpfilepath);


        // 外国人英文姓名
        [DllImport(DLL_PATH, EntryPoint = "GetEnName",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetEnName(StringBuilder EnName);


        // 外国人国籍代码 符合GB/T2659-2000规定
        [DllImport(DLL_PATH, EntryPoint = "GetNationalityCode",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetNationalityCode(StringBuilder NationalityCode);

        // 港澳台通行证号码
        [DllImport(DLL_PATH, EntryPoint = "GetTXZHM",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetTXZHM(StringBuilder txzhm);

        // 港澳台通行证签发次数
        [DllImport(DLL_PATH, EntryPoint = "GetTXZQFCS",
        CallingConvention = CallingConvention.StdCall)]
        public static extern StatusEnum GetTXZQFCS(StringBuilder txzqfcs);
    }
}
