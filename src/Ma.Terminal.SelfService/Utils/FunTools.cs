/* ***********************************************
* Author:          TanJun (32965926@qq.com)
* Created Time:    2015-02-02
* CopyRight:       Copyright (C) 2008-2015 深圳市宇川智能系统有限公司 All Rights Reserved
* NameSpace:       YC.Qloud.Common
* Description:     工具类
* ***********************************************/

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;

namespace Ma.Terminal.Utils
{
    public static class FunTools
    {
        #region HEX字符串转换为BYTES
        /// <summary>
        /// HEX字符串转换为BYTES
        /// </summary>
        /// <param name="inputstring"></param>
        /// <returns></returns>
        public static byte[] StrToHexBytes(string inputstring)
        {
            if ((inputstring.Length % 2) != 0)
            {
                inputstring += " ";
            }

            byte[] returnBytes = new byte[inputstring.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(inputstring.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }
        #endregion

        #region BYTES转换为HEX字符串
        /// <summary>
        /// BYTES转换为HEX字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexStr(byte[] bytes)
        {
            string returnStr = "";

            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }

            return returnStr;
        }

        /// <summary>
        /// BYTES转换为HEX字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexStr(byte[] bytes, bool Desc)
        {
            string returnStr = "";

            if (bytes != null)
            {
                for (int i = bytes.Length; i > 0; i--)

                {
                    returnStr += bytes[i - 1].ToString("X2");
                }
            }

            return returnStr;
        }

        /// <summary>
        /// BYTES转换为HEX字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexStr(byte[] bytes, int offset, int count)
        {
            string returnStr = "";

            if (bytes != null)
            {
                for (int i = offset; i < offset + count; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }

            return returnStr;
        }
        #endregion

        #region 指定长度BYTES转换为HEX字符串
        /// <summary>
        /// 指定长度BYTES转换为HEX字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexStr(int length, byte[] bytes)
        {
            string returnStr = "";

            if (bytes != null)
            {
                for (int i = 0; i < length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }

            return returnStr;
        }
        #endregion

        #region 转字符串转BCD码
        /// <summary>
        /// 转字符串转BCD码
        /// </summary>
        /// <param name="strTemp"></param>
        /// <returns></returns>
        public static byte[] ConvertToBCD(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//数字的二进制码最后1位是1则为奇数
                {
                    strTemp = strTemp + "0";//数位为奇数时后面补0
                }

                byte[] aryTemp = new byte[strTemp.Length / 2];

                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    aryTemp[i] = (Byte)(((strTemp[i * 2] - '0') << 4) | (strTemp[i * 2 + 1] - '0'));
                }
                return aryTemp;//高位在前
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 结构转为字节
        /// <summary>
        /// 结构转为字节
        /// </summary>
        /// <param name="Struct"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object Struct)
        {
            //得到结构体的大小
            int len = Marshal.SizeOf(Struct);
            //创建byte数组
            byte[] bytes = new byte[len];
            //分配结构体大小的内存空间
            IntPtr structs = Marshal.AllocHGlobal(len);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(Struct, structs, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structs, bytes, 0, len);
            //释放内存空间
            Marshal.FreeHGlobal(structs);

            return bytes;
        }

        /// <summary>
        /// 字节转换成结构
        /// </summary>
        /// <param name="mData"></param>
        /// <returns></returns>
        public static object StructFromData(byte[] mData, Type T)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(T);
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(mData, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, T);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }
        #endregion

        #region DES加密与解密
        /// <summary>
        /// 向量
        /// </summary>
        private static byte[] IV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="EncBytes">待加密的数据</param>
        /// <param name="KeyBytes">加密密钥8字节</param>
        /// <returns>加密成功返回加密后的数据，失败返回源数据</returns>
        public static byte[] EncryptDES(byte[] EncBytes, byte[] KeyBytes)
        {
            try
            {
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                DCSP.Key = KeyBytes;
                DCSP.IV = IV;
                DCSP.Mode = CipherMode.CBC;
                DCSP.Padding = PaddingMode.None;

                ICryptoTransform Encryptor = DCSP.CreateEncryptor();

                return Encryptor.TransformFinalBlock(EncBytes, 0, EncBytes.Length);
            }
            catch
            {
                return EncBytes;
            }
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecBytes">待解密的数据</param>
        /// <param name="KeyBytes">解密密钥8字节</param>
        /// <returns>解密成功返回解密后的数据，失败返源数据</returns>
        public static byte[] DecryptDES(byte[] DecBytes, byte[] KeyBytes)
        {
            try
            {
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                DCSP.Key = KeyBytes;
                DCSP.IV = IV;
                DCSP.Mode = CipherMode.CBC;
                DCSP.Padding = PaddingMode.None;

                ICryptoTransform Decryptor = DCSP.CreateDecryptor();

                return Decryptor.TransformFinalBlock(DecBytes, 0, DecBytes.Length);
            }
            catch
            {
                return DecBytes;
            }
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="EncBytes">待加密的数据</param>
        /// <param name="KeyBytes">加密密钥16字节</param>
        /// <param name="Mode">加密模式</param>
        /// <param name="Padding">补码模式</param>
        /// <returns>加密成功返回加密后的数据，失败返回源数据</returns>
        public static byte[] EncryptTDES(byte[] EncBytes, byte[] KeyBytes, CipherMode Mode, PaddingMode Padding)
        {
            try
            {
                TripleDESCryptoServiceProvider TDCSP = new TripleDESCryptoServiceProvider();

                TDCSP.Key = KeyBytes;
                TDCSP.IV = IV;
                TDCSP.Mode = Mode;
                TDCSP.Padding = Padding;

                ICryptoTransform DESEncrypt = TDCSP.CreateEncryptor();

                return DESEncrypt.TransformFinalBlock(EncBytes, 0, EncBytes.Length);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  3DES解密
        /// </summary>
        /// <param name="EncBytes">待解密的数据</param>
        /// <param name="KeyBytes">加密密钥16字节</param>
        /// <param name="Mode">加密模式</param>
        /// <param name="Padding">补码模式</param>
        /// <returns>解密成功返回加密后的数据，失败返回源数据</returns>
        public static byte[] DecryptTDES(byte[] DecBytes, byte[] KeyBytes, CipherMode Mode, PaddingMode Padding)
        {
            try
            {
                TripleDESCryptoServiceProvider TDCSP = new TripleDESCryptoServiceProvider();

                TDCSP.Key = KeyBytes;
                TDCSP.IV = IV;
                TDCSP.Mode = Mode;
                TDCSP.Padding = Padding;

                ICryptoTransform Decryptor = TDCSP.CreateDecryptor();

                return Decryptor.TransformFinalBlock(DecBytes, 0, DecBytes.Length);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 异或两个字节数组
        /// <summary>
        /// 异或
        /// </summary>
        /// <param name="B1"></param>
        /// <param name="B2"></param>
        /// <returns></returns>
        public static byte[] Xor(byte[] B1, byte[] B2)
        {
            if (B1.Length != B2.Length)
            {
                throw new System.Exception("长度不一致");
            }

            MemoryStream ms = new MemoryStream();

            for (int i = 0; i < B1.Length; i++)
            {
                int xor = B1[i] ^ B2[i];
                ms.WriteByte(BitConverter.GetBytes(xor)[0]);
            }

            return ms.ToArray();
        }
        #endregion

        #region GZIP压缩
        /// <summary>
        /// GZIP压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(byte[] data)
        {
            byte[] ret = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(ms,
                    System.IO.Compression.CompressionMode.Compress))
                {
                    //将数据写入基础流，同时会被压缩
                    gzip.Write(data, 0, data.Length);
                }
                ret = ms.ToArray();
            }
            return ret;
        }

        /// <summary>
        /// GZIP解压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(byte[] data)
        {
            byte[] ret = null;

            using (MemoryStream cms = new MemoryStream(data))
            {
                using (MemoryStream dms = new MemoryStream())
                {
                    using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(cms,
                    System.IO.Compression.CompressionMode.Decompress))
                    {
                        byte[] bytes = new byte[1024];
                        int len = 0;
                        //读取压缩流，同时会被解压
                        while ((len = gzip.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            dms.Write(bytes, 0, len);
                        }

                        ret = dms.ToArray();
                    }
                }
            }

            return ret;
        }
        #endregion
    }
}
