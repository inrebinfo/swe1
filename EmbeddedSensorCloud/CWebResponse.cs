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

        public void WriteResponse()
        {

        }
    }
}
