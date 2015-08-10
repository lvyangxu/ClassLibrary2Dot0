using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary2Dot0
{
    public class FtpRequest
    {
        /// <summary>
        /// ftp下载逻辑
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="ftpmode"></param>
        /// <param name="savepath"></param>
        /// <param name="savename"></param>
        /// <returns></returns>
        private string ftpDownload(string url, string user, string password, bool ftpmode, string savepath, string savename)
        {
            //新建一个ftp的web请求
            FtpWebRequest FtpWebRequest1 = (FtpWebRequest)WebRequest.Create(url);
            //设置web请求的用户名和密码
            FtpWebRequest1.Credentials = new NetworkCredential(user, password);
            //设置模式为下载
            FtpWebRequest1.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebRequest1.UseBinary = true;

            //ftp传输模式
            FtpWebRequest1.UsePassive = ftpmode;
            FtpWebResponse FtpWebResponse1=null;
            FileStream outputStream=null;
            Stream ftpStream = null;
            try
            {
                //获得服务器的返回
                FtpWebResponse1 = (FtpWebResponse)FtpWebRequest1.GetResponse();
                ftpStream = FtpWebResponse1.GetResponseStream();
                //设置byte数组
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                //存储ftp下载的文件
                if (Directory.Exists(savepath) == false)
                {
                    Directory.CreateDirectory(savepath);
                }
                outputStream = new FileStream((savepath + savename), FileMode.Create);
                //开始读取目标文件的流
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                //清理缓冲区并关闭流
                ftpStream.Dispose();
                outputStream.Flush();
                FtpWebResponse1.Close();
                outputStream.Close();
                return "succ";
            }
            catch (Exception e)
            {
                if (ftpStream != null)
                {
                    try
                    {
                        ftpStream.Dispose();
                    }
                    catch { 
                    
                    }                    
                }
                if (outputStream != null)
                {
                    try
                    {
                        outputStream.Flush();
                        outputStream.Close();
                    }
                    catch
                    {

                    }  

                }
                if (FtpWebResponse1 != null)
                {
                    try
                    {
                        FtpWebResponse1.Close();
                    }
                    catch
                    {

                    }  

                }
                return e.Message;
            }
        }

        /// <summary>
        /// 下载失败后重试一次
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="ftpmode"></param>
        /// <param name="savepath"></param>
        /// <param name="savename"></param>
        /// <returns></returns>
        public string doFtpDownload(string url, string user, string password , string savepath, string savename)
        {
            string isSucc = ftpDownload(url, user, password, false, savepath, savename);
            //被动模式失败后,使用主动模式重试一次
            if (isSucc != "succ")
            {
                isSucc = ftpDownload(url, user, password, true, savepath, savename);
            }
            return isSucc;
        }

        /// <summary>
        /// 获取ftp目录的列表
        /// </summary>
        /// <param name="ftppath"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="ftpmode"></param>
        /// <param name="Regexstr"></param>
        /// <returns></returns>
        private List<string> ftpGetFileList(string ftppath, string user, string password, bool ftpmode, string Regexstr)
        {
            List<string> filelist = new List<string>();
            Regex Regex1 = new Regex(Regexstr);
            //新建一个ftp的web请求
            FtpWebRequest FtpWebRequest1 = (FtpWebRequest)WebRequest.Create(ftppath);
            //设置web请求的用户名和密码
            FtpWebRequest1.Credentials = new NetworkCredential(user, password);
            //设置模式为获取列表
            FtpWebRequest1.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebRequest1.UseBinary = true;

            FtpWebResponse FtpWebResponse1 = null;
            Stream ftpStream = null;
            StreamReader reader = null;

            try
            {
                //ftp传输模式
                FtpWebRequest1.UsePassive = ftpmode;
                //获得服务器的返回
                FtpWebResponse1 = (FtpWebResponse)FtpWebRequest1.GetResponse();
                ftpStream = FtpWebResponse1.GetResponseStream();

                StringBuilder result = new StringBuilder();
                reader = new StreamReader(ftpStream, Encoding.GetEncoding("UTF-8"));
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                //清理缓冲区并关闭流

                reader.Close();
                FtpWebResponse1.Close();
                ftpStream.Dispose();

                foreach (string i in result.ToString().Split('\n'))
                {
                    Match Match1 = Regex1.Match(i);
                    if (Match1.Value != "")
                    {
                        filelist.Add(i);
                    }
                }
                return filelist;
            }
            catch
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (FtpWebResponse1 != null)
                {
                    FtpWebResponse1.Close();
                }
                if (ftpStream != null)
                {
                    ftpStream.Dispose();
                }
                return null;
            }
        }

        /// <summary>
        /// 获取列表失败后重试一次
        /// </summary>
        /// <param name="ftppath"></param>
        /// <param name="user">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        /// <param name="Regexstr"></param>
        /// <returns></returns>
        public List<string> doFtpGetFileList(string ftppath, string user, string password , string Regexstr)
        {
            List<string> filelist = ftpGetFileList( ftppath,  user,  password,  false,  Regexstr);
            if (filelist == null) {
                filelist = ftpGetFileList(ftppath, user, password, true, Regexstr);
            }
            return filelist;
        }

        /// <summary>
        /// ftp上传逻辑
        /// </summary>
        /// <param name="url">需要上传的ftp文件的url全路径</param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="ftpmode"></param>
        /// <param name="filepath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string ftpUpLoad(string url, string user, string password, bool ftpmode, string filepath, string filename)
        {
            //新建一个ftp的web请求
            FtpWebRequest FtpWebRequest1 = (FtpWebRequest)WebRequest.Create(url);
            //设置web请求的用户名和密码
            FtpWebRequest1.Credentials = new NetworkCredential(user, password);
            //设置模式为上传
            FtpWebRequest1.Method = WebRequestMethods.Ftp.UploadFile;
            FtpWebRequest1.UseBinary = true;

            //ftp传输模式
            FtpWebRequest1.UsePassive = ftpmode;
            FileStream outputStream = null;
            Stream ftpStream = null;
            FtpWebResponse FtpWebResponse1 = null;
            try
            {
                FileInfo FileInfo1 = new FileInfo(filepath + filename);
                outputStream = FileInfo1.OpenRead(); 
                //设置byte数组
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];

                //开始上传
                int bytesRead = 0;
                FtpWebResponse1 = (FtpWebResponse)FtpWebRequest1.GetResponse();
                ftpStream = FtpWebRequest1.GetRequestStream();
                while (true)
                {
                    bytesRead = outputStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    ftpStream.Write(buffer, 0, bytesRead);
                }



                //清理缓冲区并关闭流
                ftpStream.Flush();
                ftpStream.Dispose();
                outputStream.Flush();
                outputStream.Dispose();
                FtpWebResponse1.Close();
                return "succ";
            }
            catch (Exception e)
            {
                if (ftpStream != null)
                {
                    ftpStream.Flush();
                    ftpStream.Dispose();
                }
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                }
                if (FtpWebResponse1 != null)
                {
                    FtpWebResponse1.Close();
                }
                return e.Message;
            }
        }

        /// <summary>
        /// 上传失败后重试一次
        /// </summary>
        /// <param name="url">需要上传的ftp文件的url全路径</param>
        /// <param name="user">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        /// <param name="filepath">本地文件的目录,结尾带'/'</param>
        /// <param name="filename">本地文件名</param>
        /// <returns></returns>
        public string doFtpUpLoad(string url, string user, string password, string filepath, string filename)
        {
            string isSucc = ftpUpLoad(url, user, password, false, filepath, filename);
            //被动模式失败后,使用主动模式重试一次
            if (isSucc != "succ")
            {
                isSucc = ftpUpLoad(url, user, password, true, filepath, filename);
            }
            return isSucc;
        }

        /// <summary>
        /// 获取ftp文件的大小，如果失败，返回-1
        /// </summary>
        /// <param name="ftppath"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="ftpmode"></param>
        /// <returns></returns>
        private long getFileLength(string ftppath, string user, string password, bool ftpmode)
        {
            List<string> filelist = new List<string>();

            //新建一个ftp的web请求
            FtpWebRequest FtpWebRequest1 = (FtpWebRequest)WebRequest.Create(ftppath);
            //设置web请求的用户名和密码
            FtpWebRequest1.Credentials = new NetworkCredential(user, password);
            //设置模式为获取列表
            FtpWebRequest1.Method = WebRequestMethods.Ftp.GetFileSize;
            FtpWebRequest1.UseBinary = true;

            FtpWebResponse FtpWebResponse1 = null;

            try
            {
                //ftp传输模式
                FtpWebRequest1.UsePassive = ftpmode;
                //获得服务器的返回
                FtpWebResponse1 = (FtpWebResponse)FtpWebRequest1.GetResponse();
    
                //清理缓冲区并关闭流
                FtpWebResponse1.Close();


                return FtpWebResponse1.ContentLength;
            }
            catch
            {
                if (FtpWebResponse1 != null)
                {
                    FtpWebResponse1.Close();
                }
                return -1;
            }

        }

        public long doGetFileLength(string ftppath, string user, string password) {
            long result = getFileLength(ftppath,user,password,false);
            if (result == -1) {
                result = getFileLength(ftppath, user, password, true);
            }
            return result;
        }
    }
}
