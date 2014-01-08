using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CStockTest
    {
        [TestMethod]
        public void CStock_successfully_sent()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("StockPlugin.html", "symbol=MSFT");

            CStock stockPlugin = new CStock();

            stockPlugin.Load(writer, urlobject);
            stockPlugin.doSomething();

            Assert.AreEqual(true, stockPlugin.IsSent);
        }

        [TestMethod]
        public void CStock_site_successfully_opened()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("StockPlugin.html", "symbol=MSFT");

            CStock stockPlugin = new CStock();

            stockPlugin.Load(writer, urlobject);
            stockPlugin.doSomething();

            Assert.AreEqual(true, stockPlugin.IsSent);
        }

        [TestMethod]
        public void CStock_Stocknodes_filled()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("StockPlugin.html", "symbol=MSFT");

            CStock stockPlugin = new CStock();

            stockPlugin.Load(writer, urlobject);
            stockPlugin.doSomething();

            int count = 0;

            foreach (XNode node in stockPlugin.Nodes)
            {
                if (node is XElement)
                {
                    count++;
                }
            }

            Assert.IsNotNull(count);
        }

        [TestMethod]
        public void CStock_No_Parameters_given()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("TemperaturePlugin.html");

            CStock stockPlugin = new CStock();

            stockPlugin.Load(writer, urlobject);
            stockPlugin.doSomething();

            Assert.AreEqual(true, stockPlugin.NoParams);
        }
    }
}
