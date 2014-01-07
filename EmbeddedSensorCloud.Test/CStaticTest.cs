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
    public class CStaticTest
    {
        [TestMethod]
        public void CStatic_png_correctly_parsed()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("StaticPlugin.html", "file=someImage.png");

            CStatic staticPlugin = new CStatic();

            staticPlugin.Load(writer, urlobject);
            staticPlugin.doSomething();

            Assert.AreEqual("png", staticPlugin.FileExtension);
        }

        [TestMethod]
        public void CStatic_png_false_parsed()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("StaticPlugin.html", "file=someImage.exe");

            CStatic staticPlugin = new CStatic();

            staticPlugin.Load(writer, urlobject);
            staticPlugin.doSomething();

            Assert.AreNotEqual("png", staticPlugin.FileExtension);
        }
    }
}
