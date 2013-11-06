using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using EmbeddedSensorCloud;

namespace CNavi
{
    public class CNavi : IPlugin
    {
        private string _pluginName = "NaviPlugin";
        public void Load(StreamWriter writer, CWebURL url)
        {
            Console.WriteLine(_pluginName + " loaded");
        }

        public void doSomething()
        {
            Console.WriteLine(_pluginName + " did something");
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
