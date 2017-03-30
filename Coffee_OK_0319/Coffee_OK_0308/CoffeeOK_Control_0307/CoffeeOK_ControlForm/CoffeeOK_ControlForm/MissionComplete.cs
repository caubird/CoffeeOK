using CoffeeOK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;

namespace CoffeeOK_ControlForm
{
    public partial class MissionComplete : Form
    {
        List<String> ComportNames = new List<string>();

        CoffeeOK_RobertConnection RobertConnectItem = new CoffeeOK_RobertConnection();

        ConnectControlBoard ControlBoardConnectItem = new ConnectControlBoard();
        int step = 0;
        int oldstep = 0;
        bool DropCup_Flag = false;
        Task TaskNow = new Task("", "", DateTime.Now.ToString(), "", "", "", "");

        int Test_Counter = 5;
        int Test_Counter1 = 0;

        bool init_flag = false;

        bool RobertInited_Flag = false;

        private Coffee_Process CoffeeProcessItem = new Coffee_Process();

        List<Coffee_Procedure> MissionList = new List<Coffee_Procedure>();
        int MissionCounter = 0;

        public MissionComplete()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TestSerialPort();
        }

        public void InitComPort(string portname)
        {
            //ComPort.PortName = ComPortName;
            // ComPort.BaudRate = 4800;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = System.IO.Ports.StopBits.One;
            serialPort1.Parity = System.IO.Ports.Parity.None;
            serialPort1.Encoding = Encoding.UTF8;
            serialPort1.BaudRate = 115200;
            serialPort1.PortName = portname;
            //ComPort.Open();
        }

