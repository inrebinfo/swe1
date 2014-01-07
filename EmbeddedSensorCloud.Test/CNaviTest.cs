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
    public class CNaviTest
    {
        [TestMethod]
        public void CNavi_is_not_locked_on_search()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("NaviPlugin.html", "mode=search");

            CNavi naviPlugin = new CNavi();

            naviPlugin.Load(writer, urlobject);
            naviPlugin.doSomething();

            Assert.AreEqual(false, naviPlugin.IsLocked);
        }

        [TestMethod]
        public void CNavi_is_locked_on_prepare()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("NaviPlugin.html", "mode=prepare");

            CNavi naviPlugin = new CNavi();

            naviPlugin.Load(writer, urlobject);
            naviPlugin.doSomething();

            Assert.AreEqual(true, naviPlugin.IsLocked);
        }
    }
}
