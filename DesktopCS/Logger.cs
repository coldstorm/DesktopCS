using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DesktopCS
{
    public class Logger
    {
        public static void Log(string message)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DesktopCS.log";

            try
            {
                StreamWriter writer = new StreamWriter(path, true);

                writer.WriteLine("{0} {1}", DateTime.Now.ToString(), message);
                writer.Close();
            }

            catch
            {

            }
        }
    }
}
