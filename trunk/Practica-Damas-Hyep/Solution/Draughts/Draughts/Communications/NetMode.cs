using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System.Windows.Threading;
using System.Windows.Documents;
using Draughts.Domain;
using System.Diagnostics;

namespace Draughts.Communications
{
    /// <summary>
    /// Clase que permite conectar a dos jugadores (uno a uno) y así poder empezar una partida de damas.
    /// </summary>
    public sealed class NetMode
    {
        private ConnectWin conn;
        public ConnectWin Conn
        {
            get { return conn; }
            set { conn = value; }
        }
        static readonly NetMode instance = new NetMode();
        private TcpListener tcpListener = null; //Socket a la espera de punto remoto
        private TcpClient tcp = null; //Socket de comunicación para el cliente
        private byte[] dataToSend = null; //Datos a enviar
        private Thread serverThread = null; //Hilo para el servidor
        private Thread conectThread = null; //Hilo para la conexion al servidor
        private Thread clientThread = null; //Hilo para el cliente
        private GameAdmin gAdmin = GameAdmin.Instance;
        static NetMode()
        {
        }

        NetMode()
        {
        }

        public static NetMode Instance
        {
            get
            {
                return instance;
            }
        }

        public void ModeServer(String ip_server, int port_server)
        {
            if (tcp == null)
            {
                IPHostEntry localHost = Dns.GetHostEntry(Dns.GetHostName());
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip_server), port_server);
                // Instanciamos el canal TCP a la escucha.
                tcpListener = new TcpListener(localEndPoint);
                tcpListener.Start();
                this.conectThread = new Thread(this.WaitListening);
                this.conectThread.Start();
            }
        }

        public void ModeClient(String ip_server, String ip_client, int port_server)
        {
            if (tcp == null)
            {
                tcp = new TcpClient(new IPEndPoint(IPAddress.Parse(ip_client), port_server + 1));
                LingerOption lingerOption = new LingerOption(false, 1);
                tcp.LingerState = lingerOption;
                tcp.Connect(IPAddress.Parse(ip_server), port_server);
                //Crear hilo para que esté atento a lo que le envían
                this.clientThread = new Thread(this.InfiniteListening);
                this.clientThread.Start();
            }
        }

        private void WaitListening()
        {
            for (; ; )
            {
                Thread.Sleep(100);
                // Si existe alguna petición se acepta y se crea un objeto con la conexión
                if (tcpListener.Pending())
                {
                    try
                    {
                        tcp = tcpListener.AcceptTcpClient();
                        this.serverThread = new Thread(this.InfiniteListening);
                        this.serverThread.Start();
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }
            }

        }

        private void InfiniteListening()
        {
            String initial = "#%"+ Convert.ToString(this.gAdmin.PlayerNumber) +"%"+ this.gAdmin.Pl.Name + "%" + this.gAdmin.Pl.Avatar;
            sendMsg(initial);
            for (; ; )
            {
                Thread.Sleep(100);
                byte[] msg = new Byte[Constants.maxNoOfBytes];
                byte count1 = 0x01;
                for (int i = 0; i < msg.Length; i++)
                {
                    msg[i] = count1++;
                }
                try
                {
                    //Leemos el buffer por si se ha recibido algo
                    int readBytes = tcp.GetStream().Read(msg, 0, msg.Length);
                    if (checkOff(readBytes, msg))
                    {
                        return;
                    }
                    else
                    {
                        //intentamos recoger el mensaje y ponerlo en pantalla
                        StringBuilder str = new StringBuilder(Constants.maxNoOfBytes);
                        for (int count = 0; count < readBytes; count++)
                        {
                            char ch = (char)msg[count];
                            str = str.Append(ch);
                        }
                        String cad = str.ToString();
                        if (cad.StartsWith("#"))
                        {
                            checkMessage(cad);
                        }
                        else
                        {
                            //Pintamos el mensaje a través de un delegado.
                            conn.delegateToRcvMsgs(cad);
                        }
                    }
                }
                catch (IOException)
                {
                    return;
                }
            }
        }

        public void sendMsg(String msg)
        {
            if (tcp != null)
            {
                if (tcp.Connected)
                {
                    if (msg.Length != 0)
                    {
                        //Prepara mensaje para enviar como array de bytes
                        char[] charArray = msg.ToCharArray(0, msg.Length);
                        dataToSend = new byte[msg.Length];
                        for (int charCount = 0; charCount < msg.Length; charCount++)
                        {
                            dataToSend[charCount] = (byte)charArray[charCount];
                        }
                        //Usamos el socket para enviar el mensaje
                        tcp.GetStream().Write(dataToSend, 0, dataToSend.Length);
                    }
                }
            }
        }

        private void checkMessage(String message)
        {
            String[] options;
            if (message.StartsWith("#%"))
            {
                options = message.Split('%');
                conn.delegateToRcvData(options[1], options[2], options[3]);
            }
            else if (message.StartsWith("#$"))
            {
                conn.delegateToBeginGame();
            }
        }

        public void FreeResources()
        {
            if (tcp != null)
            {
                if (tcp.Connected)
                {
                    this.sendOff();
                    if (tcp != null) tcp.GetStream().Close();
                    if (tcpListener != null) tcpListener.Stop();
                    if (tcp != null) tcp.Close();
                    tcp = null;
                }
            }
        }

        private void sendOff()
        {
            dataToSend = new byte[] { (byte)'s', (byte)'h', (byte)'u', (byte)'t', (byte)'d', (byte)'o', (byte)'w', (byte)'n' };
            tcp.GetStream().Write(dataToSend, 0, dataToSend.Length);
        }

        private bool checkOff(int size, byte[] msg)
        {
            //Si me envian "shutdown", desconecto los sockets
            bool exit = false;
            if (size == 8)
            {
                StringBuilder shutMessage = new StringBuilder(8);
                for (int count = 0; count < 8; count++)
                {
                    char ch = (char)msg[count];
                    shutMessage = shutMessage.Append(ch);
                }
                string shut = "shutdown";
                string receivedMessage = shutMessage.ToString();
                if (receivedMessage.Equals(shut))
                {
                    conn.delegateToRcvMsgs("--Desconectado--");
                    if (tcp != null)
                        tcp.GetStream().Close();
                    if (tcpListener != null)
                        tcpListener.Stop();
                    if (tcp != null)
                        tcp.Close();
                    exit = true;
                }
            }
            return exit;
        }
    }

    public class Constants
    {
        public const int noOfBytesToReadSuperPacket = 4;
        public const int maxNoOfBytes = 784;
    }
}
