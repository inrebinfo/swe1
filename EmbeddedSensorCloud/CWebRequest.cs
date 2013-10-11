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
        private CWebURL _URLObject;

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
                        string[] requestParts = buffer.Split(' ');

                        string webUrl = requestParts[1].Substring(1);
                        
                        _URLObject = new CWebURL(webUrl);

                        Console.WriteLine(_URLObject.WebAddress);

                        try
                        {
                            Console.WriteLine(_URLObject.WebParameters["paddfdfram1"]);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        ParsePlugin();
                    }
                }
            }
        }

        private void ParsePlugin()
        {
            string plugin = _URLObject.WebAddress;
            plugin = plugin.Remove(_URLObject.WebAddress.Length - 5);
            Console.WriteLine("parseplugin: " + plugin);
        }
    }
}
