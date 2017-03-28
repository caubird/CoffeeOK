using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace CoffeeOK_ControlForm
{
    class ConnectControlBoard
    {

        #region 命令帧汇总

        public byte[] Order_ControlBoard_ActionInit = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x00 };
        public byte[] Order_ControlBoard_ReleaseACup = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x01 };
        public byte[] Order_ControlBoard_ChangeCupQueue = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x02 };
        public byte[] Order_ControlBoard_AddSeasoning1 = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x03 };
        public byte[] Order_ControlBoard_AddSeasoning1_Half = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x03 };
        public byte[] Order_ControlBoard_AddSeasoning2 = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x04 };
        public byte[] Order_ControlBoard_AddSeasoning2_Half = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x03 };
        public byte[] Order_ControlBoard_AddSeasoning3 = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x05 };
        public byte[] Order_ControlBoard_AddSeasoning3_Half = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x03 };
        public byte[] Order_ControlBoard_AddSeasoning4 = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x06 };
        public byte[] Order_ControlBoard_AddSeasoning4_Half = { 0x55, 0xAA, 0x02, 0x04, 0x00, 0x01, 0x03 };

        public byte[] Order_ControlBoard_ReplyOK = { 0x55, 0xAA, 0x20, 0x04, 0x00, 0x01, 0x00 };

        #endregion 命令帧汇总
        #region 变量声明
        public System.IO.Ports.SerialPort ComPort = new System.IO.Ports.SerialPort();
        public ConnectControlBoard()
        {
            ReadTimer.Elapsed += ReadTimer_Elapsed;
            WriteTimer.Elapsed += WriteTimer_Elapsed;
            InitComPort("COM1");
        }

        List<byte[]> WriteFrameList = new List<byte[]>();
        List<byte[]> ReadFrameList = new List<byte[]>();
        int ResponseBytesPoint = 0;
        byte[] LastSend = new byte[0];
        public string ComPortName = "";

        public List<FrameItem> FrameList = new List<FrameItem>();

        public static byte CRC_Compute(byte[] data, int dataLen)
        {
            byte chk_sum = 0; // initialize check sum;
            byte[] ptr = data;

            for (int i = 0; i < dataLen; i++)
            {
                chk_sum ^= data[i];
            }

            return chk_sum;
        }

        //public System.IO.Ports.SerialPort ComPort = new System.IO.Ports.SerialPort();

        public byte[] OgnizeReportFrame(byte[] ReportItem)
        {
            int length = 7 + ReportItem.Length;
            byte[] result = new byte[length];

            FrameItem Frame = new FrameItem();
            Frame.ReportItem = ReportItem;

            result[0] = Frame.HeadByte[0];
            result[1] = Frame.HeadByte[1];
            result[2] = Frame.FrameType;
            result[3] = Frame.ReportType;
            result[4] = Frame.FrameLenth[0];
            result[5] = Frame.FrameLenth[1];
            for (int i = 0; i < Frame.ReportItem.Length; i++)
            {
                result[i] = Frame.ReportItem[i];
            }
            result[6 + Frame.ReportItem.Length] = CRC_Compute(result, result.Length - 1);
            return result;
        }

        public System.Timers.Timer ReadTimer = new System.Timers.Timer();

        public System.Timers.Timer WriteTimer = new System.Timers.Timer();

        byte[] DataReadBack = new byte[2000];
        int DataReadCount = 0;
        #endregion
        public void InitComPort(string portname)
        {
            //ComPort.PortName = ComPortName;
           // ComPort.BaudRate = 4800;
            ComPort.DataBits = 8;
            ComPort.StopBits = System.IO.Ports.StopBits.One;
            ComPort.Parity = System.IO.Ports.Parity.None;
            ComPort.Encoding = Encoding.UTF8;
            ComPort.BaudRate = 115200;
            ComPort.PortName = portname;
            //ComPort.Open();
        }
        public void StartConnection()
        {
            ComPort.Open();//打开端口
            ReadTimer.Interval = 100;//100ms
            WriteTimer.Interval = 100;//100ms
            WriteTimer.Start();
            ReadTimer.Start();
        }

        public void WriteControlSystem(byte[] inputstring)
        {
            //if (Conn.State == ConnectionState.Closed)
            //       Conn.Open();

            if (!ComPort.IsOpen)
            {
                try
                {
                    ComPort.Open();
                }
                catch
                {//COM口初始化失败
                    return;
                }
            }
            byte[] bytes = inputstring;// Encoding.UTF8.GetBytes(inputstring);//inputstring.Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();
            byte[] SHOW = bytes;
            byte crc = CRC_Compute(bytes,bytes.Length );
            byte[] AddSHOW = new byte[SHOW.Length + 1];
            for (int i = 0; i < SHOW.Length; i++)
            {
                AddSHOW[i] = SHOW[i];
            }
            //AddSHOW[AddSHOW.Length - 2] = crc[0];
            AddSHOW[AddSHOW.Length - 1] = crc;
            ComPort.Write(AddSHOW, 0, AddSHOW.Length);
            Thread.Sleep(AddSHOW.Length / 10 + 1);
            //Thread.Sleep(200);
            //byte[] recivedata = new byte[ComPort.BytesToRead];
            //if (ComPort.BytesToRead >= 5)
            //{
            //    bytes = new byte[ComPort.BytesToRead];
            //    ComPort.Read(recivedata, 0, recivedata.Length);
            //    byte temp = CRC_Compute(recivedata,recivedata.Length);
            //    if (temp == recivedata[recivedata.Length - 1] )//注意高低位调换
            //    {
                    
            //        ReadFrameList.Insert(ResponseBytesPoint,recivedata) ;
            //        ResponseBytesPoint++;
            //        //Console.ReadLine();
            //    }
            //    else
            //    {
            //        //Console.WriteLine("接收数据错误");
            //    }
            //}
        }
        private void WriteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ComPort.IsOpen && WriteFrameList.Count > 0)
            {
                //byte bytes=new byte [[0].]
                ComPort.Write(WriteFrameList[0], 0, WriteFrameList[0].Length);
                LastSend = WriteFrameList[0];
                WriteFrameList.RemoveAt(0);//.Remove(WriteFrameList[0])
            }
        }

        private void ReadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ComPort.BytesToRead > 0)
            {
                int counter = ComPort.BytesToRead;
                for (int i = 0; i < counter; i++)
                {
                    DataReadBack[DataReadCount] = (byte)ComPort.ReadByte();
                    DataReadCount++;
                }
            }
            if (DataReadCount > 7)
            {//满足解析的必要条件
                //
                for (int i = 0; (i + 1) < DataReadCount; i++)
                {
                    if (DataReadBack[i] == 0x55 && DataReadBack[i + 1] == 0xAA && (DataReadCount - i) >= 7)
                    {
                        FrameCopy(i);
                        ReorgnizeBuf(i);
                    }
                    else if ((DataReadCount - i) < 7)
                    {
                        //未接受完，继续接收等待
                    }
                }
            }
        }

        #region 接收数据实际处理部分
        /// <summary>
        /// 将一帧数据从接收缓冲区抓取出来，并加入接收队列，等待处理
        /// </summary>
        /// <param name="Index"></param>
        private void FrameCopy(int Index)
        {
            //找到帧长度，根据帧长度判定本帧数据是否接收完成
            int length = DataReadBack[4 + Index] * 256 + DataReadBack[5 + Index];
            if (DataReadCount >= length + 7)
            {
                byte CRC_Received = DataReadBack[Index + length + 6];
                if (CRC_Received == CRC_Compute(DataReadBack, length + 6))
                {
                    byte[] framebytes = new byte[length + 7];
                    for (int i = 0; i < length + 7; i++)
                    {
                        framebytes[i] = DataReadBack[Index + i];
                    }
                    ReadFrameList.Add(framebytes);//把一帧加入到接收列表
                }
            }
        }
        private void ReorgnizeBuf(int Index)
        {
            int length = DataReadBack[4 + Index] * 256 + DataReadBack[5 + Index];
            if (DataReadBack.Length >= Index + 6 + length)
            {
                ///移动后面内容到缓冲区前部分，前面部分内容覆盖
                for (int i = Index + 6 + length; i <= DataReadBack.Length; i++)
                {
                    DataReadBack[i - Index - 6 - length] = DataReadBack[Index];

                }
                //接收缓冲区标记长度缩减
                DataReadCount -= Index + 6 + length;
            }

        }

        public byte[] ReadBackBytes(SerialPort sp1)
        {
            if (sp1.IsOpen)
            {
                return null;
            }
            else
            {
                try
                {
                    sp1.Open();
                }
                catch
                {
                    return null;
                }
            }
            int counter = sp1.BytesToRead;
            byte[] bytes = new byte[counter];
            sp1.Read(bytes, 0, counter);

            return bytes;
        }

        public byte[] ReadBackBytes()
        {
            //byte[] bytes = new byte[10];
            int counter = ComPort.BytesToRead;
            byte[] bytes = new byte[counter];
            ComPort.Read(bytes, 0, counter);

            return bytes;
        }

        /// <summary>
        /// 协议内容分析
        /// </summary>
        private void FrameAnalysis()
        {
            for (int i = 0; i < FrameList.Count; i++)
            {
                byte[] FrameBytes = ReadFrameList[i];
                byte[] ReportItem = new byte[FrameBytes.Length - 7];
                byte ReportType = FrameBytes[5];
                for (int j = 0; j < ReportItem.Length; j++)
                {
                    ReportItem[i] = FrameBytes[i + 6];
                }
            }
        }
        #endregion 接收数据实际处理部分

        #region 发送数据组织部分

        #endregion 发送数据组织部分
    }

    public class FrameItem
    {
        public byte[] HeadByte = { 0x55, 0xAA };
        //0x01	1	STM32向上位机发送请求报文
        //0x10	1	上位机向STM32发送响应报文
        //0x02	1	上位机向STM32发送请求报文
        //0x20	1	STM32向上位机发送响应报文
        /// <summary>
        /// 帧类型（第三字节）
        /// </summary>
        public byte FrameType = 0x02;
        /// <summary>
        /// 报文类型（第四字节）
        /// </summary>
        /*
         * §1.1.5.1	报警状态上报报文（01）
         * §1.1.5.2	工序状态上报报文（02）
         * §1.1.5.3	报警状态查询报文（03）
         * §1.1.5.4	状态控制报文(04)
         */
        public byte ReportType = 0x01;//默认为报警状态上报报文

        public byte[] FrameLenth = { 0x00, 0x00 };

        public byte[] ReportItem = new byte[0];

        public byte CRCResult = 0x00;
    }
}
