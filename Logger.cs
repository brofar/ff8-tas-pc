using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace FF8_TAS
{
    class Logger
    {
        static readonly string logDirectory = "\\Logs";
        static string logFile = "";

        public static void WriteLog(string strMessage)
        {
            // If we don't have a filename.
            if(logFile.Length == 0)
            {
                logFile = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
            }

            try
            {
                Console.WriteLine(strMessage);

                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + logDirectory, logFile), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
