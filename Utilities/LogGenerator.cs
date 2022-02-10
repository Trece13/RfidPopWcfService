using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;


namespace Utilities
{
    public class LogGenerator
    {
        public void Write(string msg, string aplication, string method, bool Error = false)
        {
            string path = "C:\\Users\\Md\\AppData\\Local\\Temp\\";
            string fileLog = "logRfidPop.txt";
            try
            {
                string FileS = System.IO.Path.Combine(path, fileLog);
                if (File.Exists(FileS))
                {
                    FileInfo fileinfo = new FileInfo(FileS);
                    if (fileinfo.Length == 5000000)
                    {
                        File.Delete(path);
                        Write(msg, aplication, method, Error);
                    }
                    WriteFile(msg, aplication, method, path, Error);
                }
                else
                {
                    WriteFile(msg, aplication, method, path, Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void WriteFile(string msg, string aplication, string method, string path, bool Error = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("-------------------------------------------");
            sb.Append("\n");
            sb.Append("Date: " + DateTime.Now.ToString());
            sb.Append("\n");
            sb.Append("Application: " + aplication);
            sb.Append("\n");
            sb.Append("Method: " + method);
            sb.Append("\n");
            sb.Append("Error: " + Error);
            sb.Append("\n");
            sb.Append("Messagge: " + msg);
            sb.Append("\n");
            sb.Append("------------------------------------------");
            sb.Append("\n");
            File.AppendAllText(path + "logRfidPop.txt", sb.ToString());
            sb.Clear();
        }
    }
}
