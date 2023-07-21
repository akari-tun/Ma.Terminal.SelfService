using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reader_Sample
{
    public partial class sfz : Form
    {
        public sfz()
        {
            InitializeComponent();
        }

        int nRt = -99;
        private StringBuilder dev_name = new StringBuilder("USB1");


        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder pBmpFile = new StringBuilder(100);
            StringBuilder pFingerData = new StringBuilder(1025);
            StringBuilder pBmpData = new StringBuilder(77725);
            StringBuilder pBase64Data = new StringBuilder(6025);
            StringBuilder pName = new StringBuilder(100);
            StringBuilder pSex = new StringBuilder(100);
            StringBuilder pNation = new StringBuilder(100);
            StringBuilder pBirth = new StringBuilder(100);
            StringBuilder pAddress = new StringBuilder(100);
            StringBuilder pCertNo = new StringBuilder(100);
            StringBuilder pDepartment = new StringBuilder(100);
            StringBuilder pEffectData = new StringBuilder(100);
            StringBuilder pExpire = new StringBuilder(100);
            StringBuilder pData = new StringBuilder(100);
            StringBuilder pErrMsg = new StringBuilder(100);
            StringBuilder pTXZHM = new StringBuilder(100);
            StringBuilder pTXZQFCS = new StringBuilder(100);

            StringBuilder pEnName = new StringBuilder(200);
            StringBuilder pEnNation = new StringBuilder(100);
            StringBuilder pAuthorCode = new  StringBuilder(100);
            StringBuilder pCardVersion = new StringBuilder(100);

            string str = System.Environment.CurrentDirectory;
            pBmpFile.Append(str);
            pBmpFile.Append(@"\zp.bmp");


            int Rhandle;
            Rhandle = dev.ICC_Reader_Open(dev_name);
            if (Rhandle <= 0)
            {
                return;
            }
#if false
           //获取身份证ID
            byte[] uid = new byte[20];
            StringBuilder sUID = new StringBuilder(30);
            dev.PICC_Reader_ID_ReadUID(Rhandle, sUID);
        
#endif
            //该函数获取身份证信息的同时保存照片到指定路径
            nRt = dev.PICC_Reader_ReadIDCard(Rhandle, pErrMsg);
            if ( nRt == 0 )
            {
                //textBox1
                int icardType = -1;
                icardType = dev.GetCardType();
                
                if (icardType == 0)
                {
                    dev.GetName(pName);
                    dev.GetSex(pSex);
                    dev.GetNation(pNation);
                    dev.GetBirth(pBirth);
                    dev.GetCertNo(pCertNo);
                    dev.GetAddress(pAddress);
                    dev.GetDepartemt(pDepartment);
                    dev.GetEffectDate(pEffectData);
                    dev.GetExpireDate(pExpire);

                    this.textBox1.Text = "读卡成功，证件类型：居民身份证";
                    this.textBox1.Text += "姓名：" + pName.ToString();
                    this.textBox1.Text += "性别：" + pSex.ToString();
                    this.textBox1.Text += "民族：" + pNation.ToString();
                    this.textBox1.Text += "出生日期：" + pBirth.ToString();
                    this.textBox1.Text += "住址：" + pAddress.ToString();
                    this.textBox1.Text += "身份证号码：" + pCertNo.ToString();
                    this.textBox1.Text += "签发机关：" + pDepartment.ToString();
                    this.textBox1.Text += "有效起始日期：" + pEffectData.ToString();
                    this.textBox1.Text += "有效截止日期：" + pExpire.ToString();
                }
                if (icardType == 2)
                {
                    dev.GetName(pName);
                    dev.GetSex(pSex);
                    
                    dev.GetBirth(pBirth);
                    dev.GetCertNo(pCertNo);
                    dev.GetAddress(pAddress);
                    dev.GetDepartemt(pDepartment);
                    dev.GetEffectDate(pEffectData);
                    dev.GetExpireDate(pExpire);

                    dev.GetTXZHM(pTXZHM);
                    dev.GetTXZQFCS(pTXZQFCS);

                    this.textBox1.Text = "读卡成功，证件类型：港澳台居民居住证";
                    this.textBox1.Text += "姓名：" + pName.ToString();
                    this.textBox1.Text += "性别：" + pSex.ToString();
                    
                    this.textBox1.Text += "出生日期：" + pBirth.ToString();
                    this.textBox1.Text += "住址：" + pAddress.ToString();
                    this.textBox1.Text += "身份证号码：" + pCertNo.ToString();
                    this.textBox1.Text += "签发机关：" + pDepartment.ToString();
                    this.textBox1.Text += "有效起始日期：" + pEffectData.ToString();
                    this.textBox1.Text += "有效截止日期：" + pExpire.ToString();
                    this.textBox1.Text += "通行证号码：" + pTXZHM.ToString();
                    this.textBox1.Text += "通行证签发次数：" + pTXZQFCS.ToString();
                }
                if (icardType == 1)
                {
                    dev.GetEnName(pEnName);
                    dev.GetName(pName);
                    dev.GetSex(pSex);
                    dev.GetBirth(pBirth);
                    dev.GetCertNo(pCertNo);                    
                    dev.GetEffectDate(pEffectData);
                    dev.GetExpireDate(pExpire);
                    dev.GetNationalityCode(pEnNation);

                    this.textBox1.Text = "读卡成功，证件类型：外国人永久居留证";
                    this.textBox1.Text += "中文姓名：" + pName.ToString();
                    this.textBox1.Text += "英文姓名：" + pEnName.ToString();
                    this.textBox1.Text += "国籍代码：" + pEnNation.ToString();
                    this.textBox1.Text += "出生日期：" + pBirth.ToString();
                    this.textBox1.Text += "永久证号码：" + pCertNo.ToString();
                    this.textBox1.Text += "有效起始日期：" + pEffectData.ToString();
                    this.textBox1.Text += "有效截止日期：" + pExpire.ToString();

                }

                dev.GetBmpFile(pBmpFile);
                this.pictureBox1.Image = Image.FromFile(pBmpFile.ToString());
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox1.Show();


                return;
            }
            else
            {
                this.status.Text = "读卡失败,返回值：";
                this.status.Text += nRt.ToString();
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*   this.NameBox.Text = "";
               this.SexBox.Text = "";
               this.NationBox.Text = "";
               this.BornBox.Text = "";
               this.AddrBox.Text = "";
               this.CertNoBox.Text = "";
               this.DepartmentBox.Text = "";
               this.DataBox.Text = "";*/
            this.status.Text = "";
       //     this.UIDBox.Text = "";
            this.pictureBox1.Hide();
        }

        private void sfz_Load(object sender, EventArgs e)
        {

        }
    }
}
