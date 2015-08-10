
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClassLibrary2Dot0
{
    //public class SharpZipLib
    //{
    //    DoIO DoIO1 = new DoIO();
        
    //    /// <summary>
    //    /// 压缩目录及子目录下的所有文件,保留原来的目录结构
    //    /// </summary>
    //    /// <param name="sourcePath">待压缩的目录</param>
    //    /// <param name="zipSavePath">压缩文件存放的全路径</param>
    //    /// <returns>成功返回null,失败返回异常信息</returns>
    //    public string ZipPath(string sourcePath, string zipSavePath)
    //    {
    //        string result = null;
    //        string createDirResult = DoIO1.createDir(sourcePath);
    //        if (createDirResult != null) {
    //            result = createDirResult;
    //            return result;
    //        }

    //        try
    //        {
    //            object[] filenamesResult = DoIO1.getFileWholePathInFolder(sourcePath);
    //            if (filenamesResult[1] != null) {
    //                result = (string)filenamesResult[1];
    //                return result;
    //            }
    //            List<string> filenames = (List<string>)filenamesResult[0];
    //            string[] sourcePathArr = sourcePath.Replace("\\", "/").Split('/');


    //            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipSavePath)))
    //            {
    //                s.SetLevel(9);
    //                byte[] buffer = new byte[4096];
    //                for (int i=0;i< filenames.Count;i++)
    //                {
    //                    string file = filenames[i];
    //                    string[] fileArr = file.Replace("\\", "/").Split('/');
    //                    string relativePath = string.Empty;
    //                    for (int j = sourcePathArr.Length-1; j < fileArr.Length;j++ )
    //                    {
    //                        relativePath = relativePath + "/" + fileArr[j];
    //                    }
    //                    relativePath = relativePath.Substring(1, relativePath.Length-1);
    //                    ZipEntry entry = new ZipEntry(relativePath);
    //                    entry.DateTime = DateTime.Now;
    //                    s.PutNextEntry(entry);
    //                    using (FileStream fs = File.OpenRead(file))
    //                    {
    //                        int sourceBytes;
    //                        do
    //                        {
    //                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
    //                            s.Write(buffer, 0, sourceBytes);
    //                        } while (sourceBytes > 0);
    //                    }
    //                }
    //                s.Finish();
    //                s.Close();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            result = e.Message;
    //            return result;
    //        }
    //        return result;
    //    }

    //    /// <summary>
    //    /// 解压缩
    //    /// </summary>
    //    /// <param name="sourceFile">源文件</param>
    //    /// <param name="targetPath">目标路经</param>
    //    /// <returns>成功返回null,失败返回异常信息</returns>
    //    public string Decompress(string sourceFile, string targetPath)
    //    {
    //        string result = null;
    //        if (!File.Exists(sourceFile))
    //        {
    //            result = "未能找到压缩文件";
    //        }
    //        try
    //        {
    //            if (!Directory.Exists(targetPath))
    //            {
    //                Directory.CreateDirectory(targetPath);
    //            }
    //            using (ZipInputStream s = new ZipInputStream(File.OpenRead(sourceFile)))
    //            {
    //                ZipEntry theEntry;
    //                while ((theEntry = s.GetNextEntry()) != null)
    //                {
    //                    string directorName = Path.Combine(targetPath, Path.GetDirectoryName(theEntry.Name));
    //                    string fileName = Path.Combine(directorName, Path.GetFileName(theEntry.Name));
    //                    if (theEntry.IsDirectory)
    //                    {
    //                        continue;
    //                    }
    //                    // 创建目录
    //                    if (directorName.Length > 0)
    //                    {
    //                        Directory.CreateDirectory(directorName);
    //                    }
    //                    if (fileName != string.Empty)
    //                    {
    //                        using (FileStream streamWriter = File.Create(fileName))
    //                        {
    //                            int size = 4096;
    //                            byte[] data = new byte[4 * 1024];
    //                            while (true)
    //                            {
    //                                size = s.Read(data, 0, data.Length);
    //                                if (size > 0)
    //                                {
    //                                    streamWriter.Write(data, 0, size);
    //                                }
    //                                else break;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            result = e.Message;
    //        }
    //        return result;
    //    }

     
    //}
}
