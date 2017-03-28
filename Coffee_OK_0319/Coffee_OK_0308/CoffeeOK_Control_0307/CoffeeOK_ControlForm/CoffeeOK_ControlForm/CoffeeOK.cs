using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading;
using System.IO.Ports;

namespace CoffeeOK
{

    class CoffeeOK_RobertConnection
    {
        SerialPort serialport = new System.IO.Ports.SerialPort();
        public string SerialPortName = "COM1";
        List<string> result = new List<string>();
        public byte[] ResponseBytesBuf = new byte[1000];
        int ResponseBytesPoint = 0;
        public byte[] ReadBytesBuf = new byte[1000];
        int ReceivePoint = 0;

        public List<int> TaskList = new List<int>();
        int LatestTask = 0;

        public string Record = "";

        public bool WorkingConditionFlag = false;

        System.Timers.Timer DoOrder = new System.Timers.Timer();

       

        /// <summary>
        /// 读M550判断是否工作完成，若完成，则返回0x01,否则返回0x00
        /// </summary>
        public string ReadMissionMode = "" + (char)0x02 + (char)0x01 + (char)0x12 + (char)0x26 + (char)0x00 + (char)0x01;

        /// <summary>
        /// 请求地址为M550的输出为OFF（0）
        /// </summary>
        public byte[] RenewMissionMode = { 0x02, 0x05, 0x12, 0x26, 0x00, 0x00 };

        #region 执行指令程序1-8，顺序执行
        public byte[] Order_DoMission1 = { 0x02, 0x05, 0x11, 0xF4, 0xFF, 0x00 };//M500 伸手取杯并等待M551置位，然后拿回杯子
        public byte[] Order_DoMission2 = { 0x02, 0x05, 0x11, 0xF5, 0xFF, 0x00 };//M501 放咖啡杯到咖啡机，点击制作美式咖啡命令
        public byte[] Order_DoMission3 = { 0x02, 0x05, 0x11, 0xF6, 0xFF, 0x00 };//M502 放咖啡杯到咖啡机，点击制作意式浓缩咖啡命令
        public byte[] Order_DoMission4 = { 0x02, 0x05, 0x11, 0xF7, 0xFF, 0x00 };//M503 放杯子到调料位
        public byte[] Order_DoMission5 = { 0x02, 0x05, 0x11, 0xF8, 0xFF, 0x00 };//M504 从调料位置放置杯子到出货位
        public byte[] Order_DoMission6 = { 0x02, 0x05, 0x11, 0xF9, 0xFF, 0x00 };//M505 放咖啡杯到咖啡机，点击制作卡布奇诺咖啡命令
        public byte[] Order_DoMission7 = { 0x02, 0x05, 0x11, 0xFA, 0xFF, 0x00 };//M506 放咖啡杯到咖啡机，点击制作拿铁咖啡命令
        public byte[] Order_DoMission8 = { 0x02, 0x05, 0x11, 0xFB, 0xFF, 0x00 };//M507 从咖啡机放杯子到出货位，待机器人完善指令

        /// <summary>
        /// 继续取杯后的动作
        /// </summary>
        public byte[] Order_DoMission1_Continue = { 0x02, 0x05, 0x12, 0x27, 0xFF, 0x00 };//M551

        public byte[] Order_InitRobertWorkingCondition = { 0x02, 0x05, 0x12, 0x26, 0xFF, 0x00 };//M550

        public byte[] Order_RobotPowerOn = { 0x02, 0x05, 0x10, (byte)(0x60 + 13), 0xFF, 0x00 };//M109,远程上电
        public byte[] Order_RobotPowerOff = { 0x02, 0x05, 0x10, (byte)(0x60 + 13), 0x00, 0x00 };//M109,远程上电
        
        public byte[] Order_RobotReset = { 0x02, 0x05, 0x10, (byte)(0x60 + 14), 0xFF, 0x00 };//M110,远程复位
        public byte[] Order_RobotReset1 = { 0x02, 0x05, 0x10, (byte)(0x60 + 11), 0x00, 0x00 };//M107,远程复位
        public byte[] Order_RobotReset2 = { 0x02, 0x05, 0x10, (byte)(0x60 + 11), 0xFF, 0x00 };//M107,远程复位
        //public byte[] Order_RobotReset = { 0x02, 0x05, 0x10, (byte)(0x60 + 14), 0x00, 0x00 };//M110,远程复位

        public byte[] Order_FarCodeEnable1 = { 0x02, 0x05, 0x11, (byte)(0x4A), 0xFF, 0x00 };//M330,远程执行允许
        public byte[] Order_FarCodeEnable2 = { 0x02, 0x05, 0x11, (byte)(0x54), 0xFF, 0x00 };//M340,远程执行允许
        public byte[] Order_FarCodeRunUp = { 0x02, 0x05, 0x11, (byte)(0x5E), 0xFF, 0x00 };//M350,远程执行开始
        public byte[] Order_FarCodeRunDown = { 0x02, 0x05, 0x11, (byte)(0x5E), 0x00, 0x00 };//M350,远程执行开始

