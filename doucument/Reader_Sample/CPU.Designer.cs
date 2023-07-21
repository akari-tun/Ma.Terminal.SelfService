namespace Reader_Sample
{
    partial class CPU
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SlotBox = new System.Windows.Forms.ComboBox();
            this.apduBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.showBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(340, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "上电";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SlotBox
            // 
            this.SlotBox.FormattingEnabled = true;
            this.SlotBox.Items.AddRange(new object[] {
            "大卡座",
            "副卡座",
            "SAM1",
            "SAM2",
            "SAM3",
            "SAM4",
            "非接CPU"});
            this.SlotBox.Location = new System.Drawing.Point(183, 36);
            this.SlotBox.Name = "SlotBox";
            this.SlotBox.Size = new System.Drawing.Size(124, 20);
            this.SlotBox.TabIndex = 1;
            // 
            // apduBox
            // 
            this.apduBox.Location = new System.Drawing.Point(10, 80);
            this.apduBox.Name = "apduBox";
            this.apduBox.Size = new System.Drawing.Size(297, 21);
            this.apduBox.TabIndex = 2;
            this.apduBox.Text = "0084000008";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(340, 80);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "执行APDU";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // showBox
            // 
            this.showBox.Location = new System.Drawing.Point(10, 130);
            this.showBox.Multiline = true;
            this.showBox.Name = "showBox";
            this.showBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.showBox.Size = new System.Drawing.Size(405, 150);
            this.showBox.TabIndex = 4;
            this.showBox.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "显示:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "APDU:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(127, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "卡座:";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(12, 283);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 12);
            this.status.TabIndex = 8;
            // 
            // CPU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 300);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.apduBox);
            this.Controls.Add(this.SlotBox);
            this.Controls.Add(this.button1);
            this.Name = "CPU";
            this.Text = "CPU";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox SlotBox;
        private System.Windows.Forms.TextBox apduBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox showBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label status;
    }
}