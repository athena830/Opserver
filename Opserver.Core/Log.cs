using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackExchange.Opserver
{
    public class Log
    {

        public static string DIRNAME = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
        public static string FILENAME = DIRNAME + DateTime.Now.ToString("yyyyMMdd") + ".txt";

        public static object _locked = new object();

        public static void WriteLog(string message)
        {
            if (!Directory.Exists(DIRNAME))
                Directory.CreateDirectory(DIRNAME);

            if (!File.Exists(FILENAME))
            {
                // The File.Create method creates the file and opens a FileStream on the file. You neeed to close it.
                File.Create(FILENAME).Close();
            }

            lock (_locked)
            {
                using (StreamWriter sw = File.AppendText(FILENAME))
                {
                    Logger(message, sw);
                }

            }
        }

        private static void Logger(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("Time:: {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            w.WriteLine("Message:: {0}", logMessage);
            //w.WriteLine("-------------------------------");
        }

        public static void ReadLog(string Date_yyyyMMdd)
        {
            string DIRNAME = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
            string FILENAME = DIRNAME + Date_yyyyMMdd + ".txt";

            if (File.Exists(FILENAME))
            {
                using (StreamReader r = File.OpenText(FILENAME))
                {
                    DumpLog(r);
                }
            }
            else
            {
                Console.WriteLine(Date_yyyyMMdd + ": No Data!");
            }
        }

        private static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
