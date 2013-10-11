using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    class CWebRequest
    {
        private StreamReader _requestReader;
        private string _requestedURL;

        //GET /favicon.ico <-- ignore
        //url split bei ? für file & parameter
        //parameterteil split bei & für parameter-value-paar
        //parameter split bei = für parametername und value

        public CWebRequest(StreamReader reader)
        {
            this._requestReader = reader;
            ParseRequest();
        }
        
        private void ParseRequest()
        {
            string buffer;
            while ((buffer = _requestReader.ReadLine()) != "")
            {
                if (buffer.StartsWith("GET"))
                {
                    if (!buffer.Contains("favicon.ico"))
                    {
                        Console.WriteLine(buffer);
                        string[] requestParts = buffer.Split(' ');
                        foreach (string part in requestParts)
                        {
                            Console.WriteLine(part);
                        }
                    }
                }
                //Console.WriteLine(buffer);
            }
        }
    }
}
