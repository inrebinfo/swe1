using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using EmbeddedSensorCloud;

namespace EmbeddedSensorCloud
{
    public class CNavi : IPlugin
    {
        private string _pluginName = "NaviPlugin";
        private StreamWriter _writer;
        private CWebURL _url;
        private static Dictionary<string, List<string>> _Locations = new Dictionary<string, List<string>>();
        private static bool _locked = false;
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
                    if (entry.Key == "mode")
                    {
                        
                        if (entry.Value == "prepare")
                        {
                            /*try
                            {*/
                                #region prepare
                                string msg = "";
                                if (_locked == false)
                                {
                                    _locked = true;

                                    Thread thread = new Thread(PrepareData);
                                    thread.Start();

                                    msg = "Please wait until data is completelyup to date.";

                                }
                                else
                                {
                                    msg = "The data is to be updated. Please try again later.";
                                }

                                //send response
                                string html = @"<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>NaviPlugin</h1>
        <p>" + msg + @"</p>  
        <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
    </body>
</html>";
                                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                                CWebResponse response = new CWebResponse(_writer);
                                response.ContentLength = html.Length;
                                response.ContentType = "text/html";
                                response.WriteResponse(html);
                                #endregion
                            /*}
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }*/
                        }
                        else if (entry.Value == "search")
                        {
                            try
                            {
                                #region searchform
                                if (_locked == true)
                                {   
                                    string html = @"<html>
        <head>
            <title>EmbeddedSensorCloud</title>
        </head>
        <body>
            <h1>NaviPlugin</h1>
            <p>The data is to be updated. Please try again later.</p>
            <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
        </body>
    </html>";

                                    int size = ASCIIEncoding.ASCII.GetByteCount(html);

                                    CWebResponse response = new CWebResponse(_writer);
                                    response.ContentLength = html.Length;
                                    response.ContentType = "text/html";
                                    response.WriteResponse(html);
                                }
                                else
                                {
                                    string html = @"<html>
        <head>
            <title>EmbeddedSensorCloud</title>
        </head>
        <body>
            <h1>NaviPlugin</h1>
            <form method='post' action='http://localhost:8080/NaviPlugin.html'>
                Enter Streetname <input type='text' name='street' value='Barrett Lane'><br>
                <input type='submit'>
            </form>
            <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
        </body>
    </html>";

                                    int size = ASCIIEncoding.ASCII.GetByteCount(html);

                                    CWebResponse response = new CWebResponse(_writer);
                                    response.ContentLength = html.Length;
                                    response.ContentType = "text/html";
                                    response.WriteResponse(html);
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else if (entry.Key == "street")
                    {
                        try
                        {
                            #region street
                            if (_locked == true)
                            {
                                string html = @"<html>
        <head>
            <title>EmbeddedSensorCloud</title>
        </head>
        <body>
            <h1>NaviPlugin</h1>
            <p>The data is to be updated. Please try again later.</p>
            <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
        </body>
    </html>";

                                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                                CWebResponse response = new CWebResponse(_writer);
                                response.ContentLength = html.Length;
                                response.ContentType = "text/html";
                                response.WriteResponse(html);
                            }
                            else
                            {
                                string DecodedParam = HttpUtility.UrlDecode(entry.Value.Replace("+", " "));

                                string msg = "";

                                foreach (KeyValuePair<string, List<string>> location in _Locations)
                                {
                                    try
                                    {
                                        if (location.Key == DecodedParam)
                                        {
                                            int counter = location.Value.Count;

                                            msg += "<table><tr><th>City</th><th>Street</th></tr>";

                                            for (int i = 0; i < counter; i++)
                                            {
                                                msg += "<tr><td>" + location.Value[i] + "</td><td>" + location.Key + "</td></tr>";
                                            }

                                            msg += "</table>";
                                        }
                                    }
                                    catch
                                    {
                                        msg = "<p>No results.</p>";
                                    }
                                }

                                string html = @"<html>
        <head>
            <title>EmbeddedSensorCloud</title>
        </head>
        <body>
            <h1>NaviPlugin</h1>
            " + msg + @"
            <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
        </body>
    </html>";

                                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                                CWebResponse response = new CWebResponse(_writer);
                                response.ContentLength = html.Length;
                                response.ContentType = "text/html";
                                response.WriteResponse(html);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    #region no parmaters

                _noParams = true;

                if (_locked == true)
                {
                    string html = @"<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>NaviPlugin</h1>
        <p>The data is to be updated. Please try again later.</p>
        <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
    </body>
</html>";

                    int size = ASCIIEncoding.ASCII.GetByteCount(html);

                    CWebResponse response = new CWebResponse(_writer);
                    response.ContentLength = html.Length;
                    response.ContentType = "text/html";
                    response.WriteResponse(html);
                }
                else
                {
                    string html = @"<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>NaviPlugin</h1>
        <ul>
            <li><a href='NaviPlugin.html?mode=prepare'>Stra&szlig;enkarte neu aufbereiten</a></li>
            <li><a href='NaviPlugin.html?mode=search'>Stra&szlig;en <--> Orte</a></li>
        </ul>    
        <p><a href='http://localhost:8080/'>Startseite</a></p><p><a href='http://localhost:8080/NaviPlugin.html'>Go Back</a></p>
    </body>
</html>";

                    int size = ASCIIEncoding.ASCII.GetByteCount(html);

                    CWebResponse response = new CWebResponse(_writer);
                    response.ContentLength = html.Length;
                    response.ContentType = "text/html";
                    response.WriteResponse(html);
                }
                #endregion
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private void PrepareData()
        {
            DateTime date1 = DateTime.Now;
            Console.WriteLine(date1.ToString());

            string path = @"C:\Users\Zexion\Desktop\austria-latest.osm\austria-latest.osm";
            Console.WriteLine(path);

            using (var fs = File.OpenRead(path))
            {
                using (var xml = new XmlTextReader(fs))
                {
                    while (xml.Read())
                    {
                        if (xml.NodeType == System.Xml.XmlNodeType.Element && xml.Name == "osm")
                        {
                            ReadOsm(xml);
                        }
                    }
                }
            }

            DateTime date2 = DateTime.Now;
            Console.WriteLine(date2.ToString());
            Console.WriteLine(date2 - date1);
            Console.WriteLine("---reading file complete---");

            _locked = false;

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

        private void ReadOsm(System.Xml.XmlTextReader xml)
        {
            using (var osm = xml.ReadSubtree())
            {
                while (osm.Read())
                {
                    if (osm.NodeType == System.Xml.XmlNodeType.Element && (osm.Name == "node" || osm.Name == "way"))
                    {
                        ReadAnyOsmElement(osm);
                    }
                }
            }
        }

        private void ReadAnyOsmElement(System.Xml.XmlReader osm)
        {
            Address a = new Address();
            string street = null;
            string city = null;

            using (var element = osm.ReadSubtree())
            {
                while (element.Read())
                {
                    if (element.NodeType == System.Xml.XmlNodeType.Element && element.Name == "tag")
                    {
                        ReadTag(element, a, city, street);
                    }
                }
            }
        }

        private void ReadTag(System.Xml.XmlReader element, Address a, string city, string street)
        {

            string tagType = element.GetAttribute("k");
            string value = element.GetAttribute("v");
            switch (tagType)
            {
                case "addr:city":
                    a.City = value;
                    break;

                case "addr:street":
                    a.Street = value;
                    break;
            }

            if (a.Street != null & a.City != null)
            {

                if (_Locations.ContainsKey(a.Street))
                {
                    if (!(_Locations[a.Street].Contains(a.City)))
                    {
                        _Locations[a.Street].Add(a.City);
                    }
                }
                else
                {
                    _Locations.Add(a.Street, new List<string>() { a.City });
                }
            }
        }

        public bool IsLocked
        {
            get
            {
                return _locked;
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

    public class Address
    {
        private string _City;
        private string _Street;


        public string City
        {
            set
            {
                _City = value;
            }

            get
            {
                return _City;
            }
        }

        public string Street
        {
            set
            {
                _Street = value;
            }

            get
            {
                return _Street;
            }
        }

    }
}
