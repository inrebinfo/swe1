using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using EmbeddedSensorCloud;
using System.Data.SqlClient;

namespace CTemperature
{
    public class CTemperature : IPlugin
    {
        private string _pluginName = "TemperaturePlugin";
        private StreamWriter _writer;
        private CWebURL _url;
        private SqlConnection _SQLCon;

        public void Load(StreamWriter writer, CWebURL url)
        {
            Console.WriteLine(_pluginName + " loaded");
            _writer = writer;
            _url = url;

            _SQLCon = new SqlConnection();

            _SQLCon.ConnectionString = "Data Source=(local);" +
                       "Initial Catalog=EmbeddedSensorCloud;" +
                       "Integrated Security=sspi";
            _SQLCon.Open();
        }

        public void doSomething()
        {
            DateTime now = DateTime.Now;
            string today = now.ToString("dd-MM-yyyy");
            Console.WriteLine("Today is: " + today);

            Console.WriteLine(_pluginName + " did sosdmething");

            string command = "SELECT * FROM temperatures WHERE day = '" + today + "'";
            SqlCommand cmd = new SqlCommand(command, _SQLCon);
            
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("day: " + reader["day"] + " --- temp: " + reader["temp"]);
            }

            if (_url.WebParameters.Count > 0)
            {
                Console.WriteLine("parameters given");

                

                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>TemperaturePlugin</h1>
        <form method='post' action='http://localhost:8080/TemperaturePlugin.html'>
            Enter Day (dd-mm-yyyy)<input type='text' name='day'><br>
            <input type='submit'>
        </form>
        <br>
        <p>Parameters given!</p>
    </body>
</html>";

                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                string header = @"HTTP/1.1 200 OK
                    Server: EmbeddedSensorCloud Server
                    Content-Length: " + size + @"
                    Content-Language: de
                    Content-Type: text/html
                    Connection: close";

                CWebResponse response = new CWebResponse(_writer);
                response.WriteResponse(header, html);
            }
            else
            {
                Console.WriteLine("no parameters given");

                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>TemperaturePlugin</h1>
        <form method='post' action='http://localhost:8080/TemperaturePlugin.html'>
            Enter Day (dd-mm-yyyy)<input type='text' name='day'><br>
            <input type='submit'>
        </form>
        <br>
        <p>No Parameters given!</p>
    </body>
</html>";

                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                string header = @"HTTP/1.1 200 OK
                    Server: EmbeddedSensorCloud Server
                    Content-Length: " + size + @"
                    Content-Language: de
                    Content-Type: text/html
                    Connection: close";

                CWebResponse response = new CWebResponse(_writer);
                response.WriteResponse(header, html);
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
    }
}
