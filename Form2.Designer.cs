
namespace CommTest
{
    partial class Form2
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
            this.btn_Open = new System.Windows.Forms.Button();
            this.labCOM = new System.Windows.Forms.Label();
            this.cmb_ComPort = new System.Windows.Forms.ComboBox();
            this.labdatabit = new System.Windows.Forms.Label();
            this.cmb_Databit = new System.Windows.Forms.ComboBox();
            this.labparity = new System.Windows.Forms.Label();
            this.cmb_Parity = new System.Windows.Forms.ComboBox();
            this.labstopbit = new System.Windows.Forms.Label();
            this.cmb_Stopbit = new System.Windows.Forms.ComboBox();
            this.labbaudrate = new System.Windows.Forms.Label();
            this.cmb_BaudRate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_TimeOut = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btn_Open
            // 
            this.btn_Open.BackColor = System.Drawing.Color.White;
            this.btn_Open.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Open.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Open.ForeColor = System.Drawing.Color.SteelBlue;
            this.btn_Open.Location = new System.Drawing.Point(493, 108);
            this.btn_Open.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(81, 57);
            this.btn_Open.TabIndex = 68;
            this.btn_Open.Text = "OK";
            this.btn_Open.UseVisualStyleBackColor = false;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // labCOM
            // 
            this.labCOM.AutoSize = true;
            this.labCOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCOM.ForeColor = System.Drawing.Color.White;
            this.labCOM.Location = new System.Drawing.Point(12, 34);
            this.labCOM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCOM.Name = "labCOM";
            this.labCOM.Size = new System.Drawing.Size(58, 24);
            this.labCOM.TabIndex = 67;
            this.labCOM.Text = "ポート";
            // 
            // cmb_ComPort
            // 
            this.cmb_ComPort.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmb_ComPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_ComPort.ForeColor = System.Drawing.Color.SteelBlue;
            this.cmb_ComPort.FormattingEnabled = true;
            this.cmb_ComPort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmb_ComPort.Location = new System.Drawing.Point(12, 63);
            this.cmb_ComPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmb_ComPort.MaxDropDownItems = 20;
            this.cmb_ComPort.Name = "cmb_ComPort";
            this.cmb_ComPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_ComPort.Size = new System.Drawing.Size(101, 28);
            this.cmb_ComPort.TabIndex = 66;
            this.cmb_ComPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextLimit);
            // 
            // labdatabit
            // 
            this.labdatabit.AutoSize = true;
            this.labdatabit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labdatabit.ForeColor = System.Drawing.Color.White;
            this.labdatabit.Location = new System.Drawing.Point(231, 34);
            this.labdatabit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labdatabit.Name = "labdatabit";
            this.labdatabit.Size = new System.Drawing.Size(98, 24);
            this.labdatabit.TabIndex = 61;
            this.labdatabit.Text = "データビット";
            // 
            // cmb_Databit
            // 
            this.cmb_Databit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Databit.ForeColor = System.Drawing.Color.SteelBlue;
            this.cmb_Databit.FormattingEnabled = true;
            this.cmb_Databit.Items.AddRange(new object[] {
            "8",
            "7"});
            this.cmb_Databit.Location = new System.Drawing.Point(231, 63);
            this.cmb_Databit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmb_Databit.Name = "cmb_Databit";
            this.cmb_Databit.Size = new System.Drawing.Size(98, 28);
            this.cmb_Databit.TabIndex = 60;
            this.cmb_Databit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextLimit);
            // 
            // labparity
            // 
            this.labparity.AutoSize = true;
            this.labparity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labparity.ForeColor = System.Drawing.Color.White;
            this.labparity.Location = new System.Drawing.Point(344, 34);
            this.labparity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labparity.Name = "labparity";
            this.labparity.Size = new System.Drawing.Size(107, 24);
            this.labparity.TabIndex = 59;
            this.labparity.Text = "パリティビット";
            // 
            // cmb_Parity
            // 
            this.cmb_Parity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Parity.ForeColor = System.Drawing.Color.SteelBlue;
            this.cmb_Parity.FormattingEnabled = true;
            this.cmb_Parity.Items.AddRange(new object[] {
            "NONE",
            "ODD",
            "EVEN"});
            this.cmb_Parity.Location = new System.Drawing.Point(344, 63);
            this.cmb_Parity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmb_Parity.Name = "cmb_Parity";
            this.cmb_Parity.Size = new System.Drawing.Size(107, 28);
            this.cmb_Parity.TabIndex = 58;
            this.cmb_Parity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextLimit);
            // 
            // labstopbit
            // 
            this.labstopbit.AutoSize = true;
            this.labstopbit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labstopbit.ForeColor = System.Drawing.Color.White;
            this.labstopbit.Location = new System.Drawing.Point(467, 34);
            this.labstopbit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labstopbit.Name = "labstopbit";
            this.labstopbit.Size = new System.Drawing.Size(107, 24);
            this.labstopbit.TabIndex = 57;
            this.labstopbit.Text = "ストップビット";
            // 
            // cmb_Stopbit
            // 
            this.cmb_Stopbit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Stopbit.ForeColor = System.Drawing.Color.SteelBlue;
            this.cmb_Stopbit.FormattingEnabled = true;
            this.cmb_Stopbit.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cmb_Stopbit.Location = new System.Drawing.Point(467, 63);
            this.cmb_Stopbit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmb_Stopbit.Name = "cmb_Stopbit";
            this.cmb_Stopbit.Size = new System.Drawing.Size(107, 28);
            this.cmb_Stopbit.TabIndex = 56;
            this.cmb_Stopbit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextLimit);
            // 
            // labbaudrate
            // 
            this.labbaudrate.AutoSize = true;
            this.labbaudrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labbaudrate.ForeColor = System.Drawing.Color.White;
            this.labbaudrate.Location = new System.Drawing.Point(127, 34);
            this.labbaudrate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labbaudrate.Name = "labbaudrate";
            this.labbaudrate.Size = new System.Drawing.Size(90, 24);
            this.labbaudrate.TabIndex = 55;
            this.labbaudrate.Text = "通信速度";
            // 
            // cmb_BaudRate
            // 
            this.cmb_BaudRate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmb_BaudRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_BaudRate.ForeColor = System.Drawing.Color.SteelBlue;
            this.cmb_BaudRate.FormattingEnabled = true;
            this.cmb_BaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600"});
            this.cmb_BaudRate.Location = new System.Drawing.Point(129, 63);
            this.cmb_BaudRate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmb_BaudRate.Name = "cmb_BaudRate";
            this.cmb_BaudRate.Size = new System.Drawing.Size(96, 28);
            this.cmb_BaudRate.TabIndex = 54;
            this.cmb_BaudRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextLimit);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(353, 108);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 24);
            this.label1.TabIndex = 70;
            this.label1.Text = "タイムアウト";
            // 
            // cmb_TimeOut
            // 
            this.cmb_TimeOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmb_TimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_TimeOut.ForeColor = System.Drawing.Color.SteelBlue;
            this.cmb_TimeOut.FormattingEnabled = true;
            this.cmb_TimeOut.Items.AddRange(new object[] {
            "50",
            "100",
            "200",
            "300",
            "400",
            "500",
            "600",
            "700",
            "800",
            "900",
            "1000"});
            this.cmb_TimeOut.Location = new System.Drawing.Point(355, 137);
            this.cmb_TimeOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmb_TimeOut.Name = "cmb_TimeOut";
            this.cmb_TimeOut.Size = new System.Drawing.Size(96, 28);
            this.cmb_TimeOut.TabIndex = 69;
            this.cmb_TimeOut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextLimit);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(586, 217);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_TimeOut);
            this.Controls.Add(this.btn_Open);
            this.Controls.Add(this.labCOM);
            this.Controls.Add(this.cmb_ComPort);
            this.Controls.Add(this.labdatabit);
            this.Controls.Add(this.cmb_Databit);
            this.Controls.Add(this.labparity);
            this.Controls.Add(this.cmb_Parity);
            this.Controls.Add(this.labstopbit);
            this.Controls.Add(this.cmb_Stopbit);
            this.Controls.Add(this.labbaudrate);
            this.Controls.Add(this.cmb_BaudRate);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COM Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labCOM;
        private System.Windows.Forms.Label labdatabit;
        private System.Windows.Forms.Label labparity;
        private System.Windows.Forms.Label labstopbit;
        private System.Windows.Forms.Label labbaudrate;
        internal System.Windows.Forms.ComboBox cmb_ComPort;
        internal System.Windows.Forms.ComboBox cmb_Databit;
        internal System.Windows.Forms.ComboBox cmb_Parity;
        internal System.Windows.Forms.ComboBox cmb_Stopbit;
        internal System.Windows.Forms.ComboBox cmb_BaudRate;
        public System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox cmb_TimeOut;
    }
}