using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EmbeddedSensorCloud;
using System.Drawing;
using System.Drawing.Imaging;

namespace CStatic
{
    public class CStatic : IPlugin
    {
        private string _pluginName = "StaticPlugin";
        private StreamWriter _writer;
        private CWebURL _url;

        public void Load(StreamWriter writer, CWebURL url)
        {
            Console.WriteLine(_pluginName + " loaded");
            _writer = writer;
            _url = url;
        }

        public void doSomething()
        {
            Console.WriteLine(_pluginName + " did something");

            /*MemoryStream memory = new MemoryStream();
            Image image = Image.FromFile("myimage.jpg");
            ImageFormat format = ImageFormat.Jpeg;
            image.Save( memory, format );
            byte[] bild = memory.ToArray();*/

            byte[] bild;
            FileStream fstream = new FileStream("myimage.jpg", FileMode.Open, FileAccess.Read);

            bild = new byte[(long)fstream.Length];

            fstream.Read(bild, 0, Convert.ToInt32(fstream.Length));
            
            CWebResponse response = new CWebResponse(_writer);
            response.ContentLength = bild.Length;
            response.ContentDisposition = "myimage.jpg";
            response.ContentType = "image/jpeg";
            response.WriteResponse("", bild);
            //memory.Close();
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
