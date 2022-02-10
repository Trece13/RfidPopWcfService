using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLog("xxx", "xxx", "xxx", "xxx");
        }

        public static void WriteLog(string msg, string aplication, string method, string ClassOrigin, bool Error = false)
        {
            string path = "C:\\Users\\Md\\AppData\\Local\\Temp\\";
            string fileLog = "logRfidPop.txt";
            try
            {
                string FileS = System.IO.Path.Combine(path,fileLog);
                if (File.Exists(FileS))
                {
                    FileInfo fileinfo = new FileInfo(FileS);
                    if (fileinfo.Length == 5000000)
                    {
                        File.Delete(path);
                        WriteLog(msg, aplication, method, ClassOrigin, Error);
                    }
                    WriteFile(msg, aplication, method, path, Error);
                }
                else
                {
                    WriteFile(msg, aplication, method , path, Error);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void WriteFile(string msg, string aplication, string method,string path ,bool Error = false){
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
