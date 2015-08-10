using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClassLibrary2Dot0
{
    public class DoIO
    {
        /// <summary>
        /// 获取目录及子目录下所有文件的列表
        /// </summary>
        /// <param name="folderpath">路径目录</param>
        /// <returns>返回object的数组，元素分别为string的list文件列表(全路径)和异常信息</returns>
        public object[] getFileWholePathInFolder(string folderpath)
        {
            object[] result = new object[2] { null, null };
            List<string> filelist = new List<string>();
            DirectoryInfo folder = null;
            try
            {
                folder = new DirectoryInfo(folderpath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            //遍历文件
            FileInfo[] filearr = null;
            try
            {
                filearr = folder.GetFiles();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            foreach (FileInfo file in filearr)
            {
                filelist.Add(file.FullName);
            }
            //遍历文件夹
            DirectoryInfo[] dirarr = folder.GetDirectories();
            foreach (DirectoryInfo dir in dirarr)
            {
                //遍历子目录,回调函数本身
                object[] temp = getFileWholePathInFolder(dir.FullName);
                if (temp[1] != null)
                {
                    result[1] = temp[1];
                    return result;
                }
                filelist.AddRange((List<string>)temp[0]);
            }
            result[0] = filelist;
            return result;
        }

        /// <summary>
        /// 获取目录及子目录下所有文件的列表
        /// </summary>
        /// <param name="folderpath">路径目录</param>
        /// <returns>返回object的数组，元素分别为string的list文件列表(全路径)和异常信息</returns>
        public object[] getFileRelativePathInFolder(string folderpath,string relativePath)
        {
            object[] result = new object[2] { null, null };
            List<string> filelist = new List<string>();
            DirectoryInfo folder = null;
            try
            {
                folder = new DirectoryInfo(folderpath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            //遍历文件
            FileInfo[] filearr = null;
            try
            {
                filearr = folder.GetFiles();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            foreach (FileInfo file in filearr)
            {
                filelist.Add(relativePath+"/"+file.Name);
            }
            //遍历文件夹
            DirectoryInfo[] dirarr = folder.GetDirectories();
            foreach (DirectoryInfo dir in dirarr)
            {
                //遍历子目录,回调函数本身
                string[] relativeDirArr = dir.Name.Replace("\\", "/").Split('/');
                string relativeDir = relativeDirArr[relativeDirArr.Length - 1];
                object[] temp = getFileRelativePathInFolder(dir.FullName, relativePath+"/"+relativeDir + "/");
                if (temp[1] != null)
                {
                    result[1] = temp[1];
                    return result;
                }
                filelist.AddRange((List<string>)temp[0]);
            }
            result[0] = filelist;
            return result;
        }


        /// <summary>
        /// 获取目录及子目录下所有文件的列表
        /// </summary>
        /// <param name="folderpath">路径目录</param>
        /// <returns>返回object的数组，元素分别为string的list文件列表(文件名)和异常信息</returns>
        public object[] getFileNameInFolder(string folderpath)
        {
            object[] result = new object[2] { null, null };
            List<string> filelist = new List<string>();
            DirectoryInfo folder = null;
            try
            {
                folder = new DirectoryInfo(folderpath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            //遍历文件
            FileInfo[] filearr = null;
            try
            {
                filearr = folder.GetFiles();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            foreach (FileInfo file in filearr)
            {
                filelist.Add(file.Name);
            }
            //遍历文件夹
            DirectoryInfo[] dirarr = folder.GetDirectories();
            foreach (DirectoryInfo dir in dirarr)
            {
                //遍历子目录,回调函数本身
                object[] temp = getFileWholePathInFolder(dir.FullName);
                if (temp[1] != null)
                {
                    result[1] = temp[1];
                    return result;
                }
                filelist.AddRange((List<string>)temp[0]);
            }
            result[0] = filelist;
            return result;
        }

        /// <summary>
        /// 获取目录(不包括子目录)下所有文件夹
        /// </summary>
        /// <param name="folderpath">路径目录</param>
        /// <returns>返回object的数组，元素分别为string的list文件夹列表(文件夹名称)和异常信息</returns>
        public object[] getFileFolderInDirectlyFolder(string folderpath)
        {
            object[] result = new object[2] { null, null };
            List<string> filelist = new List<string>();
            DirectoryInfo folder = null;
            try
            {
                folder = new DirectoryInfo(folderpath);
            }
            catch (Exception e) {
                result[1] = e.Message;
                return result;       
            }
            //遍历文件夹
            DirectoryInfo[] dirarr = folder.GetDirectories();
            foreach (DirectoryInfo dir in dirarr)
            {
                //遍历子目录,回调函数本身
                filelist.Add(dir.Name);
            }
            result[0] = filelist;
            return result;
        }


        /// <summary>
        /// 以指定的编码读取文本文件,参数文件目录前后不带'/',如果文件不存在返回null
        /// </summary>
        /// <param name="filePath">文件目录,前后不带'/'</param>
        /// <param name="fileName">文件名</param>
        /// <param name="encode">编码</param>
        /// <returns>数组第一个值为文件内容,第二个值为异常内容,成功读取时第一个值返回内容,失败时第一个值为null,第二个值为异常信息</returns>
        public string[] readTextFile(string filePath, string fileName, string encode)
        {
            string[] arr = new string[2] {null,null };
            if (!File.Exists(filePath + "/" + fileName))
            {
                arr[1] = "文件不存在";
                return arr;
            }
            StreamReader StreamReader1 = null;
            try
            {
                StreamReader1 = new StreamReader(filePath + "/" + fileName, Encoding.GetEncoding(encode));
                string result = StreamReader1.ReadToEnd();
                StreamReader1.Dispose();
                arr[0] = result;
                return arr;
            }
            catch (Exception e)
            {
                if (StreamReader1 != null)
                {
                    StreamReader1.Dispose();
                }
                arr[1] ="读取文件" + filePath + "/" + fileName + "失败:" + e.Message ;
                return arr;
            }
        }

        /// <summary>
        /// 以指定的编码读取文本文件,参数文件目录前后不带'/',如果文件不存在返回null
        /// </summary>
        /// <param name="wholeFilePath">文件路径</param>
        /// <param name="encode">编码</param>
        /// <returns>数组第一个值为文件内容,第二个值为异常内容,成功读取时第一个值返回内容,失败时第一个值为null,第二个值为异常信息</returns>
        public string[] readTextFile(string wholeFilePath, string encode)
        {
            string[] arr = new string[2] { null, null };
            if (!File.Exists(wholeFilePath))
            {
                arr[1] = "文件不存在";
                return arr;
            }
            StreamReader StreamReader1 = null;
            try
            {
                StreamReader1 = new StreamReader(wholeFilePath, Encoding.GetEncoding(encode));
                string result = StreamReader1.ReadToEnd();
                StreamReader1.Dispose();
                arr[0] = result;
                return arr;
            }
            catch (Exception e)
            {
                if (StreamReader1 != null)
                {
                    StreamReader1.Dispose();
                }
                arr[1] = "读取文件" + wholeFilePath + "失败:" + e.Message;
                return arr;
            }
        }


        /// <summary>
        /// 以指定的字符和编码创建文本文件
        /// </summary>
        /// <param name="filePath">文件目录</param>
        /// <param name="fileName">文件名</param>
        /// <param name="encode">编码</param>
        /// <param name="content">字符内容</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string createTextFile(string filePath, string fileName, string encode, string content)
        {
            //目录校验
            if (filePath[filePath.Length - 1] == '\\' || filePath[filePath.Length - 1] == '/')
            {
                filePath = filePath.Substring(0, filePath.Length - 1);
            }

            string createResult = createDir(filePath);
            if (createResult != null)
            {
                return createResult + ",将不会执行文件创建操作";
            }
            StreamWriter StreamWriter1 = null;
            try
            {
                StreamWriter1 = new StreamWriter(filePath + "/" + fileName, false, Encoding.GetEncoding(encode));
                StreamWriter1.Write(content);
                StreamWriter1.Flush();
                StreamWriter1.Dispose();
            }
            catch (Exception e)
            {
                if (StreamWriter1 != null)
                {
                    StreamWriter1.Dispose();
                }
                return "写入文件" + filePath + "/" + fileName + "时失败:" + e.Message;
            }            
            return null;
        }

        /// <summary>
        /// 创建目录,参数目录前后不带'/',成功创建后或不需要创建时返回succ,失败返回异常的Message
        /// </summary>
        /// <param name="filePath">文件目录,前后不带'/'</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string createDir(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                try
                {
                    Directory.CreateDirectory(filePath);
                }
                catch (Exception e)
                {
                    return "创建目录" + filePath + "失败:" + e.Message;
                }
            }
            return null;
        }

        /// <summary>
        /// 文件夹拷贝
        /// </summary>
        /// <param name="directoryPath">源路径</param>
        /// <param name="destinationPath">目标路径</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string directoryCopy(string directoryPath,string destinationPath) {
            string result = null;
            object[] getResult = getFileWholePathInFolder(directoryPath);
            if (getResult[1] != null)
            {
                return (string)getResult[1];
            }

            List<string> fileResult = (List<string>)getResult[0];
            try
            {
                foreach (string filePath in fileResult)
                {
                    string destinationChildDirAndFileName = filePath.Substring(directoryPath.Length, filePath.Length - directoryPath.Length);
                    
                       string[] arr = destinationChildDirAndFileName.Replace("\\","/").Split('/');
                       string destinationChildDir = destinationChildDirAndFileName.Substring(0, destinationChildDirAndFileName.Length-arr[arr.Length - 1].Length);
                    createDir(destinationPath + destinationChildDir);
                    File.Copy(filePath, destinationPath + destinationChildDirAndFileName, true);
                }
            }
            catch(Exception e) {
                result = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="fileSourcePath"></param>
        /// <param name="fileDestinationPath"></param>
        /// <returns></returns>
        public string renameFile(string fileSourcePath, string fileDestinationPath)
        {
            string result = null;
            try
            {
                File.Move(fileSourcePath, fileDestinationPath);
            }
            catch(Exception e)
            {
                result = e.Message;
            } 
            return result;
        }

        /// <summary>
        /// 清空目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string clearDirectory(string directoryPath)
        {
            string result = null;
            if (Directory.Exists(directoryPath)) {
                try
                {
                    Directory.Delete(directoryPath, true);
                }
                catch (Exception e) {
                    result = e.Message;
                    return result;
                }
            }

            try
            {
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception e) {
                result = e.Message;
                return result;         
            }

            return result;
        }

        /// <summary>
        /// 获取目录下的第一个文件名
        /// </summary>
        /// <param name="folderpath">目录全路径</param>
        /// <returns>返回string的数组,元素分别为文件名、异常信息</returns>
        public string[] getFileNameInDirectory(string folderpath)
        {
            string[] result = new string[2] { null, null };
            DirectoryInfo folder = null;
            try
            {
                folder = new DirectoryInfo(folderpath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            //遍历文件
            FileInfo[] filearr = null;
            try
            {
                filearr = folder.GetFiles();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            result[0] = filearr[0].Name;            
            return result;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileWholePath">文件全路径</param>
        /// <returns>成功返回null，失败返回异常信息</returns>
        public string deleteFile(string fileWholePath) {
            string result = null;
            if (File.Exists(fileWholePath)) {
                try
                {
                    File.Delete(fileWholePath);
                }
                catch (Exception e) {
                    result = e.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据流存储文件
        /// </summary>
        /// <param name="Stream1"></param>
        /// <param name="savePath"></param>
        /// <param name="saveName"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string saveFileByStream(Stream Stream1,string savePath,string saveName,string encode) {
            string result = null;
            string fileString = string.Empty;
            try
            {
                StreamReader StreamReader1 = new StreamReader(Stream1);
                fileString = StreamReader1.ReadToEnd();
            }
            catch (Exception e) {
                result = e.Message;
                return result;
            }
            string createResult = createTextFile(savePath, saveName, encode, fileString);
            result = createResult;
            return result;
        }
    }
}
