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
            Console.WriteLine("Server now listening on port 1337\n");
            //start listener on port 1337
            TcpListener listener = new TcpListener(IPAddress.Any, 1337);
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

            CWebRequest WebRequest = new CWebRequest(ReaderForClient);
            CPluginManager PluginManager = new CPluginManager();
            _loadedPlugins = PluginManager.LoadPlugins(@"\plugins\", "*.dll", typeof(EmbeddedSensorCloud.IPlugin));

            foreach (IPlugin plug in _loadedPlugins)
            {
                if (plug.PluginName == WebRequest.RequestedPlugin)
                {
                    Console.WriteLine("requested plugin: " + plug.PluginName);
                }
            }


            try
            {
                CWebURL url = WebRequest.URLObject;
                Console.WriteLine("file: " + url.WebAddress);
                foreach (KeyValuePair<string, string> entry in url.WebParameters)
                {
                    Console.WriteLine("key, value:" + entry.Key + ", " + entry.Value);
                }
            }
            catch { }

            //DO HERE WHAT THE PLUGIN HAS TO DO!

            Console.WriteLine();


            //CREATE RESPONSE
            CWebResponse response = new CWebResponse(WriterForClient);
            response.WriteResponse();

            WriterForClient.Flush();

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
