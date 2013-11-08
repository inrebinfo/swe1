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

            _SQLCon = new SqlConnection(
                @"Data Source=.\SqlExpress;
                Initial Catalog=EmbeddedSensorCloud;
                Integrated Security=true;");

            _SQLCon.Open();
        }

        public void doSomething()
        {
            DateTime now = DateTime.Now;
            string today = now.ToString("dd-MM-yyyy");
            Console.WriteLine("Today is: " + today);

            Console.WriteLine(_pluginName + " did something");
            
            if (_url.WebParameters.Count > 0)
            {
                #region parameters given

                Console.WriteLine("parameters given");

                string command = "";
                string selectday = "";
                string limit = "";

                foreach (KeyValuePair<string, string> entry in _url.WebParameters)
                {
                    if (entry.Key == "day")
                    {
                        Console.WriteLine("value for day: " + entry.Value);
                        selectday = entry.Value;
                    }

                    if (entry.Key == "limit")
                    {
                        if (entry.Value != "")
                        {
                            limit = "TOP " + entry.Value;
                        }
                    }
                }

                command = "SELECT " + limit + " * FROM [temperatures] WHERE [day] = '" + selectday + "' ORDER BY [id] DESC";
                
                SqlCommand cmd = new SqlCommand(command, _SQLCon);

                string res = "";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string day = reader["day"].ToString();
                    string[] dayparts = day.Split(' ');
                    res += "<tr><td>" + reader["id"] + "</td><td>" + dayparts[0] + "</td><td>" + reader["temp"] + "</td></tr>";
                    //Console.WriteLine("day: " + reader["day"] + " --- temp: " + reader["temp"]);
                }

                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>TemperaturePlugin</h1>
        <form method='post' action='http://localhost:8080/TemperaturePlugin.html'>
            Enter Day (yyyy-mm-dd) <input type='text' name='day'><br>
            Last x Entries (all by default) <input type='text' name='limit'><br>
            <input type='submit'>
        </form>
        <br>
        <h1>All temperatures from " + selectday + @"</h1>
        <table border='1'>
            <tr><th>#</th><th>Day</th><th>Temperature</th></tr>" +
            res +
        @"</table>
    </body>
</html>";

                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                CWebResponse response = new CWebResponse(_writer);
                response.ContentLength = html.Length;
                response.ContentType = "text/html";
                response.WriteResponse(html);

                #endregion
            }
            else
            {
                #region no parameters given

                Console.WriteLine("no parameters given");
                
                string command = "SELECT * FROM [temperatures] WHERE [day] = '" + today + "' ORDER BY [id] DESC";
                SqlCommand cmd = new SqlCommand(command, _SQLCon);

                string res = "";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string day = reader["day"].ToString();
                    string[] dayparts = day.Split(' ');
                    res += "<tr><td>" + reader["id"] + "</td><td>" + dayparts[0] + "</td><td>" + reader["temp"] + "</td></tr>";
                    //Console.WriteLine("day: " + reader["day"] + " --- temp: " + reader["temp"]);
                }

                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>TemperaturePlugin</h1>
        <form method='post' action='http://localhost:8080/TemperaturePlugin.html'>
            Enter Day (yyyy-mm-dd) <input type='text' name='day'><br>
            Last x Entries (all by default) <input type='text' name='limit'><br>
            <input type='submit'>
        </form>
        <br>
        <h1>All temperatures from today (" + today + @")</h1>
        <table border='1'>
            <tr><th>#</th><th>Day</th><th>Temperature</th></tr>" + 
            res +   
        @"</table>
    </body>
</html>";

                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                CWebResponse response = new CWebResponse(_writer);
                response.ContentLength = html.Length;
                response.ContentType = "text/html";
                response.WriteResponse(html);

                #endregion
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
