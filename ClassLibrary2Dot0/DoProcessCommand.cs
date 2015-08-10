using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoProcessCommand
    {
        /// <summary>
        /// 执行cmd命令并获取最后一行输出流
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="argument">执行参数</param>
        /// <returns>返回string[2]的数组,元素分别为最后一行输出,异常信息</returns>
        public string[] processCommand(string commandName,string argument)
        {
            ProcessStartInfo ProcessStartInfo1 = new ProcessStartInfo(commandName);
            //设置命令参数
            ProcessStartInfo1.Arguments = argument;
            //不显示dos命令行窗口
            ProcessStartInfo1.CreateNoWindow = true;
            ProcessStartInfo1.RedirectStandardOutput = true;
            ProcessStartInfo1.RedirectStandardInput = true;
            //是否指定操作系统外壳进程启动程序
            ProcessStartInfo1.UseShellExecute = false;

            string[] result = new string[2] { null, null };
            Process Process1 = new Process();
            StreamReader StreamReader1 = null;

            try
            {
                Process1 = Process.Start(ProcessStartInfo1);
                //截取输出流
                StreamReader1 = Process1.StandardOutput;
                string line = StreamReader1.ReadLine();
                while (!StreamReader1.EndOfStream)
                {
                    line = StreamReader1.ReadLine();
                }
                //等待程序执行完退出进程
                Process1.WaitForExit();
                result[0] = line;
            }
            catch (Exception e) {
                result[1] = e.Message;
            }
            //释放资源
            Process1.Dispose();
            if (StreamReader1 != null)
            {
                StreamReader1.Dispose();
            }
            return result;
        }
    }
}
