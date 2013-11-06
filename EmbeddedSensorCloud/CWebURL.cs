using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    public class CWebURL
    {
        private string _webAddress;
        private Dictionary<string, string> _webParameters = new Dictionary<string, string>();

        public CWebURL(string address)
        {
            this._webAddress = address;
        }

        public CWebURL(string address, string parameters)
        {
            this._webAddress = address;
            createParameters(parameters);
        }

        private void createParameters(string parameters)
        {
            string[] pairs = parameters.Split('&');
            foreach(string pair in pairs)
            {
                string[] parts = pair.Split('=');
                _webParameters.Add(parts[0].ToString(), parts[1].ToString());
            }
        }

        public string WebAddress
        {
            get
            {
                return this._webAddress;
            }
        }

        public Dictionary<string, string> WebParameters
        {
            get
            {
                return this._webParameters;
            }
        }
    }
}
