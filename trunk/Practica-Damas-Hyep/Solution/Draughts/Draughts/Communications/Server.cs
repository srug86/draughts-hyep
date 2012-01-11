using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Draughts.Communications
{
    class Server
    {
        private TcpListener tcpListener = null; //Socket a la espera de punto remoto
        private TcpClient tcpClient = null;   //Socket de comunicación para el cliente
        private Thread threadServer = null; //Hilo para el servidor
        private byte[] dataToSend = null; //Datos a enviar
        ArrayList conections;   //Colección de conexiones creada
        private String myIP;
        private int port;   //Puerto al que conectarse

        public Server(String ipServer, int port)
        {
            this.myIP = ipServer;
            this.port = port;
            // Obtenemos el localhost
            IPHostEntry localHost = Dns.GetHostEntry(Dns.GetHostName());
            // Creamos el EndPoint
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(this.myIP), port);
            // Instanciamos el canal TCP a la escucha
            tcpListener = new TcpListener(localEndPoint);
            tcpListener.Start();
            // Esperamos clientes en un hilo aparte
            threadServer = new Thread(this.InfiniteListening);
            threadServer.Start();
        }

        private void InfiniteListening()
        {
            for (;;)
            {
                Thread.Sleep(100);
                // Si existe alguna petición se acepta y se crea un objeto con la conexión
                if (tcpListener.Pending())
                {
                    try
                    {
                        int port = this.port + 2;
                        tcpClient = tcpListener.AcceptTcpClient();
                        SndPort(tcpClient, port.ToString());
                        tcpClient.Close();    //Cerramos y creamos nueva conexión en el nuevo puerto
                        TcpListener aux = new TcpListener(IPAddress.Parse(this.myIP), port);
                        aux.Start();
                        while (!aux.Pending()) Thread.Sleep(100);
                        tcpClient = aux.AcceptTcpClient();
                    }
                    catch (IOException) { return; }
                }
            }
        }

        private void SndPort(TcpClient tc, string msg)
        {
            //Prepara mensaje para enviar como array de bytes
            char[] charArray = msg.ToCharArray(0, msg.Length);
            dataToSend = new byte[msg.Length];
            for (int charCount = 0; charCount < msg.Length; charCount++)
            {
                dataToSend[charCount] = (byte)charArray[charCount];
            }
            //Usamos el socket para enviar el mensaje
            tc.GetStream().Write(dataToSend, 0, dataToSend.Length);
        }
    }
}
