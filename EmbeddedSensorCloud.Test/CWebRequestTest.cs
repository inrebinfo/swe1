using System;
using System.IO;
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
        public void CWebRequest_ParseRequest_Correct_Pluginname_parsed()
        {
            string _pluginname = "TestPlugin";
            string _header = @"GET /TestPlugin.html HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;
            //Socket _sock;

            // convert string to stream
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            // convert stream to string
            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(_pluginname, _WebReq.RequestedPlugin, "message");
        }
    }
}
