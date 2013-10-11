using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EmbeddedSensorCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            CWebServer newWebServer = new CWebServer();
            newWebServer.Start();

            while (newWebServer.isRunning)
            {
                string exitString = Console.ReadLine();

                if (exitString == "quit")
                {
                    Environment.Exit(1);
                }
            }
        }
    }
}
