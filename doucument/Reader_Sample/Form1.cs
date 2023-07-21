using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace Reader_Sample
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m1 formM1 = new m1();
            formM1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sfz formSFZ = new sfz();
            formSFZ.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            CPU formcpu = new CPU();
            formcpu.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder dev_name = new StringBuilder("USB1");
            byte[] sn = new byte[4];
            byte[] date = new byte[10];
            byte[] kh = new byte[255]; //这里是完整的二磁道数据，只需要卡号的自行截取 = 号之前的数据即可
            byte[] kh_len = new byte[2];
            int iType = 1;
            int RHandle = -1;

            RHandle = dev.ICC_Reader_Open(dev_name);
            if (RHandle <= 0)
            {
                textBox1.Text = "设备未连接";
                return;
            }
            int nRt = dev.PICC_Reader_CardInfo(RHandle, sn, date, kh, kh_len, iType);
            if (nRt != 0)
            {
                textBox1.Text = "读卡失败";
                return;
            }

            textBox1.Text = System.Text.Encoding.Default.GetString(kh);

            dev.ICC_Reader_Close(RHandle);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StringBuilder dev_name = new StringBuilder("USB1");
            StringBuilder pName = new StringBuilder(100);
            StringBuilder pSex = new StringBuilder(100);
            StringBuilder pNation = new StringBuilder(100);
            StringBuilder pBirth = new StringBuilder(100);
            StringBuilder pCertNo = new StringBuilder(100);
            StringBuilder pEffectData = new StringBuilder(100);
            StringBuilder pExpire = new StringBuilder(100);
            StringBuilder pCardNo = new StringBuilder(9);
            StringBuilder pErrMsg = new StringBuilder(100);
            int iType = 1;
            int RHandle = -1;

            RHandle = dev.ICC_Reader_Open(dev_name);
            if (RHandle <= 0)
            {
                textBox2.Text = "设备未连接";
                return;
            }
            int nRt = dev.PICC_Reader_SICARD(RHandle, pCardNo, pName, pSex, pNation, pBirth, pCertNo, pEffectData, pExpire, pErrMsg);
            if (nRt != 0)
            {
                textBox2.Text = "读卡失败" + pErrMsg.ToString();
                return;
            }
            this.textBox2.Text = "";
            this.textBox2.Text += pCardNo.ToString();
            this.textBox2.Text += "|" + pName.ToString();
            this.textBox2.Text += "|" + pSex.ToString();
            this.textBox2.Text += "|" + pNation.ToString();
            this.textBox2.Text += "|" + pBirth.ToString();
            this.textBox2.Text += "|" + pCertNo.ToString();
            this.textBox2.Text += "|" + pEffectData.ToString();
            this.textBox2.Text += "|" + pExpire.ToString();

            dev.ICC_Reader_Close(RHandle);
        }
    }
}
