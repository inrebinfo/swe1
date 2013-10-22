using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace EmbeddedSensorCloud
{
    class CWebRequest
    {
        private StreamReader _requestReader;
        private Socket _socket;
        private string _requestedURL;
        private CWebURL _URLObject;
        private string _requestedPlugin;

        public CWebRequest(StreamReader reader, Socket sock)
        {
            this._requestReader = reader;
            this._socket = sock;

            ParseRequest();
        }

        private void ParseRequest()
        {
            string buffer;
            int postLength = 0;
            bool post = false;
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
                else if (buffer.StartsWith("POST"))
                {
                    post = true;
                    string[] requestParts = buffer.Split(' ');

                    string webUrl = requestParts[1].Substring(1);
                }
                else if (buffer.StartsWith("Content-Length"))
                {
                    string[] parts = buffer.Split(' ');
                    postLength = Convert.ToInt32(parts[1]);
                }
                Console.WriteLine(buffer);
            }

            if (post && postLength > 0)
            {
                //POST Params auslesen
                var buf = new char[postLength];
                _requestReader.Read(buf, 0, postLength);
                string bodystr = new string(buf);
                Console.WriteLine(bodystr);
            }

        }

        public string RequestedPlugin
        {
            get
            {
                return this._requestedPlugin;
            }
        }

        public CWebURL URLObject
        {
            get
            {
                return this._URLObject;
            }
        }
    }
}
