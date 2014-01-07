using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CTemperatureTest
    {
        [TestMethod]
        public void CTemperature_REST_successfully_generated()
        {
            // http://localhost:8080/GetTemperature/2014-01-07
            string check = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>";

            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("TemperaturePlugin.html", "rest=2014-01-07");

            CTemperature tempPlugin = new CTemperature();

            tempPlugin.Load(writer, urlobject);
            tempPlugin.doSomething();

            bool startswith = tempPlugin.XML.StartsWith(check);

            Assert.AreEqual(true, startswith);
        }
    }
}
