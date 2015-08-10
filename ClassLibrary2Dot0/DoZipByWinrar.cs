using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoZipByWinrar
    {
        /// <summary>
        /// 压缩成zip文件
        /// </summary>
        /// <param name="resourcePath">待压缩的文件、文件夹源路径</param>
        /// <param name="zipPath">压缩后存放的路径</param>
        /// <param name="zipName">文件名</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string CompressZip(string resourcePath, string zipSavePath, string zipName)
        {
            string result = null;
            try
            {
                string RegPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe").GetValue("").ToString();
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe").Close();
                string ZipCommand = " a    " + zipName + " " + resourcePath + " -afzip -r -ep1";
                ProcessStartInfo ProcessStartInfo1 = new ProcessStartInfo();
                ProcessStartInfo1.FileName = RegPath;
                ProcessStartInfo1.Arguments = ZipCommand;
                ProcessStartInfo1.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录
                ProcessStartInfo1.WorkingDirectory = zipSavePath;
                Process Process1 = new Process();
                Process1.StartInfo = ProcessStartInfo1;
                Process1.Start();
                Process1.WaitForExit();
                Process1.Close();
               
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}