        private void TestSerialPort()
        {
            InitComPort("COM1");
            for (int i = 0; i < 32; i++)///COM端口扫描
            {
                string ComName = "COM" + i.ToString();
                string str = serialPort1.PortName;
                serialPort1.PortName = ComName;
                try
                {
                    serialPort1.Open();
                    ComportNames.Add(ComName);
                    serialPort1.Close();
                }
                catch
                {

                }
            }
            
            RobertComPort_comboBox.DataSource = ComportNames;//.Add(ComportNames[i]);

            /*
            for (int i = 0; i < RobertComPort_comboBox.Items.Count; i++)
            {
                if (Properties.Settings.Default.RobertComPort == RobertComPort_comboBox.Items[i].ToString())
                {
                    RobertComPort_comboBox.SelectedIndex = i;
                    byte[] ControlBoardTestFrame = { 0x55, 0xAA, 0x02, 0x05, 0x00, 0x01, 0x00, 0x00 };
                    byte[] sendfram = new byte[ControlBoardTestFrame.Length + 1];
                    for (int j = 0; j < ControlBoardTestFrame.Length; j++)
                    {
                        sendfram[j] = ControlBoardTestFrame[j];
                    }
                    sendfram[sendfram.Length - 1] = ConnectControlBoard.CRC_Compute(sendfram, sendfram.Length - 1);
                    
                    serialPort1.Write(sendfram, 0, sendfram.Length);
                }
            }
            */
            List<string> strlist = new List<string>();
            for(int i=0;i< ComportNames.Count;i++)
            { strlist.Insert(i, ComportNames[i]); }
            ControlBoardComPort_comboBox.DataSource = strlist;

            for (int i = 0; i < ControlBoardComPort_comboBox.Items.Count; i++)
            {
                if (Properties.Settings.Default.ControlBoardComPort == ControlBoardComPort_comboBox.Items[i].ToString())
                {
                    ControlBoardComPort_comboBox.SelectedIndex = i;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //serialPort1.PortName =
            if (RobertComPort_comboBox.SelectedItem != null)
            {
                RobertConnectItem.SerialPortName = RobertComPort_comboBox.SelectedItem.ToString();

                if (RobertConnectItem.SerialPortName != null && RobertConnectItem.SerialPortName.Contains("COM"))
                {
                    RobertConnectItem.SerialPortInit();
                    //textBox1.Text = RobertConnectItem.GetWriteAddress_M(0x1067);
                    RobertConnectItem.TaskList.Add(7);
                    RobertConnectItem.TaskList.Add(1);
                    RobertConnectItem.TaskList.Add(2);
                    RobertConnectItem.TaskList.Add(3);
                    RobertConnectItem.TaskList.Add(4);
                    RobertConnectItem.TaskList.Add(5);
                    RobertConnectItem.TaskList.Add(6);
                    RobertConnectItem.TaskList.Add(7);
                    // RobertConnectItem.StartTaskServer();
                }
            }
            else
            {
                MessageBox.Show("请选择串口号");
            }
            #region 此处定义任务类型，1为泡咖啡任务，2为取咖啡任务，3为丢弃咖啡任务

            #endregion
            if (TaskNow.CodeID == "1")
            {
                step = 1;
                StopStep = 5;
            }
            else if (TaskNow.CodeID == "2")
            {
                if (step != 0)
                {
                    oldstep = step;
                }
                step = 1;
                StopStep = 5;
            }
            if (TaskNow.CodeID == "3")
            {
                step = 7;
                StopStep = 7;
            }
            MainTimer.Interval = 1000;
            DB_CheckTimer.Interval = 1000;
            //if(timer1.is )
            MainTimer.Start();
            button1.BackColor = Color.Green;
            //timer2.Start();
            //MessageBox.Show("开始");

        }

        private void ControlBoardComPort_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RobertComPort = ControlBoardComPort_comboBox.SelectedItem.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            this.textBox1.SelectionLength = 0;
            this.textBox1.ScrollToCaret();
        }

        /// <summary>
        /// Timer1主要负责机器人任务的执行及执行顺序控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //每秒读取一次
            ReadRobotCondition();
            if (RobertInited_Flag == false)
            {
                RobertInited_Flag = true;
                InitRobertWorkCondition();
            }
            if (MissionList!=null && MissionList.Count  > MissionCounter)
            {
                switch (MissionList[MissionCounter].ProcedureName)
                {
                    case "PickACup":
                        DoMitionOne();
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "DropACup":
                        DoMitionNine();
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PutCupToCoffeeMachine":
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            DoMitionTwo();// 
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PressLatte":
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            DoMitionSeven();
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PressCappuccino":
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            DoMitionSix();//DoMitionFive();
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PressAmaricanBlack":
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工步
                            DoMitionEight();
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PressBlack":
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            DoMitionThree();//DoMitionSeven();
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PutCupToAddSugarPossition":
                        DoMitionFour();
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PutCupToPickUpPossition":
                        //待添加
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "AddSugar":
                        //加糖，需要分析库存情况决定使用哪个加糖位置
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            Add_Suger();
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "AddHalfSugar":
                        //加半糖，需要添加相应程序
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "Addchocolate":
                        //加巧克力
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            Add_Cholclate();
                        }
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "PutCoffeeWithSugarToPickUpPossiton":
                        if (RobertConnectItem.WorkingConditionFlag == true)
                        {//机器人完成前面工步，才能执行此工布
                            DoMitionFive();
                        }
                         //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                            break;
                    case "PushOutCoffee":
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    case "DiscardCoffee":
                        //丢掉咖啡，需要添加相应程序
                        //Sleep(CoffeeProcessItem.ProcedureTable[MissionList[MissionCounter]].WaitTimeAfterWork);
                        break;
                    default:
                        break;
                }
                Sleep(MissionList[MissionCounter].WaitTimeAfterWork);

            }
        }

        private void MissionSleep(object p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 任务执行
        /// </summary>
        private void MissionAction()
        {

        }

        /// <summary>
        /// 定时器2主要负责数据库查询及任务指令接收
        /// 同时从数据库生成任务表
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DB_CheckTimerTick(object sender, EventArgs e)
        {
            //检查数命令状态
            ConnectMysql_DB();
            //查看任务列表
            if (TaskTable.Count > 0)
            {
                if (TaskTable[0].Status == "0")//任务开始部分
                {
                    textBox1.Text += "任务开始，制作咖啡" + (char)0x0d + (char)0x0a;

                    TaskTable[0].Status = "1";
                    SetMysql_DB_TaskStatus(TaskTable[0]);
                    TaskNow = TaskTable[0];
                    Coffee_Process_FromDB cpf = new Coffee_Process_FromDB(int.Parse(TaskNow.processid));
                    //MissionList = CoffeeProcessItem.GetProcedure();
                    MissionList = cpf.Get_Coffee_Process_FromDB(int.Parse(TaskNow.processid));

                    Start_TaskAction();
                }
                else if (TaskTable[0].Status == "1")//正在执行
                {
                    if (TaskNow.Status == "2")
                    {
                        textBox1.Text += "咖啡制作完成" + (char)0x0d + (char)0x0a;

                        TaskTable[0].Status = "2";
                        SetMysql_DB_TaskStatus(TaskTable[0]);
                        Stop_TaskNow();
                    }
                }
                else if (TaskTable[0].Status == "2")//执行完成
                {
                    //从本地删除

                }
            }
            else
            {
                //无任务正在执行
            }
        }

        #region 任务跳转逻辑
        private void StepJumpControl()
        {
            switch (step)
            {
                case 0:
                    step = 0;
                    break;

                case 1://伸手接杯
                    step = 2;
                    //if (TaskNow.processid == "1")
                    //{
                    //    step = 0;
                    //}
                    break;
                case 2://分配器落杯，同时等待机械手缩回
                    step = 3;
                    break;
                case 3://
                    step = 4;
                    break;
                case 4:
                    step = 5;
                    break;
                case 5://咖啡制作结束
                    step = 0;
                    break;
                case 6://取咖啡步骤
                    step = 0;
                    break;
                case 7:
                    step = 0;
                    break;
                default:
                    step = 0;
                    break;
            }
            oldstep = 0;
        }

        private void MissionConvertControl()
        {
            if (MissionCounter < MissionList.Count )
            {
                step = MissionList[MissionCounter].ProcedureNumber;
                MissionCounter++;
            }
        }

        private void MissionSleep(int stepcounter)
        {
            switch (stepcounter)
            {
                case 0:
                    break;

                case 1:
                    break;

            }
            //Sleep(time);
        }

        private void Sleep(int time)
        {
            if (time < 1000)
            {
                Thread.Sleep(time);
            }
            else
            {
                while (time > 500)
                {
                    int time1 = time - 50;
                    time -= 1000;
                    Thread.Sleep(50);
                }
                Thread.Sleep(time);
            }
        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            DoMitionOne();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DoMitionTwo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DoMitionThree();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DoMitionFour();
        }

        #region 机器人工序分配及机器人状态读取、机器人初始化
        /// <summary>
        /// 机器人伸手到取杯位置，等待杯子掉落
        /// </summary>
        private void DoMitionOne()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission1);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission2[0] && RobertConnectItem.Order_DoMission2[1] == Responsebytes[1])
            {
                //MessageBox.Show("写入命令成功!");
            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 机器人将杯子取回，然后送到咖啡机位置
        /// </summary>
        private void DoMitionTwo()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission1_Continue);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission2[0] && RobertConnectItem.Order_DoMission2[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 点咖啡机，制作意式咖啡
        /// </summary>
        private void DoMitionThree()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission3);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission3[0] && RobertConnectItem.Order_DoMission3[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 放杯子到待添加调料位置
        /// </summary>
        private void DoMitionFour()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission4);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission4[0] && RobertConnectItem.Order_DoMission4[1] == Responsebytes[1])
            {
                //MessageBox.Show("写入命令成功!");
            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 放杯子到出货位置
        /// </summary>
        private void DoMitionFive()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission5);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission2[0] && RobertConnectItem.Order_DoMission2[1] == Responsebytes[1])
            {
                //MessageBox.Show("写入命令成功!");
            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 点击制作卡布奇诺
        /// </summary>
        private void DoMitionSix()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission6);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission2[0] && RobertConnectItem.Order_DoMission2[1] == Responsebytes[1])
            {
                //MessageBox.Show("写入命令成功!");
            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 点击制作拿铁
        /// </summary>
        private void DoMitionSeven()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission7);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission3[0] && RobertConnectItem.Order_DoMission3[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 点击制作美式咖啡
        /// </summary>
        private void DoMitionEight()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission2);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission4[0] && RobertConnectItem.Order_DoMission4[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 放一个杯子下来
        /// </summary>
        private void DoMitionNine()
        {
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                try
                {
                    ControlBoardConnectItem.ComPort.PortName = ControlBoardComPort_comboBox.SelectedItem.ToString();
                    ControlBoardConnectItem.ComPort.Open();
                }
                catch
                {
                    MessageBox.Show("控制板串口打开失败！");
                }
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_ReleaseACup);
            Thread.Sleep(100);
            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "落杯" + (char)0x0d + (char)0x0a;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0d + (char)0x0a;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 把咖啡从咖啡机直接移到出货位置
        /// </summary>
        private void DoMitionTen()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission8);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission4[0] && RobertConnectItem.Order_DoMission4[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        /// <summary>
        /// 添加位置1调料，一份
        /// </summary>
        private void DoMition_AddPosition1_Full()
        {
            //RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission4);
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning1);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料1一份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置1调料，半份
        /// </summary>
        private void DoMition_AddPositon1_Half()
        {
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning1_Half);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料1半份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置2调料，一份
        /// </summary>
        private void DoMition_AddPosition2_Full()
        {
            //RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission4);
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning2);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料2一份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置2调料，半份
        /// </summary>
        private void DoMition_AddPositon2_Half()
        {
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning2_Half);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料2半份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置3调料，一份
        /// </summary>
        private void DoMition_AddPosition3_Full()
        {
            //RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission4);
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning3);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料3一份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置3调料，半份
        /// </summary>
        private void DoMition_AddPositon3_Half()
        {
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning3_Half);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料3半份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置4调料，一份
        /// </summary>
        private void DoMition_AddPosition4_Full()
        {
            //RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission4);
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning4);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料4一份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }

        /// <summary>
        /// 添加位置4调料，半份
        /// </summary>
        private void DoMition_AddPositon4_Half()
        {
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning4_Half);

            Thread.Sleep(100);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

            if (
                bytes != null && bytes.Length > 7 &&
                bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
                textBox1.Text += "加调料4半份" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {
                //序列命令接收失败，不成功，重发
                textBox1.Text += "命令重发" + (char)0x0a + (char)0x0d;
                RleasACup();
                return;
            }
        }



        /// <summary>
        /// 初始化机器人工作状态，每次程序开启执行一次
        /// </summary>
        private void InitRobertWorkCondition()
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_InitRobertWorkingCondition);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission2[0] && RobertConnectItem.Order_DoMission2[1] == Responsebytes[1])
            {
                //MessageBox.Show("写入命令成功!");
            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {

            }
        }

        /// <summary>
        /// 读取机器人当前工作状态，如果M550为低，则为工作中，为高，则为空闲中
        /// </summary>
        private void ReadRobotCondition()
        {
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.GetReadBytes();
            //RobertConnectItem.ReadBytesBuf;
            //RobertConnectItem.WorkingConditionFlag = bytes[3];
            if (bytes.Length >5 && bytes[3] > 0 && bytes[0] == RobertConnectItem.ReadMissionMode[0] && bytes[1] == RobertConnectItem.ReadMissionMode[1])
            {
                RobertConnectItem.WorkingConditionFlag = true;
                button6.BackColor = Color.Green;
            }
            else if (bytes.Length > 5 && bytes[3] == 0 && bytes[0] == RobertConnectItem.ReadMissionMode[0] && bytes[1] == RobertConnectItem.ReadMissionMode[1])
            {
                RobertConnectItem.WorkingConditionFlag = false;
                button6.BackColor = Color.DarkRed;
            }
            else//没有正确的回复
            {
                //MessageBox.Show(bytes.ToString());
            }
            RobertConnectItem.ReadBytesBuf = new byte[1000];
        }

        #endregion 工步的实现

        private void button6_Click(object sender, EventArgs e)
        {
            ReadRobotCondition();
        }

        int counter = 0;

        private void button7_Click(object sender, EventArgs e)
        {
            RleasACup();
        }
        private void RleasACup()
        {
            try
            {
                if (ControlBoardComPort_comboBox.SelectedItem != null)
                {
                    //serialPort2.Encoding = Encoding.UTF8;
                    //serialPort2.BaudRate = 115200;

                    //serialPort2.PortName = ControlBoardComPort_comboBox.SelectedItem.ToString();
                    //serialPort2.Open();
                    //byte bytes1 = ConnectControlBoard.CRC_Compute(bytes1, bytes.Length);
                    byte[] Order_ReleaseACup = ControlBoardConnectItem.Order_ControlBoard_ReleaseACup;// { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x01, 0xF9 };
                    //byte bytes1 = ConnectControlBoard.CRC_Compute(bytes1, bytes.Length);
                    //byte bytes1 = new byte[bytes.Length + 1];
                    if (!ControlBoardConnectItem.ComPort.IsOpen)
                    {
                        ControlBoardConnectItem.InitComPort(ControlBoardComPort_comboBox.SelectedItem.ToString());
                    }

                    ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_ReleaseACup);
                    //serialPort2.Write(Order_ReleaseACup, 0, Order_ReleaseACup.Length);
                    Thread.Sleep(500);

                    byte[] bytes = ControlBoardConnectItem.ReadBackBytes();

                    if (
                        bytes != null && bytes.Length > 7 &&
                        bytes[3] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[3] &&
                        bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                        bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                        bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                        bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
                       )
                    {
                        //序列命令收到，认为成功
                        textBox1.Text += "落杯" + (char)0x0d + (char)0x0a;
                    }
                    else//发送不成功，重发
                    {
                        //序列命令接收失败，不成功，重发
                        textBox1.Text += "命令重发" + (char)0x0d + (char)0x0a;
                        RleasACup();
                        return;
                    }

                    //serialPort2.Close();
                    Thread.Sleep(100);
                }
            }
            catch { }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            RobertConnectItem.SerialPortName = RobertComPort_comboBox.SelectedItem.ToString();

            if (RobertConnectItem.SerialPortName != null && RobertConnectItem.SerialPortName.Contains("COM"))
            {
                RobertConnectItem.SerialPortInit();
            }

            //step = 1;

            MainTimer.Start();
        }

        #region 数据库连接部分
        List<int> WorkQueue = new List<int>();

        List<Task> TaskTable = new List<Task>();

        int WorkConter = 0;
        // string M_str_sqlcon = "server=192.168.1.100;user id=root;password=gengku;database=coffee"; //根据自己的设置
        string M_str_sqlcon = "server=localhost;user id=root;password=1234;database=coffee"; //根据自己的设置

        private void ConnectMysql_DB()
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(M_str_sqlcon);
            conn.Open();
            //MySqlCommand commn = new MySqlCommand("set names gb2312", conn);
            MySqlCommand commn = new MySqlCommand("set names UTF8", conn);
            commn.ExecuteNonQuery();
            string sql = "select * from coffee_execute_test ";
            MySqlDataAdapter mda = new MySqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            // mda.Fill(ds, "coffee_excute");
            mda.Fill(ds, "coffee_execute_test");
            //this.dataGridView1.DataSource = ds.Tables["coffee_execute"];
            //任务表列顺序：ID,code_id,create_time,status,list_id,priority
            //对应意义：    ID,代码，  创建时间，  执行状态,高级ID，优先级（数字越小越高）
            
            //创建新的TaskTable
            TaskTable = new List<Task>();

            //其中执行状态意义如下：0为未执行，1为正在执行，2为已完成
            //if (TaskTable.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql1 = "select * from claim_goods_list where id = '" + ds.Tables[0].Rows[0][4].ToString() + "'";
                    DataSet ds1 = new DataSet();
                    MySqlDataAdapter mda1 = new MySqlDataAdapter(sql1, conn);
                    mda1.Fill(ds1, "claim_goods_list");

                    string ProcessID = ds1.Tables[0].Rows[0][6].ToString();
                    TaskTable.Insert(i,
                        new Task(
                        ds.Tables[0].Rows[i][0].ToString(),
                        ds.Tables[0].Rows[i][1].ToString(),
                        ds.Tables[0].Rows[i][2].ToString(),
                        ds.Tables[0].Rows[i][3].ToString(),
                        ds.Tables[0].Rows[i][4].ToString(),
                        ds.Tables[0].Rows[i][5].ToString(),
                        ProcessID
                        )
                        );

                }
            }
            conn.Close();
        }

        private void SetMysql_DB_TaskStatus(Task TaskItem)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(M_str_sqlcon);
            conn.Open();
            //MySqlCommand commn = new MySqlCommand("set names gb2312", conn);
            MySqlCommand commn = new MySqlCommand("set names UTF8", conn);
            commn.ExecuteNonQuery();
            string sql = "update coffee_execute_test set status = '" + TaskItem.Status + "' where id= " + TaskItem.ID + "";
            //MySqlDataAdapter mda = new MySqlDataAdapter(sql, conn);
            //DataSet ds = new DataSet();
            // mda.Fill(ds, "coffee_excute");
            //mda.Fill(ds, "coffee_execute");

            MySqlCommand command = CreatMysqlCommand(sql, conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("数据修改失败");
            }

            //this.dataGridView1.DataSource = ds.Tables["coffee_execute"];
            //任务表列顺序：ID,code_id,create_time,status,list_id,priority
            //对应意义：    ID,代码，  创建时间，  执行状态,高级ID，优先级（数字越小越高）
            //mda.Update(ds);

            //创建新的TaskTable
            //其中执行状态意义如下：0为未执行，1为正在执行，2为已完成


            conn.Close();
        }

        /// <summary>
        /// 插入一条任务数据，用作测试整套机构
        /// </summary>
        /// <param name="TaskItem"></param>
        private void InsertMysql_DB_TaskStatus(Task TaskItem)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(M_str_sqlcon);
            conn.Open();
            //MySqlCommand commn = new MySqlCommand("set names gb2312", conn);
            MySqlCommand commn = new MySqlCommand("set names UTF8", conn);
            commn.ExecuteNonQuery();
            string sql_insert =
                "INSERT into coffee_execute_test VALUES(" +
                TaskItem.ID + ", '" +
                TaskItem.CodeID + "', '" +
                DateTime.Now.ToString() + "', '" +
                TaskItem.Status + "', '" +
                TaskItem.listid + "', '" +
                TaskItem.priority + "')";

            string sql = "update coffee_execute_test set status = '" + TaskItem.Status + "' where id= " + TaskItem.ID + "";
            //MySqlDataAdapter mda = new MySqlDataAdapter(sql, conn);
            //DataSet ds = new DataSet();
            // mda.Fill(ds, "coffee_excute");
            //mda.Fill(ds, "coffee_execute");

            MySqlCommand command = CreatMysqlCommand(sql_insert, conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("数据修改失败");
            }
            //this.dataGridView1.DataSource = ds.Tables["coffee_execute"];
            //任务表列顺序：ID,code_id,create_time,status,list_id,priority
            //对应意义：    ID,代码，  创建时间，  执行状态,高级ID，优先级（数字越小越高）
            //mda.Update(ds);

            //创建新的TaskTable
            //其中执行状态意义如下：0为未执行，1为正在执行，2为已完成


            conn.Close();
        }

        public MySqlCommand CreatMysqlCommand(string sql, MySqlConnection connection)
        {
            MySqlCommand mysqlcomand = new MySqlCommand(sql, connection);
            return mysqlcomand;
        }



        private void Start_TaskAction()
        {
            //if(TaskNow.Status=="0" && step ==0 && TaskNow.listid=="6")
            {
                button1_Click(null, null);
            }
        }
        private void Stop_TaskNow()
        {
            button1.BackColor = Color.DarkRed;
            //if (TaskNow.Status == "1" && step == 0 && TaskNow.listid == "6")
            {
                // button1_Click(null, null);
                TaskNow.Status = "2";
                SetMysql_DB_TaskStatus(TaskNow);

            }
        }

        #endregion 数据库连接部分
        /// <summary>
        /// 测试数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {//
            // ConnectMysql_DB();
            //Coffee_Process_FromDB cpdb = new Coffee_Process_FromDB(20);
            //int id=cpdb.Get_ProcessID();
            
            //InsertMysql_DB_TaskStatus(new Task("1", "1", DateTime.Now.ToString(), "0", "6", "1", "5"));
        }

        private void StartDB_Check_Click(object sender, EventArgs e)
        {
            if (DB_CheckTimer.Enabled)
            {
                DB_CheckTimer.Stop();
            }
            else
            {
                DB_CheckTimer.Start();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //取咖啡动作执行
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission5);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission5[0] && RobertConnectItem.Order_DoMission5[1] == Responsebytes[1])
            {
                //MessageBox.Show("写入命令成功!");
            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
            Thread.Sleep(1000);
            Thread.Sleep(1000);
            Thread.Sleep(1000);
            Thread.Sleep(1000);

        }

        private void button12_Click(object sender, EventArgs e)
        {
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_DoMission6);
            byte[] Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            byte[] bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void Form1_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 测试定时器，用于定时插入测试任务到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {

            if (Test_Counter == 1)
            {
                Test_Counter = 210;//300秒一次循环，5分钟执行一次
                //InsertMysql_DB_TaskStatus(new Task("1", "1", DateTime.Now.ToString(), "0", "6", "1", "5"));
            }
            else
                if (Test_Counter == 100)
            {
                //300秒一次循环，5分钟执行一次
                // InsertMysql_DB_TaskStatus(new Task("2", "2", DateTime.Now.ToString(), "0", "6", "1", "5"));
            }
            Test_Counter--;
        }

        public int farcodetestcounter = 0;

        public int StopStep { get; private set; }

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] Responsebytes = new byte[0];
            byte[] bytes = new byte[0];
            switch (farcodetestcounter)
            {
                case 0://上电
                    RobertConnectItem.WriteRobot(RobertConnectItem.Order_RobotPowerOn);
                    Responsebytes = RobertConnectItem.ResponseBytesBuf;
                    if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
                    {

                        //MessageBox.Show("写入命令成功!");

                    }
                    RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
                    bytes = RobertConnectItem.ReadBytesBuf;
                    if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
                    {
                        //MessageBox.Show("读取命令成功!");
                    }
                    farcodetestcounter = 1;
                    break;
                case 1://运行使能
                    RobertConnectItem.WriteRobot(RobertConnectItem.Order_FarCodeEnable1);
                    Responsebytes = RobertConnectItem.ResponseBytesBuf;
                    if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
                    {

                        //MessageBox.Show("写入命令成功!");

                    }
                    RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
                    bytes = RobertConnectItem.ReadBytesBuf;
                    if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
                    {
                        //MessageBox.Show("读取命令成功!");
                    }
                    Thread.Sleep(500);

                    RobertConnectItem.WriteRobot(RobertConnectItem.Order_FarCodeEnable2);
                    Responsebytes = RobertConnectItem.ResponseBytesBuf;
                    if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
                    {

                        //MessageBox.Show("写入命令成功!");

                    }
                    RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
                    bytes = RobertConnectItem.ReadBytesBuf;
                    if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
                    {
                        //MessageBox.Show("读取命令成功!");
                    }
                    farcodetestcounter = 2;
                    break;
                case 2://开始运行
                    RobertConnectItem.WriteRobot(RobertConnectItem.Order_FarCodeRunUp);
                    Responsebytes = RobertConnectItem.ResponseBytesBuf;
                    if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
                    {

                        //MessageBox.Show("写入命令成功!");

                    }
                    RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
                    bytes = RobertConnectItem.ReadBytesBuf;
                    if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
                    {
                        //MessageBox.Show("读取命令成功!");
                    }
                    Thread.Sleep(500);
                    Thread.Sleep(500);
                    Thread.Sleep(500);

                    RobertConnectItem.WriteRobot(RobertConnectItem.Order_FarCodeRunDown);
                    Responsebytes = RobertConnectItem.ResponseBytesBuf;
                    if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
                    {

                        //MessageBox.Show("写入命令成功!");

                    }
                    RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
                    bytes = RobertConnectItem.ReadBytesBuf;
                    if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
                    {
                        //MessageBox.Show("读取命令成功!");
                    }
                    farcodetestcounter = 3;
                    break;
                case 3://重置
                    RobertConnectItem.WriteRobot(RobertConnectItem.Order_RobotReset);
                    Responsebytes = RobertConnectItem.ResponseBytesBuf;
                    if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
                    {

                        //MessageBox.Show("写入命令成功!");

                    }
                    RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
                    bytes = RobertConnectItem.ReadBytesBuf;
                    if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
                    {
                        //MessageBox.Show("读取命令成功!");
                    }
                    farcodetestcounter = 0;
                    break;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            byte[] Responsebytes = new byte[0];
            byte[] bytes = new byte[0];
            RobertConnectItem.WriteRobot(RobertConnectItem.Order_RobotReset1);
            Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
            farcodetestcounter = 0;

            Thread.Sleep(500);
            Thread.Sleep(500);
            Thread.Sleep(500);

            RobertConnectItem.WriteRobot(RobertConnectItem.Order_RobotReset2);
            Responsebytes = RobertConnectItem.ResponseBytesBuf;
            if (Responsebytes[0] == RobertConnectItem.Order_DoMission6[0] && RobertConnectItem.Order_DoMission6[1] == Responsebytes[1])
            {

                //MessageBox.Show("写入命令成功!");

            }
            RobertConnectItem.ReadRobot(RobertConnectItem.ReadMissionMode);
            bytes = RobertConnectItem.ReadBytesBuf;
            if (bytes[0] == RobertConnectItem.ReadMissionMode[0] && RobertConnectItem.ReadMissionMode[1] == bytes[1])
            {
                //MessageBox.Show("读取命令成功!");
            }
            farcodetestcounter = 0;

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //string comname = ControlBoardComPort_comboBox.SelectedItem.ToString();
            //if (!ControlBoardConnectItem.ComPort.IsOpen)
            //{
            //    ControlBoardConnectItem.ComPort.PortName = comname;
            //}
            //else
            //{
            //    ControlBoardConnectItem.ComPort.Close();
            //    ControlBoardConnectItem.ComPort.PortName = ControlBoardComPort_comboBox.SelectedItem.ToString();

            //}
            //ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning3);

            Add_Suger();

        }

        /// <summary>
        /// 加调料1
        /// </summary>
        private void Add_Suger()
        {
            string comname = ControlBoardComPort_comboBox.SelectedItem.ToString();
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.ComPort.PortName = comname;
            }
            else
            {
                ControlBoardConnectItem.ComPort.Close();
                ControlBoardConnectItem.ComPort.PortName = ControlBoardComPort_comboBox.SelectedItem.ToString();

            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning1);

            Thread.Sleep(500);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();
            if (bytes != null && bytes.Length > 7 &&
                 bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
               bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
               bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
                 )
            {
                //序列命令收到，认为成功
                textBox1.Text += "添加调料2成功" + (char)0x0a + (char)0x0d;
            }
            else//发送不成功，重发
            {

                Add_Suger();
                return;
            }

        }

        /// <summary>
        /// 加调料2，暂定巧克力
        /// </summary>
        private void Add_Cholclate()
        {
            string comname = ControlBoardComPort_comboBox.SelectedItem.ToString();
            if (!ControlBoardConnectItem.ComPort.IsOpen)
            {
                ControlBoardConnectItem.ComPort.PortName = comname;
            }
            else
            {
                ControlBoardConnectItem.ComPort.Close();
                ControlBoardConnectItem.ComPort.PortName = ControlBoardComPort_comboBox.SelectedItem.ToString();

            }
            ControlBoardConnectItem.WriteControlSystem(ControlBoardConnectItem.Order_ControlBoard_AddSeasoning2);

            Thread.Sleep(500);

            byte[] bytes = ControlBoardConnectItem.ReadBackBytes();
            if (
                bytes != null && bytes.Length > 7 &&
                bytes[4] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[4] &&
                bytes[5] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[5] &&
                bytes[6] == ControlBoardConnectItem.Order_ControlBoard_ReplyOK[6] &&
                bytes[7] == ConnectControlBoard.CRC_Compute(bytes, 7)
               )
            {
                //序列命令收到，认为成功
            }
            else//发送不成功,立刻重发
            {

                Add_Cholclate();
                return;
            }
        }

        private void TaskControl_Tick(object sender, EventArgs e)
        {
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //1,4,time,0,18,1
        }
        /// <summary>
        /// 任务序列处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MissionQueueSolute(object sender, EventArgs e)
        {
            //
        }
    }
}
