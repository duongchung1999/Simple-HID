using MerryTestFramework.testitem.Headset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    class Program
    {
        //创建辅助类实例
        static Command command = new Command();
        static GetHandle getHandle = new GetHandle();
        static void Main(string[] args)
        {
            //指定控制台字符集编码
            Console.OutputEncoding = Encoding.Unicode;
            //调用GetHandle类中获取句柄方法gethandle=》对应SimpleHIDWrite.exe中插入设备然后出现句柄的操作
            //0951和1747=》对应SimpleHIDWrite.exe句柄上显示的耳机PidVid，也可以从装置管理员中找到设备pidvid（具体操作百度，谷歌）
            getHandle.gethandle("1747", "0951", "xxxx", "xxxx");
            //将GetHandle类获取到的设备地址以及句柄传入到GetRXFW方法中
            GetRXFW(getHandle.headsetpath[0], getHandle.headsethandle[0]);
            //阻塞控制台结束
            Console.ReadKey();
        }

        private static void GetRXFW(string headsetpath, IntPtr headsethandle)
        {
            var value = "07 88 04";
            var indexs = "3 4 5 6";
            var returnvalue = "07 88 04 03";
            if (command.WriteReturn(value, 20, returnvalue, indexs, headsetpath, headsethandle))
            {
                Console.WriteLine("下指令成功");
                Console.WriteLine($"指令回传值为：{command.ReturnValue}");
                var fw = "V";
                //切割字符串获取fw
                foreach (var item in command.ReturnValue.Split(' '))
                {
                    fw += $"{Convert.ToInt32(item, 16)}.";
                }
                //移除最后一个字符.
                Console.WriteLine($"FW为：{fw.Remove(fw.Length-1, 1)}");
            }
            else
            {
                Console.WriteLine("下指令失败");
            }
        }
    }
}
