using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSensorCloud
{
    public interface IPlugin
    {
        string PluginName { get; }
        void Load();
        void doSomething();
        void Clean();
    }
}
