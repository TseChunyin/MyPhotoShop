using System;
using System.IO;

namespace MyPhotoShop
{
    public class LogHelper
    {
        private static StreamWriter streamWriter; //写文件    

        public static void WriteError(string message)
        {
            try
            {
                //DateTime dt = new DateTime();  
                string directPath = Environment.CurrentDirectory + @"\log\";   //在获得文件夹路径  
                if (!Directory.Exists(directPath))   //判断文件夹是否存在，如果不存在则创建  
                {
                    Directory.CreateDirectory(directPath);
                }
                directPath += string.Format(@"\{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                if (streamWriter == null)
                {
                    streamWriter = !File.Exists(directPath) ? File.CreateText(directPath) : File.AppendText(directPath);    //判断文件是否存在如果不存在则创建，如果存在则添加。  
                }
                streamWriter.WriteLine("***********************************************************************");
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                streamWriter.WriteLine("输出信息：错误信息");
                if (message != null)
                {
                    streamWriter.WriteLine("异常信息：\r\n" + message);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
        }
    }
}

