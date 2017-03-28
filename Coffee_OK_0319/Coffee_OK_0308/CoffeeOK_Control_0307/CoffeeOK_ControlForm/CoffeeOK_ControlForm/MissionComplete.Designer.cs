namespace CoffeeOK_ControlForm
{
    partial class MissionComplete
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RobertComPort_comboBox = new System.Windows.Forms.ComboBox();
            this.RobertComPort_Label = new System.Windows.Forms.Label();
            this.ControlBoardComPort_Label = new System.Windows.Forms.Label();
            this.ControlBoardComPort_comboBox = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.DB_CheckTimer = new System.Windows.Forms.Timer(this.components);
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.StartDB_Check = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RobertComPort_comboBox
            // 
            this.RobertComPort_comboBox.FormattingEnabled = true;
            this.RobertComPort_comboBox.Location = new System.Drawing.Point(12, 77);
            this.RobertComPort_comboBox.Name = "RobertComPort_comboBox";
            this.RobertComPort_comboBox.Size = new System.Drawing.Size(121, 20);
            this.RobertComPort_comboBox.TabIndex = 0;
            // 
            // RobertComPort_Label
            // 
            this.RobertComPort_Label.AutoSize = true;
            this.RobertComPort_Label.Location = new System.Drawing.Point(13, 59);
            this.RobertComPort_Label.Name = "RobertComPort_Label";
            this.RobertComPort_Label.Size = new System.Drawing.Size(113, 12);
            this.RobertComPort_Label.TabIndex = 1;
            this.RobertComPort_Label.Text = "选择机器人通信串口";
            // 
            // ControlBoardComPort_Label
            // 
            this.ControlBoardComPort_Label.AutoSize = true;
            this.ControlBoardComPort_Label.Location = new System.Drawing.Point(12, 142);
            this.ControlBoardComPort_Label.Name = "ControlBoardComPort_Label";
            this.ControlBoardComPort_Label.Size = new System.Drawing.Size(101, 12);
            this.ControlBoardComPort_Label.TabIndex = 3;
            this.ControlBoardComPort_Label.Text = "选择控制电控串口";
            // 
            // ControlBoardComPort_comboBox
            // 
            this.ControlBoardComPort_comboBox.FormattingEnabled = true;
            this.ControlBoardComPort_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ControlBoardComPort_comboBox.Location = new System.Drawing.Point(11, 160);
            this.ControlBoardComPort_comboBox.Name = "ControlBoardComPort_comboBox";
            this.ControlBoardComPort_comboBox.Size = new System.Drawing.Size(121, 20);
            this.ControlBoardComPort_comboBox.TabIndex = 2;
            this.ControlBoardComPort_comboBox.SelectedIndexChanged += new System.EventHandler(this.ControlBoardComPort_comboBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(314, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(201, 335);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 1000;
            this.MainTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(173, 131);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Setp1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(173, 160);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Setp2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(173, 189);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "Setp3";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(173, 218);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Setp4";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(173, 339);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "Read";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // DB_CheckTimer
            // 
            this.DB_CheckTimer.Interval = 1000;
            this.DB_CheckTimer.Tick += new System.EventHandler(this.DB_CheckTimerTick);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(15, 284);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(111, 23);
            this.button7.TabIndex = 11;
            this.button7.Text = "分杯测试";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(173, 74);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(111, 23);
            this.button8.TabIndex = 12;
            this.button8.Text = "Start port";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(521, 324);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(111, 23);
            this.button9.TabIndex = 13;
            this.button9.Text = "SetDB";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // StartDB_Check
            // 
            this.StartDB_Check.Location = new System.Drawing.Point(521, 191);
            this.StartDB_Check.Name = "StartDB_Check";
            this.StartDB_Check.Size = new System.Drawing.Size(111, 23);
            this.StartDB_Check.TabIndex = 14;
            this.StartDB_Check.Text = "开始运行";
            this.StartDB_Check.UseVisualStyleBackColor = true;
            this.StartDB_Check.Click += new System.EventHandler(this.StartDB_Check_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(173, 247);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(111, 23);
            this.button11.TabIndex = 15;
            this.button11.Text = "Setp5";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Visible = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(173, 276);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(111, 23);
            this.button12.TabIndex = 16;
            this.button12.Text = "Setp6";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Visible = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(521, 35);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(111, 23);
            this.button13.TabIndex = 17;
            this.button13.Text = "FarCodeTest";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(521, 77);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(111, 23);
            this.button14.TabIndex = 18;
            this.button14.Text = "PowerOff_Test";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(15, 339);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(111, 23);
            this.button15.TabIndex = 19;
            this.button15.Text = "调料添加测试";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(521, 255);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(111, 23);
            this.button16.TabIndex = 20;
            this.button16.Text = "插入订单";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // MissionComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 374);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.StartDB_Check);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ControlBoardComPort_Label);
            this.Controls.Add(this.ControlBoardComPort_comboBox);
            this.Controls.Add(this.RobertComPort_Label);
            this.Controls.Add(this.RobertComPort_comboBox);
            this.Name = "MissionComplete";
            this.Text = "CoffeeOK";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Enter += new System.EventHandler(this.Form1_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox RobertComPort_comboBox;
        private System.Windows.Forms.Label RobertComPort_Label;
        private System.Windows.Forms.Label ControlBoardComPort_Label;
        private System.Windows.Forms.ComboBox ControlBoardComPort_comboBox;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Timer DB_CheckTimer;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button StartDB_Check;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
    }
}

