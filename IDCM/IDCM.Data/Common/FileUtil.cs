using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common
{
    class FileUtil
    {
        public static bool writeToUTF8File(String filepath, String outputStr)
        {
            FileStream fs = new FileStream(filepath, FileMode.Create);
            Byte[] info = new UTF8Encoding(true).GetBytes(outputStr);
            BinaryWriter bw = new BinaryWriter(fs);
            fs.Write(info, 0, info.Length);
            bw.Close();
            fs.Close();
            return true;
        }

        public static String readAsUTF8Text(String filepath)
        {
            if (!System.IO.File.Exists(filepath))
                return null;
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int len = (int)fs.Length;
            Byte[] info = new Byte[len];
            len = br.Read(info, 0, len);
            br.Close();
            fs.Close();
            return new UTF8Encoding(true).GetString(info, 0, len);
        }

        //public static bool simpleLogLine(string outputStr, string filepath = "./error.log")
        //{
        //    try
        //    {
        //        FileStream fs = new FileStream(filepath, FileMode.Append);
        //        Console.WriteLine(outputStr);
        //        Byte[] info = new UTF8Encoding(true).GetBytes(outputStr + "\t[" + DateTime.Now.ToString() + "]\r\n");
        //        BinaryWriter bw = new BinaryWriter(fs);
        //        fs.Write(info, 0, info.Length);
        //        bw.Close();
        //        fs.Close();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error:" + outputStr + "\r\nException:\r\n" + ex.Message + "\n" + ex.StackTrace);
        //        Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
        //    }
        //    return false;
        //}
        /// <summary>
        /// 判断文件是否正在被占用  
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool isFileInUse(string fileName)
        {
            bool inUse = true;
            if (File.Exists(fileName))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    inUse = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
                return inUse;           //true表示正在使用,false没有使用
            }
            else
            {
                return false;           //文件不存在则一定没有被使用
            }
        }
        /// <summary>
        /// 判断指定文件夹归属下的文件是否被占用
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool isFolderInUse(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (isFileInUse(fi.FullName))
                    return true;
            }
            return false;
        }
    }
}