        #endregion

        /// <summary>
        /// 写命令头
        /// </summary>
        string WriteHead = "" + (char)0x02 + (char)0x05;


        /// <summary>
        /// 写命令基地址，即M500变量的地址，也即第一个任务的地址
        /// </summary>
        byte BaseAddressHigh = 0x11;
        byte BaseAddressLow = 0xF4;

        string BaseAddress = "";


        /// <summary>
        /// 写ON命令的命令尾
        /// </summary>
        string WriteTrueEnd = "" + (char)0xff + (char)0x00;

        /// <summary>
        /// 写OFF命令的命令尾
        /// </summary>
        string WriteFalseEnd = "" + (char)0x00 + (char)0x00;

        public CoffeeOK_RobertConnection()
        {
            DoOrder.Interval = 500;//100ms
            DoOrder.Elapsed += DoOrder_Elapsed;
        }
        public void StartTaskServer()
        {
            DoOrder.Start();
        }
        public void StopTaskServer()
        {
            DoOrder.Stop();
        }

        /// <summary>
        /// 主计时器，用于定时查询机器人工作状态，并下发工作指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoOrder_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //首先，查询当前任务是否完成
            WorkingConditionFlag = GetWorkConditionNow();
            if (WorkingConditionFlag)
            {
                //机器正在执行任务，不做动作
            }
            else
            {//机器人已经停止执行任务，可以下发下一步动作
                if (TaskList.Count > 0)
                {
                    byte[] str_tosend = new byte[0];
                    switch (TaskList[0])
                    {
                        case 1:
                            str_tosend = Order_DoMission1;
                            break;
                        case 2:
                            str_tosend = Order_DoMission2;
                            break;
                        case 3:
                            str_tosend = Order_DoMission3;
                            break;
                        case 4:
                            str_tosend = Order_DoMission4;
                            break;
                        case 5:
                            str_tosend = Order_DoMission5;
                            break;

                        case 6:
                            str_tosend = Order_DoMission6;
                            break;

                        case 7:
                            str_tosend = Order_DoMission7;
                            break;
                        case 8:
                            str_tosend = Order_DoMission8;
                            break;
                    }

                    if (str_tosend.Length > 0)
                    {
                        WriteRobot(str_tosend);
                        byte[] bytes = GetResPonseData();
                        if (bytes.Length > 5 && bytes[0] == 0x02 && bytes[1] == 0x05)
                        {
                            //写入成功
                            TaskList.RemoveAt(0);
                        }
                        else
                        {
                            //写入不成功，暂不添加逻辑
                        }
                    }
                }
                else
                {
                    //没有可执行任务

                }
            }
            //
        }


        #region 上位机控制指令发送
        public void WriteRobot(byte[] inputstring)
        {
            //if (Conn.State == ConnectionState.Closed)
            //       Conn.Open();


            byte[] bytes = inputstring;// Encoding.UTF8.GetBytes(inputstring);//inputstring.Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();
            byte[] SHOW = bytes;
            byte[] crc = CalculateCRC(bytes);
            byte[] AddSHOW = new byte[SHOW.Length + 2];
            for (int i = 0; i < SHOW.Length; i++)
            {
                AddSHOW[i] = SHOW[i];
            }
            AddSHOW[AddSHOW.Length - 2] = crc[0];
            AddSHOW[AddSHOW.Length - 1] = crc[1];
            serialport.Write(AddSHOW, 0, AddSHOW.Length);
            Thread.Sleep(200);
            byte[] recivedata = new byte[serialport.BytesToRead];
            if (serialport.BytesToRead >= 5)
            {
                bytes = new byte[serialport.BytesToRead];
                serialport.Read(recivedata, 0, recivedata.Length);
                byte[] temp = CheckCRC(recivedata);
                if (temp[1] == recivedata[recivedata.Length - 1] && temp[0] == recivedata[recivedata.Length - 2])//注意高低位调换
                {
                    for (int i = 0; i < Display(recivedata).Count; i++)
                    {
                        //Console.WriteLine(Display(recivedata)[i]);
                        ResponseBytesBuf[ResponseBytesPoint] = recivedata[i];
                        ResponseBytesPoint++;
                    }

                    //Console.ReadLine();
                }
                else
                {
                    //Console.WriteLine("接收数据错误");
                }
            }
        }
        public byte[] GetResPonseData()
        {
            int counter = ResponseBytesPoint;
            byte[] bytes = new byte[ResponseBytesPoint + 1];
            for (int i = 0; i < (ResponseBytesPoint + 1); i++)
            {
                bytes[i] = ResponseBytesBuf[i];
            }
            ResponseBytesPoint -= counter;
            return bytes;
        }
        public int GetResPonseDataLenth()
        {
            return ResponseBytesPoint;
        }
        #endregion

