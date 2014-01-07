using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    public interface IPlugin
    {
        string PluginName { get; }
        void Load(StreamWriter writer, CWebURL url);
        void doSomething();
        void Clean();
    }
}
