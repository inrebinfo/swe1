using System;
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

            //sexy request message
            /*Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Request from " + sClient.LocalEndPoint + " in Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());
            Console.ResetColor();*/

            //networkstream for the data between the client and us
            NetworkStream StreamFromClient = new NetworkStream(sClient);

            //we need to write something back!
            StreamWriter WriterForClient = new StreamWriter(StreamFromClient);

            //we also need to read something
            StreamReader ReaderForClient = new StreamReader(StreamFromClient);

            //http header with get request
            /*string buffer;

            while ((buffer = ReaderForClient.ReadLine()) != "")
            {
                Console.WriteLine(sClient.LocalEndPoint + " -> Server: " + buffer);
            }*/

            CWebRequest WebRequest = new CWebRequest(ReaderForClient);

            Console.WriteLine();

            #region defaultfiles
            //defaultfiles
            /*string[] defaultFiles = new string[] { "index.html", "index.htm", "default.html", "default.htm" };
            string defaultFile = "";

            int ii = 0;

            while (ii <= defaultFiles.Length)
            {
                if (defaultFile == "")
                {
                    if (File.Exists(defaultFiles[ii]))
                    {
                        defaultFile = defaultFiles[ii];
                    }
                }
                ii++;
            }

            //handle the requested file (TODO)
            if (defaultFile == "")
            {

            }

            StreamReader fileReader = new StreamReader(defaultFile);
            FileInfo FI = new FileInfo(defaultFile);
            string sSize = Convert.ToString(FI.Length);*/
            #endregion

            string html = "<html><head><title>EmbeddedSensorCloud</title></head><body><h1>It works!</h1>" + DateTime.Now.ToString() + "</body></html>";

            int size = ASCIIEncoding.ASCII.GetByteCount(html);

            //encapsulate response!
            WriterForClient.WriteLine("HTTP/1.1 200 OK");
            WriterForClient.WriteLine("Server: EmbeddedSensorCloud Server");
            WriterForClient.WriteLine("Content-Length: " + size);
            WriterForClient.WriteLine("Content-Language: de");
            WriterForClient.WriteLine("Content-Type: text/html");
            WriterForClient.WriteLine("Connection: close");
            WriterForClient.WriteLine("");

            //write file
            //WriterForClient.WriteLine(fileReader.ReadToEnd());
            WriterForClient.WriteLine(html);

            WriterForClient.Flush();

            //close all writers and readers
            //fileReader.Close();
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