        #region 读取机器人状态变量
        public void ReadRobot(string inputstring)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputstring);// inputstring.Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();
            byte[] SHOW = bytes;
            byte[] crc = CalculateCRC(bytes);
            byte[] AddSHOW = new byte[SHOW.Length + 2];
            for (int i = 0; i < SHOW.Length; i++)
            {
                AddSHOW[i] = SHOW[i];
            }
            AddSHOW[AddSHOW.Length - 2] = crc[0];
            AddSHOW[AddSHOW.Length - 1] = crc[1];
            serialport.Write(AddSHOW, 0, AddSHOW.Length);
            Thread.Sleep(200);
            byte[] recivedata = new byte[serialport.BytesToRead];
            if (serialport.BytesToRead >= 5)
            {
                bytes = new byte[serialport.BytesToRead];
                serialport.Read(recivedata, 0, recivedata.Length);
                byte[] temp = CheckCRC(recivedata);
                if (temp[1] == recivedata[recivedata.Length - 1] && temp[0] == recivedata[recivedata.Length - 2])//注意高低位调换
                {
                    for (int i = 0; i < Display(recivedata).Count; i++)
                    {
                        //Console.WriteLine(Display(recivedata)[i]);
                        ReadBytesBuf[ReceivePoint] = recivedata[i];
                        ReceivePoint++;
                    }

                    //Console.ReadLine();
                }
                else
                {
                    //Console.WriteLine("接收数据错误");
                }
            }
        }
        #endregion

        /// <summary>
        /// 读取当前机器人运行任务状态，如果有任务运行，返回false，否则返回true
        /// </summary>
        /// <returns></returns>
        public bool GetWorkConditionNow()
        {
            ReadRobot(ReadMissionMode);
            byte[] bytes = GetReadBytes();
            if (bytes.Length > 5 && bytes[0] == 0x02 && bytes[1] == 0x02)
            {
                if (bytes[3] > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {//默认为false回复
                return false;
            }
        }

        public byte[] GetReadBytes()
        {
            int counter = ReceivePoint;
            byte[] bytes = new byte[ReceivePoint + 1];
            for (int i = 0; i < (ReceivePoint + 1); i++)
            {
                bytes[i] = ReadBytesBuf[i];

            }
            ReceivePoint -= counter;
            return bytes;
        }
        public int GetReadBytesLenth()
        {
            return ReceivePoint;
        }

        /// <summary>
        /// 地址换算
        /// </summary>
        /// <param name="address">M地址，比如M500，则address=500</param>
        /// <returns>返回的字符串，根据高低字节拆分为两个字节</returns>
        public string GetWriteAddress_M(int address)
        {
            BaseAddress = "" + (char)BaseAddressHigh + (char)(BaseAddressLow + address);
            return BaseAddress;
        }

        public List<string> Display(byte[] bytes)
        {
            result.Clear();
            foreach (byte item in bytes)
            {
                result.Add(string.Format("{0:X2} ", item));
            }
            return result;
        }
        #region 初始化串口
        public bool SerialPortInit()
        {
            serialport.BaudRate = 9600;
            serialport.DataBits = 8;
            serialport.Parity = Parity.None;
            serialport.StopBits = StopBits.One;
            serialport.DtrEnable = true;
            serialport.RtsEnable = true;
            serialport.Encoding = Encoding.UTF8;
            if (SerialPortName != null && SerialPortName.Contains("COM") && serialport.IsOpen == false)
            {
                serialport.PortName = SerialPortName;// Console.ReadLine();
            }
            try
            {
                serialport.Open();
                return true;
            }
            catch
            {
                Console.WriteLine("Port open failed");
                Console.ReadLine();
                return false;
            }
        }
        #endregion
        #region 主站生成CRC16校验码
        private byte[] CalculateCRC(byte[] frame)
        {
            byte[] result = new byte[2];
            ushort CRCFull = 0xFFFF;
            char CRCLSB;
            for (int i = 0; i < frame.Length; i++)
            {
                CRCFull = (ushort)(CRCFull ^ frame[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (Char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                    {
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                    }
                }
            }
            result[1] = (byte)((CRCFull >> 8) & 0xFF);
            result[0] = (byte)(CRCFull & 0xFF);
            return result;
        }
        #endregion
        #region 校验从站返回CRC16
        private byte[] CheckCRC(byte[] frame)
        {
            byte[] result = new byte[2];
            ushort CRCFull = 0xFFFF;
            char CRCLSB;
            for (int i = 0; i < frame.Length - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ frame[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (Char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                    {
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                    }
                }
            }
            result[1] = (byte)((CRCFull >> 8) & 0xFF);
            result[0] = (byte)(CRCFull & 0xFF);
            return result;
        }
        #endregion
    }
}
/* 解释
机器人对应的子程序判断变量为M300-M309（对应映射地址为112c-1135），M330-M339（对应映射地址为114a-1153）
共20个子程序。 m200为状态变量可通过读取这个变量判断子程序是否执行完毕。
modbus对应单线圈写入指令格式以写M300置位为例02 05 11 2c ff 00 返回为02 05 11 2c ff 00
modbus对应单线圈读出指令格式以M200被置位为例02 01 10 c8 00 01 返回为02 01 11 2c 01 01
 */
