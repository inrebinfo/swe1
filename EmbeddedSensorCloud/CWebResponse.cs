using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    class CWebResponse
    {
        private StreamWriter _ResponseWriter;
        private string _status;
        private int _contentLength;
        private string _contentType;

        public CWebResponse(StreamWriter writer)
        {
            _ResponseWriter = writer;
        }

        //public CWebResponse(StreamWriter writer, 

        public void WriteResponse()
        {
            string html = "<html><head><title>EmbeddedSensorCloud</title></head><body><h1>It works!</h1>" + DateTime.Now.ToString() + "<form method='post' action='http://localhost:1337/'><input type='text' name'param1'></form></body></html>";

            int size = ASCIIEncoding.ASCII.GetByteCount(html);

            //encapsulate response!
            _ResponseWriter.WriteLine("HTTP/1.1 200 OK");
            _ResponseWriter.WriteLine("Server: EmbeddedSensorCloud Server");
            _ResponseWriter.WriteLine("Content-Length: " + size);
            _ResponseWriter.WriteLine("Content-Language: de");
            _ResponseWriter.WriteLine("Content-Type: text/html");
            _ResponseWriter.WriteLine("Connection: close");
            _ResponseWriter.WriteLine("");

            //write file
            _ResponseWriter.WriteLine(html);
            _ResponseWriter.Flush();
        }
    }
}
