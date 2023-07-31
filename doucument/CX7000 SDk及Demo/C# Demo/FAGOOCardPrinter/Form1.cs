using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing.Printing;

namespace FAGOOCardPrinter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        int result;
        int piSlot = 1, piID = 2;

        //扫描打印机
        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_ScanPrinter", SetLastError = true)]
        public static extern int CXCMD_ScanPrinter(ref int piSlot, ref int piID);


        //检测打印机是否准备就绪
        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_TestUnitReady", SetLastError = true)]
        public static extern int CXCMD_TestUnitReady(int iSlot, int iID);


  

        //移动卡片到制定位置
        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_LoadCard", SetLastError = true)]
        public static extern int CXCMD_LoadCard(int iSlot, int iID, int iDest, int iFlip, int iFilmlnit, int ilmmed);



        //移动卡片
        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_MoveCard", SetLastError = true)]
        public static extern int CXCMD_MoveCard(int iSlot, int iID, int iDest, int iFlip, int iFilmlnit, int ilmmed);



        //初始化打印机即重启打印机
        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_RezeroUnit", SetLastError = true)]
        public static extern int CXCMD_RezeroUnit(int iSlot, int iID, int iAction);



        //接触式控制
        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_ICControl", SetLastError = true)]
        public static extern int CXCMD_ICControl(int iSlot, int iID, int ilcType, int iAction);


        [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_ModeSense", SetLastError = true)]
        public static extern int CXCMD_ModeSense(int iSlot, int iID, int iPC, int iPage, Byte[] pbyBuffer);


       //读取信息
       // [DllImport("PCP21CT.dll", CharSet = CharSet.Ansi, EntryPoint = "CXCMD_StandardInquiry", SetLastError = true)]
       //  public static extern int CXCMD_StandardInquiry(int iSlot, int iID, Byte[] pbyBuffer);




       // int CXCMD_ModeSense(int iSlot,int iID,int iPC,int iPage,BYTE *pbyBuffer)


        //int CXCMD_StandardInquiry(int iSlot,int iID,BYTE *pbyBuffer)


        private void button1_Click(object sender, EventArgs e)
        {
            result = CXCMD_ScanPrinter(ref piSlot, ref piID);//扫描打印机，返回值为0，返回参数piSlot,piID
            if (result == 0)
                MessageBox.Show("扫描打印机成功");
            else
            {
                MessageBox.Show("连接打印机错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i;

            i = CXCMD_TestUnitReady(piSlot, piID);
           

            if (i == 0)
            {
                MessageBox.Show("准备就绪");
            }

            else if (i == -33620098)
            {
                MessageBox.Show("打印机未连接");
            }

            else if (i == -16964352)
            {
                MessageBox.Show("机器初始化，请等待");
            }
            else if (i == -16964096)
            {
                MessageBox.Show("加热中，请等待");
            }

            else if (i == -16961536)//转换为16进制-1 02 d0 00
            {
                MessageBox.Show("无卡，请放卡");
            }

            else if (i == -16961792)
            {
                MessageBox.Show("门开启，请重启");
            }

            else
            {
                MessageBox.Show(i.ToString());
            }

            //参考76页文档里，将值转换为16进制 对比错误信息，
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            result = CXCMD_LoadCard(piSlot, piID, 1, 0, 0, 0);
            if (result == 0)
            {
                MessageBox.Show("移动卡片到接触式位置成功");
            }
            else
            {
                MessageBox.Show("移动卡片到接触式位置失败");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
          result=CXCMD_ICControl(piSlot, piID, 0, 0);//读头落下
          if (result == 0)
          {
              MessageBox.Show("接触式读头落下成功");
          }
          else
          {
              MessageBox.Show("接触式读头落下失败");
          }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //整合读卡器C#
        }

        private void button5_Click(object sender, EventArgs e)
        {
            result = CXCMD_ICControl(piSlot, piID, 0, 1);//读头抬起
            if (result == 0)
            {
                MessageBox.Show("接触式读头抬起成功");
            }
            else
            {
                MessageBox.Show("接触式读头抬起失败");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            result = CXCMD_RezeroUnit(piSlot, piID, 0);
            if (result == 0)
            {
                MessageBox.Show("重启成功");
            }
            else
            {
                MessageBox.Show("重启失败");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
           result=CXCMD_MoveCard(piSlot, piID, 4, 0, 0, 0);//出卡
           if (result == 0)
           {
               MessageBox.Show("排卡成功");
           }
           else
           {
               MessageBox.Show("排卡失败");
           }
        }

       

        private void button7_Click(object sender, EventArgs e)
        {
            printDocument1.PrinterSettings.PrinterName = "CX-7000 U1";
            printDocument1.DefaultPageSettings.Margins.Left = 0;
            printDocument1.DefaultPageSettings.Margins.Top = 0;
            printDocument1.DefaultPageSettings.Margins.Right = 0;
            printDocument1.DefaultPageSettings.Margins.Bottom = 0;
            printDocument1.DocumentName = "社保卡打印";
            printDocument1.Print();
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image colorImg = null;
            Rectangle rect;
            IntPtr hPrinterDC;

            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            //---------------------------------------------
            hPrinterDC = e.Graphics.GetHdc();
            e.Graphics.ReleaseHdc(hPrinterDC);

            colorImg = Image.FromFile("1.jpg");

            rect = new Rectangle(720, 160, 260, 350);
            e.Graphics.DrawImage(colorImg, rect);

            //---------------------------------------------
            e.Graphics.DrawString(Name, new Font(new FontFamily("黑体"), 10), System.Drawing.Brushes.Black, 160, 170);//姓名
           
           
            //
        }

        private void button10_Click(object sender, EventArgs e)
        {

            result = CXCMD_LoadCard(piSlot, piID, 0, 0, 0, 0); ;
            if (result == 0)
            {
                MessageBox.Show("移动卡片到RF位置成功");
            }
            else
            {
                MessageBox.Show("移动卡片到RF位置失败");
            }
        }
       
    }
}
