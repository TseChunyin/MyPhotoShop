using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MyPhotoShop
{
    /// <summary>
    /// ini文件操作
    /// </summary>
    public class IniFile
    {
        private string m_FileName;
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        /// <summary>
        /// 对ini文件的写
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        
        /// <summary>
        /// 对ini文件的读
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, Byte[] retVal, int size, string filePath);
       
        /// <summary>
        /// 读取ini文件的server
        /// </summary>
        /// <param name="iniFilename">地址</param>
        /// <returns></returns>
        public static List<string> ReadSections(string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[10240];
            int len = GetPrivateProfileString(null, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    result.Add(Encoding.UTF8.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }
            return result;
        }
       
        /// <summary>
        /// 获取server下的所有key
        /// </summary>
        /// <param name="iniFilename">地址</param>
        /// <param name="server">节点名</param>
        /// <returns></returns>
        public static List<string> GetSections(string iniFilename, string server)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            int len = GetPrivateProfileString(server, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }
       
        /// <summary>
        /// 获得server下的所有value
        /// </summary>
        /// <param name="iniFilename">地址</param>
        /// <param name="server">节点名</param>
        /// <returns></returns>
        public static List<string> GetValues(string iniFilename, string server)
        {
            List<string> result = new List<string>();
            byte[] buf = null;
            foreach (var item in GetSections(iniFilename, server))
            {
                buf = new byte[1024];
                int len = GetPrivateProfileString(server, item, null, buf, buf.Length, iniFilename);

                result.Add(Encoding.Default.GetString(buf, 0, len));
            }
            return result;
        }
        
        /// <summary>
        /// ini文件添加
        /// </summary>
        /// <param name="section">ini的section</param>
        /// <param name="keys">键的集合</param>
        /// <param name="values">键值集合</param>
        /// <param name="filePath">地址</param>
        public static void WriteSections(string section, List<string> keys, List<string> values, string filePath)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                WritePrivateProfileString(section, keys[i], values[i], filePath);
            }
        }

        /// <summary>
        /// 删除指定区域。
        /// </summary>
        /// <param name="Section">指定区域名。</param>
        /// <returns>返回删除是否成功。</returns>
        public static bool EraseSection(string Section,string filePath)
        {
            return WritePrivateProfileString(Section, null, null, filePath) > 0 ? true : false;
        }

        /// <summary>
        /// 删除指定变量。
        /// </summary>
        /// <param name="Section">变量所在区域。</param>
        /// <param name="Ident">变量标识。</param>
        /// <returns>返回删除是否成功。</returns>
        public static bool DeleteKey(string Section, string Ident,string filePath)
        {
            return WritePrivateProfileString(Section, Ident, null, filePath) > 0 ? true : false;
        }
        /// <summary>
        /// 更新文件。
        /// </summary>
        /// <returns>返回更新是否成功。</returns>
        public static bool UpdateFile(string Section,string filePath)
        {
            return WritePrivateProfileString(Section, null, null, filePath) > 0 ? true : false;
        }
    }
}

