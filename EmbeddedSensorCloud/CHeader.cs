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

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        /// <summary>
        /// Erzeugt ein Header-Wertpaar
        /// </summary>
        /// <param name="Name">Name des Headers</param>
        /// <param name="Value">Wert des Headers</param>
        public CHeader(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name + ": " + _value + System.Environment.NewLine;
        }

    }
}
