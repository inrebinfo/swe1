using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EmbeddedSensorCloud;

namespace EmbeddedSensorCloud
{
    public class CStatic : IPlugin
    {
        private string _pluginName = "StaticPlugin";
        private StreamWriter _writer;
        private CWebURL _url;

        private string _file;
        private bool _noParams = false;

        public void Load(StreamWriter writer, CWebURL url)
        {
            Console.WriteLine(_pluginName + " loaded");
            _writer = writer;
            _url = url;
        }

        public void doSomething()
        {
            Console.WriteLine(_pluginName + " did something");

            if (_url.WebParameters.Count > 0)
            {
                try
                {
                    #region filename

                    string filename = "";

                    foreach (KeyValuePair<string, string> entry in _url.WebParameters)
                    {
                        if (entry.Key == "file")
                        {
                            filename = entry.Value;
                            _file = filename;
                        }
                    }

                    byte[] file;
                    FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

                    file = new byte[fileStream.Length];

                    fileStream.Read(file, 0, Convert.ToInt32(fileStream.Length));

                    CWebResponse response = new CWebResponse(_writer);
                    response.ContentLength = file.Length;

                    string[] fileparts = filename.Split('.');

                    if (fileparts[fileparts.Length - 1] == "jpeg" || fileparts[fileparts.Length - 1] == "jpg")
                    {
                        //jpeg
                        response.ContentType = "image/jpeg";
                    }
                    else if (fileparts[fileparts.Length - 1] == "png")
                    {
                        //png
                        response.ContentType = "image/png";
                    }
                    else if (fileparts[fileparts.Length - 1] == "gif")
                    {
                        //gif
                        response.ContentType = "image/gif";
                    }
                    else if (fileparts[fileparts.Length - 1] == "html" || fileparts[fileparts.Length - 1] == "htm" || fileparts[fileparts.Length - 1] == "xhtml")
                    {
                        //html
                        response.ContentType = "text/html";
                    }
                    else if (fileparts[fileparts.Length - 1] == "xml")
                    {
                        //xml
                        response.ContentType = "text/xml";
                    }
                    else if (fileparts[fileparts.Length - 1] == "txt" || fileparts[fileparts.Length - 1] == "ini" || fileparts[fileparts.Length - 1] == "config")
                    {
                        //rawtext
                        response.ContentType = "text/plain";
                    }
                    else
                    {
                        //octet-stream
                        response.ContentDisposition = filename;
                        response.ContentType = "application/octet-stream";
                    }

                    response.WriteResponse(file);
                    //memory.Close();

                    #endregion
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    #region no params

                string[] filePaths = Directory.GetFiles(".");

                string files = "";
                _noParams = true;

                foreach (string s in filePaths)
                {
                    files += "<tr><td><a href='?file=" + s.Substring(2) + "'>" + s.Substring(2) + "</a></td></tr>";
                }

                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>StaticPlugin</h1>
        <h1>Choose your file</h1>
        <p>html, xml, plaintext, png, jpeg and gif are supported in browser, others will get you a download</p>
        <table border='1'>" +
            files +
        @"</table>
        <br>
        <p><a href='http://localhost:8080/'>Startseite</a></p>
    </body>
</html>";

                CWebResponse response = new CWebResponse(_writer);
                response.ContentLength = html.Length;
                response.ContentType = "text/html";
                response.WriteResponse(html);

                    #endregion
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Clean()
        {
            Console.WriteLine("cleaned " + _pluginName);
        }

        public string PluginName
        {
            get
            {
                return this._pluginName;
            }
        }

        public string FileExtension
        {
            get
            {
                string[] arr1 = _file.Split('\\');
                string[] realfile = arr1[arr1.Length - 1].Split('.');

                return realfile[1];
            }
        }

        public bool NoParams
        {
            get
            {
                return _noParams;
            }
        }
    }
}
