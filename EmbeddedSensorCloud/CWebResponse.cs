using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    public class CWebResponse
    {
        private StreamWriter _ResponseWriter;
        private string _status;
        private int _contentLength;
        private string _contentType;
        private string _contentDisposition;
        private List<CHeader> _headers;

        

        public CWebResponse(StreamWriter writer)
        {
            _headers = new List<CHeader>();
            _headers.Add(new CHeader("Server", "EmbeddedSensorCloud Server"));
            _headers.Add(new CHeader("Content-Language", "de"));

            _ResponseWriter = writer;
        }

        public void WriteResponse(string responseString)
        {
            WriteHeader();
            _ResponseWriter.Write(responseString);
            _ResponseWriter.Flush();
        }

        public void WriteResponse(byte[] responseArray)
        {
            WriteHeader();
            _ResponseWriter.BaseStream.Write(responseArray, 0, responseArray.Length);
            _ResponseWriter.Flush();
        }

        private void WriteHeader()
        {
            _ResponseWriter.Write("HTTP/1.1 200 OK" + System.Environment.NewLine);
            foreach (CHeader h in _headers)
            {
                _ResponseWriter.Write(h.ToString());
            }
            _ResponseWriter.Write(System.Environment.NewLine);
            _ResponseWriter.Flush();
        }

        public int ContentLength
        {
            get { return _contentLength; }
            set
            {
                _contentLength = value;
                CHeader toDelete = (from s in _headers
                                    where s.Name == "Content-Length"
                                    select s).FirstOrDefault();
                _headers.Remove(toDelete);
                _headers.Add(new CHeader("Content-Length", value.ToString()));
            }
        }

        public string ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                CHeader toDelete = (from s in _headers
                                    where s.Name == "Content-Type"
                                    select s).FirstOrDefault();
                _headers.Remove(toDelete);
                _headers.Add(new CHeader("Content-Type", value));
            }
        }

        public string ContentDisposition
        {
            get { return _contentDisposition; }
            set
            {
                _contentDisposition = value;
                CHeader toDelete = (from s in _headers
                                    where s.Name == "Content-disposition: attachment; filename="
                                    select s).FirstOrDefault();
                _headers.Remove(toDelete);
                _headers.Add(new CHeader("Content-disposition: attachment; filename=", value.ToString()));
            }
        }
    }
}
