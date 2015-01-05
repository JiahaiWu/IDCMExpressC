using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IDCM.Data.Base.Utils
{
    public class FileUtil
    {
        public static bool isValidFilePath(string filepath, bool autoCreateFile = false)
        {
            if (filepath != null && filepath.Length > 0)
            {
                try
                {
                    if (File.Exists(filepath))
                        return true;
                    string dir = Path.GetDirectoryName(filepath);
                    if (Directory.Exists(dir) && Path.GetFileName(filepath).Length > 0)
                        return true;
                }
                catch (Exception ex)
                {
                    log.Debug("File Path Detect Failed with Exception: ", ex);
                }
            }
            return false;
        }

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

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
