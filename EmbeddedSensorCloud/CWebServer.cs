using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EmbeddedSensorCloud
{
    class CWebServer
    {

        private bool _isRunning;
        private ArrayList _loadedPlugins;

        public void Start()
        {
            new Thread(RunServer).Start();
            _isRunning = true;
        }

        public void RunServer()
        {
            Console.WriteLine("Server now listening on port 8080\n");
            //start listener on port 1337
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            //loop for server instances
            while (true)
            {
                //if we get an request accept socket and create new thread for the instance
                Socket SocketForClient = listener.AcceptSocket();
                if (SocketForClient.Connected)
                {
                    ParameterizedThreadStart ThreadsForClient = new ParameterizedThreadStart(NewConnection);
                    Thread ThreadForNewClient = new Thread(NewConnection);
                    ThreadForNewClient.Start(SocketForClient);
                }
            }
        }

        public void NewConnection(object SessionClient)
        {
            Socket sClient = (Socket)SessionClient;

            //networkstream for the data between the client and us
            NetworkStream StreamFromClient = new NetworkStream(sClient);

            //we need to write something back!
            StreamWriter WriterForClient = new StreamWriter(StreamFromClient);

            //we also need to read something
            StreamReader ReaderForClient = new StreamReader(StreamFromClient);

            CWebRequest WebRequest = new CWebRequest(ReaderForClient, sClient);
            
            CWebURL url = WebRequest.URLObject;

            CPluginManager PluginManager = new CPluginManager();
            _loadedPlugins = PluginManager.LoadPlugins("/plugins/", "*.dll", typeof(EmbeddedSensorCloud.IPlugin));

            int counter = 0;

            foreach (IPlugin plug in _loadedPlugins)
            {
                if (plug.PluginName == WebRequest.RequestedPlugin)
                {
                    counter++;
                    Console.WriteLine("requested plugin: " + plug.PluginName);

                    plug.Load(WriterForClient, url);
                    plug.doSomething();
                }
            }

            Console.WriteLine();


            //CREATE RESPONSE
            if (counter == 0)
            {
                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>EmbeddedSensorCloud</h1>
        <p><a href='TemperaturePlugin.html'>Temperature Plugin</a></p>
        <p><a href='StaticPlugin.html'>Static Plugin</a></p>
        <p><a href='NaviPlugin.html'>Navi Plugin</a></p>
    </body>
</html>";

                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                string header = @"HTTP/1.1 200 OK
                    Server: EmbeddedSensorCloud Server
                    Content-Length: " + size + @"
                    Content-Language: de
                    Content-Type: text/html
                    Connection: close";

                CWebResponse response = new CWebResponse(WriterForClient);
                response.WriteResponse(header, html);
            }

            //close all writers and readers
            StreamFromClient.Close();
            WriterForClient.Close();
            ReaderForClient.Close();
            sClient.Close();
        }

        public bool isRunning
        {
            get { return _isRunning; }
            set { this._isRunning = value; }
        }
    }
}
