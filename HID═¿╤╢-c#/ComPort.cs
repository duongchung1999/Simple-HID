
using MerryTestFramework.testitem.Utils;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MerryTestFramework.testitem.Headset
{
    /// <summary>
    /// 自定义串口类
    /// </summary>
    public class ComPort
    {
        /// <summary>
        /// 串口对象
        /// </summary>
        public SerialPort port;
        private DataConversion dataConversion = new DataConversion();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="comport">串口</param>
        /// <param name="baudRate">波特率</param>
        public ComPort(string comport, int baudRate)
        {
            //指定COM口
            port = new SerialPort(comport);
            //指定波特率
            port.BaudRate = baudRate;
            //指定字节标准数据位长度
            port.DataBits = 8;
            //指定字节标准停止位数为1
            port.StopBits = StopBits.One;
            //不发生奇偶验证检查
            port.Parity = Parity.None;
            //启动请求发送RTS信号
            port.RtsEnable = true;
            //启动数据终端就绪DTR信号
            port.DtrEnable = true;
            port.ReadTimeout = 3000;
            port.WriteTimeout = 3000;

            try
            {
                //打开COM口
                port.Open();
            }
            catch (Exception)
            {

            }
        }
        #region 对串口发送数据
        /// <summary>
        /// 对串口发送数据(转为Byte格式)并获取返回值
        /// </summary>
        /// <param name="command">指令</param>
        /// <returns>串口返回值</returns>
        public bool OnlySendByte(string command)
        {
            Thread.Sleep(20);
            var length = command.Split(' ').Length;
            //將指令16進制字符串轉為byte數組
            var byteCommand = dataConversion.GetByteArray(command, length);
            try
            {
                if (!port.IsOpen) port.Open();
                port.Write(byteCommand, 0, length);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region 对串口发送字符串数据
        /// <summary>
        /// 对串口发送数据并获取返回值
        /// </summary>
        /// <param name="command">指令</param>
        /// <returns>串口返回值</returns>
        public bool OnlySends(string command)
        {
            Thread.Sleep(20);
            try
            {
                port.Write(command);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 对串口发送数据并获取返回值
        /// <summary>
        /// 对串口发送数据(转为Byte格式)并获取返回值
        /// </summary>
        /// <param name="command">指令</param>
        /// <returns>串口返回值</returns>
        public string SendByte(string command)
        {
            Thread.Sleep(20);
            var length = command.Split(' ').Length;
            //將指令16進制字符串轉為byte數組
            var byteCommand = dataConversion.GetByteArray(command, length);
            try
            {
                if (!port.IsOpen) port.Open();
                port.Write(byteCommand, 0, length);
                //读取返回数据

                while (port.BytesToRead == 0)
                {
                    Thread.Sleep(1);
                }
                Thread.Sleep(50); //50毫秒内数据接收完毕，可根据实际情况调整
                var recData = new byte[port.BytesToRead];
                port.Read(recData, 0, recData.Length);

                return dataConversion.byteToHexStr(recData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region 对串口发送字符串数据并获取返回值
        /// <summary>
        /// 对串口发送数据并获取返回值
        /// </summary>
        /// <param name="command">指令</param>
        /// <returns>串口返回值</returns>
        public string Sends(string command)
        {
            Thread.Sleep(20);
            try
            {
                port.Write(command);
                //读取返回数据
                /*
                while (port.BytesToRead == 0)
                {
                    Thread.Sleep(1);
                }*/
                Thread.Sleep(50); //50毫秒内数据接收完毕，可根据实际情况调整
                var recData = new byte[port.BytesToRead];
                port.Read(recData, 0, recData.Length);

                return dataConversion.byteToHexStr(recData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        /// <summary>
        /// 关闭COM口
        /// </summary>
        public void Close()
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
                port.Dispose();
            }
        }

    }
}
