using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    class CHeader
    {
        private string _name;
        private string _value;
        
        public CHeader(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name + ": " + _value + System.Environment.NewLine;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
}
