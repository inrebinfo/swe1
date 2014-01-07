using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CWebServerTest
    {
        [TestMethod]
        public void CWebserver_IsRunning()
        {
            CWebServer server = new CWebServer();
            server.Start();

            Assert.AreEqual(true, server.isRunning);
        }
        
        [TestMethod]
        public void CWebserver_IsNotRunning()
        {
            CWebServer server = new CWebServer();
            
            Assert.AreEqual(false, server.isRunning);
        }
    }
}
