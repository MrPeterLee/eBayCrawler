using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBayFetch
{
    class DirAppend
    {
        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
        }

        public static void DumpLog(StreamReader r)
        {
            // While not at the end of the file, read and write lines.
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            r.Close();
        }
    }
}
