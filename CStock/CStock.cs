using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using EmbeddedSensorCloud;

namespace EmbeddedSensorCloud
{
    public class CStock : IPlugin
    {
        private string _pluginName = "StockPlugin";
        private StreamWriter _writer;
        private CWebURL _url;
        private IEnumerable<XNode> _stocknodes;
        private bool _isSent = false;
        private bool _siteOpen = false;
        private bool _noParams = false;

        public void Load(StreamWriter writer, CWebURL url)
        {
            Console.WriteLine(_pluginName + " loaded");
            _writer = writer;
            _url = url;
        }

        public void doSomething()
        {
            if (_url.WebParameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in _url.WebParameters)
                {
                    if (entry.Key == "symbol")
                    {
                        try
                        {
                            #region symbol

                            string msg = "<table border=1><tr><th>Key</th><th>Value</th></tr>";
                            string file = @"";

                            using (WebClient client = new WebClient())
                            {
                                try
                                {
                                    string htmlCode = client.DownloadString("http://www.webservicex.net/stockquote.asmx/GetQuote?symbol=" + entry.Value);
                                    file = htmlCode;
                                    _siteOpen = true;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            file = file.Replace("\"", "'");
                            file = file.Replace("&lt;", "<");
                            file = file.Replace("&gt;", ">");
                            file = file.Replace("<string xmlns='http://www.webserviceX.NET/'>", "");
                            file = file.Replace("</string>", "");
                            file = file.Replace("<StockQuotes>", "");
                            file = file.Replace("</StockQuotes>", "");
                            file = file.Replace("><", ">" + System.Environment.NewLine + "<");

                            XDocument doc = XDocument.Parse(file);
                            _stocknodes = from s in doc.DescendantNodes()
                                          select s;

                            foreach (XNode node in _stocknodes)
                            {
                                if (node is XElement)
                                {
                                    XElement ele = (XElement)node;
                                    if (ele.Name != "Stock")
                                    {
                                        msg += "<tr><td>" + ele.Name + "</td>";
                                        Console.WriteLine((ele as XElement).Name);
                                    }
                                }
                                else
                                {
                                    msg += "<td>" + node + "</td></tr>";
                                    Console.WriteLine(node);
                                }
                            }

                            msg += "</table>";

                            string html = @"<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>StockPlugin</h1>
        " + msg + @"
        <form method='post' action='http://localhost:8080/StockPlugin.html'>
            Enter Stock Symbol (for example: MSFT for Microsoft) <input type='text' name='symbol'><br>
            <input type='submit'>
        </form>
        <p><a href='http://localhost:8080/'>Startseite</a></p>
    </body>
</html>";

                            int size = ASCIIEncoding.ASCII.GetByteCount(html);

                            CWebResponse response = new CWebResponse(_writer);
                            response.ContentLength = html.Length;
                            response.ContentType = "text/html";
                            response.WriteResponse(html);

                            _isSent = true;

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            _isSent = false;
                        }
                     }
                }
            }
            else
            {
                try
                {
                    #region no params form
                _noParams = true;
                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>StockPlugin</h1>
        <form method='post' action='http://localhost:8080/StockPlugin.html'>
            Enter Stock Symbol (for example: MSFT for Microsoft) <input type='text' name='symbol'><br>
            <input type='submit'>
        </form>
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

        private string Unescape(string xml)
        {
            var output = Regex.Replace(xml, @"\\[rnt\]", m =>
            {
                switch (m.Value)
                {
                    case @"\r": return "\r";
                    case @"\n": return "\n";
                    case @"\t": return "\t";
                    default: return m.Value;
                }
            });

            return output;
        }

        public IEnumerable<XNode> Nodes
        {
            get
            {
                return _stocknodes;
            }
        }

        public bool IsSent
        {
            get
            {
                return _isSent;
            }
        }

        public bool SiteOpen
        {
            get
            {
                return _siteOpen;
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
