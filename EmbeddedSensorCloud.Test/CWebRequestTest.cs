using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using EmbeddedSensorCloud;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CWebRequestTest
    {
        [TestMethod]
        public void CWebRequest_ParseRequest_TemperaturPlugin_correct_Parsed()
        {
            string _pluginname = "TemperaturPlugin";
            string _header = @"GET /TemperaturPlugin.html HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;


            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(_pluginname, _WebReq.RequestedPlugin);
        }

        [TestMethod]
        public void CWebRequest_ParseRequest_StaticPlugin_correct_Parsed()
        {
            string _pluginname = "StaticPlugin";
            string _header = @"GET /StaticPlugin.html HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;


            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(_pluginname, _WebReq.RequestedPlugin);
        }

        [TestMethod]
        public void CWebRequest_ParseRequest_NaviPlugin_correct_Parsed()
        {
            string _pluginname = "NaviPlugin";
            string _header = @"GET /NaviPlugin.html HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;


            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(_pluginname, _WebReq.RequestedPlugin);
        }

        [TestMethod]
        public void CWebRequest_ParseRequest_StockPlugin_correct_Parsed()
        {
            string _pluginname = "StockPlugin";
            string _header = @"GET /StockPlugin.html HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;


            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(_pluginname, _WebReq.RequestedPlugin);
        }

        [TestMethod]
        public void CWebRequest_ParseRequest_REST_Parameter_correct_Parsed()
        {
            string _header = @"GET /GetTemperature/2014-01-07 HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("rest", "2014-01-07");

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(dict["rest"], "2014-01-07");
        }
    }
}
