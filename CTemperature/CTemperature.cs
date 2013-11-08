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

                Console.WriteLine("parameters given");

                foreach (KeyValuePair<string, string> entry in _url.WebParameters)
                {
                    if (entry.Key == "day")
                    {
                        #region normal request

                        string command = "";
                        string selectday = "";
                        string limit = "";

                        foreach (KeyValuePair<string, string> entry2 in _url.WebParameters)
                        {
                            if (entry2.Key == "day")
                            {
                                Console.WriteLine("value for day: " + entry2.Value);
                                selectday = entry2.Value;
                            }

                            if (entry2.Key == "limit")
                            {
                                if (entry2.Value != "")
                                {
                                    limit = "TOP " + entry2.Value;
                                }
                            }
                        }

                        command = "SELECT " + limit + " * FROM [temperatures] WHERE [day] = '" + selectday + "' ORDER BY [id] DESC";

                        SqlCommand cmd = new SqlCommand(command, _SQLCon);

                        string res = "";

                        try
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                string day = reader["day"].ToString();
                                string[] dayparts = day.Split(' ');
                                res += "<tr><td>" + reader["id"] + "</td><td>" + dayparts[0] + "</td><td>" + reader["temp"] + "</td><td><a href='/GetTemperature/" + selectday + "'>XML</a></td></tr>";
                                //Console.WriteLine("day: " + reader["day"] + " --- temp: " + reader["temp"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
            Last x Entries (all by default) <input type='text' name='limit'><br><small>Only works when day is entered!</small><br>
            <input type='submit'>
        </form>
        <br>
        <h1>All temperatures from " + selectday + @"</h1>
        <table border='1'>
            <tr><th>#</th><th>Day</th><th>Temperature</th><th>Get as XML</th></tr>" +
                    res +
                @"</table>
        <br>
        <p><a href='http://localhost:8080/'>Startseite</a></p>
    </body>
</html>";

                        int size = ASCIIEncoding.ASCII.GetByteCount(html);

                        CWebResponse response = new CWebResponse(_writer);
                        response.ContentLength = html.Length;
                        response.ContentType = "text/html";
                        response.WriteResponse(html);

                        #endregion
                    }
                    else if (entry.Key == "rest")
                    {
                        #region rest request

                        Console.WriteLine("rest request");

                        string command = "";
                        string selectday = "";

                        foreach (KeyValuePair<string, string> entry2 in _url.WebParameters)
                        {
                            if (entry2.Key == "rest")
                            {
                                Console.WriteLine("value for rest: " + entry2.Value);
                                selectday = entry2.Value;
                            }
                        }

                        command = "SELECT * FROM [temperatures] WHERE [day] = '" + selectday + "' ORDER BY [id] DESC";

                        SqlCommand cmd = new SqlCommand(command, _SQLCon);

                        string res = "";

                        try
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                string day = reader["day"].ToString();
                                string[] dayparts = day.Split(' ');
                                res += @"<entry><id>" + reader["id"] + "</id><temperature>" + reader["temp"] + "</temperature></entry>";
                                //Console.WriteLine("day: " + reader["day"] + " --- temp: " + reader["temp"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        string xml = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><temperatures day='" + selectday + @"'>" + res + "</temperatures>";

                        int size = ASCIIEncoding.ASCII.GetByteCount(xml);

                        CWebResponse response = new CWebResponse(_writer);
                        response.ContentLength = xml.Length;
                        response.ContentType = "text/xml";
                        response.WriteResponse(xml);

                        #endregion
                    }
                }
            }
            else
            {
                #region no parameters given

                Console.WriteLine("no parameters given");
                
                string command = "SELECT * FROM [temperatures] WHERE [day] = '" + today + "' ORDER BY [id] DESC";
                SqlCommand cmd = new SqlCommand(command, _SQLCon);

                string res = "";

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string day = reader["day"].ToString();
                        string[] dayparts = day.Split(' ');
                        res += "<tr><td>" + reader["id"] + "</td><td>" + dayparts[0] + "</td><td>" + reader["temp"] + "</td><td><a href='/GetTemperature/" + today + "'>XML</a></td></tr>";
                        //Console.WriteLine("day: " + reader["day"] + " --- temp: " + reader["temp"]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
            Last x Entries (all by default) <input type='text' name='limit'><br><small>Only works when day is entered!</small><br>
            <input type='submit'>
        </form>
        <br>
        <h1>All temperatures from today (" + today + @")</h1>
        <table border='1'>
            <tr><th>#</th><th>Day</th><th>Temperature</th><th>Get as XML</th></tr>" + 
            res +
        @"</table>
        <br>
        <p><a href='http://localhost:8080/'>Startseite</a></p>
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
