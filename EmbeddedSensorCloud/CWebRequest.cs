using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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
            string postURL = "";
            string postParameters = "";

            while ((buffer = _requestReader.ReadLine()) != "")
            {
                try
                {
                    if (buffer.StartsWith("GET"))
                    {
                        if (!buffer.Contains("favicon.ico"))
                        {
                            string[] requestParts = buffer.Split(' ');

                            string webUrl = requestParts[1].Substring(1);


                            string regex = @"^GetTemperature/\d{4}-\d{2}-\d{2}$";
                            RegexOptions options = RegexOptions.Multiline;

                            Match match = Regex.Match(webUrl, regex, options);

                            if (match.Success)
                            {
                                //gettemperature part
                                string[] parts = webUrl.Split('/');

                                if (parts.Length == 2)
                                {
                                    string param = "rest=" + parts[1];
                                    _URLObject = new CWebURL("TemperaturePlugin.html", param);
                                }

                                string plugin = _URLObject.WebAddress;
                                if (plugin != "")
                                {
                                    plugin = plugin.Remove(_URLObject.WebAddress.Length - 5);
                                    this._requestedPlugin = plugin;
                                }
                            }
                            else
                            {
                                string[] parts = webUrl.Split('?');

                                if (parts.Length > 1)
                                {
                                    _URLObject = new CWebURL(parts[0], parts[1]);
                                }
                                else
                                {
                                    _URLObject = new CWebURL(parts[0]);
                                }

                                string plugin = _URLObject.WebAddress;
                                if (plugin != "")
                                {
                                    plugin = plugin.Remove(_URLObject.WebAddress.Length - 5);
                                    this._requestedPlugin = plugin;
                                }
                            }
                        }
                    }
                    else if (buffer.StartsWith("POST"))
                    {
                        post = true;
                        string[] requestParts = buffer.Split(' ');

                        postURL = requestParts[1].Substring(1);
                    }
                    else if (buffer.StartsWith("Content-Length"))
                    {
                        string[] parts = buffer.Split(' ');
                        postLength = Convert.ToInt32(parts[1]);
                    }
                    Console.WriteLine(buffer);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            }

            if (post && postLength == 0)
            {
                _URLObject = new CWebURL(postURL);
            }
            else if (post && postLength > 0)
            {
                //POST Params auslesen
                var buf = new char[postLength];
                _requestReader.Read(buf, 0, postLength);
                string bodystr = new string(buf);

                postParameters = bodystr;
                _URLObject = new CWebURL(postURL, postParameters);


                string plugin = _URLObject.WebAddress;
                if (plugin != "")
                {
                    plugin = plugin.Remove(_URLObject.WebAddress.Length - 5);
                    this._requestedPlugin = plugin;
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

        public CWebURL URLObject
        {
            get
            {
                return this._URLObject;
            }
        }
    }
}
