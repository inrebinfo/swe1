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
        private string _requestedPlugin;

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

                        Console.WriteLine("requested file " + _URLObject.WebAddress);

                        string plugin = _URLObject.WebAddress;
                        if (plugin != "")
                        {
                            plugin = plugin.Remove(_URLObject.WebAddress.Length - 5);
                            this._requestedPlugin = plugin;
                        }
                    }
                }
            }
        }

        public string RequestedPlugin
        {
            get
            {
                return this._requestedPlugin;
            }
        }
    }
}
