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
    /// Esta clase utiliza el patrón Singleton.
    /// </summary>
    public sealed class NetMode
    {
        /// <summary>
        /// Atributo de tipo ConnectWin.
        /// </summary>
        private ConnectWin conn;
        /// <summary>
        /// Devuelve o modifica el atributo conn.
        /// </summary>
        /// <value>
        /// ConnectWin.
        /// </value>
        public ConnectWin Conn
        {
            get { return conn; }
            set { conn = value; }
        }
        /// <summary>
        /// Atributo de tipo GameWin para poder acceder a sus métodos.
        /// </summary>
        private GameWin game;

        /// <summary>
        /// Devuelve o modifica el atributo game.
        /// </summary>
        /// <value>
        /// GameWin.
        /// </value>
        public GameWin Game
        {
            get { return game; }
            set { game = value; }
        }
        /// <summary>
        /// Instancia de tipo de la clase NetMode.
        /// </summary>
        static readonly NetMode instance = new NetMode();
        /// <summary>
        /// Escucha las conexiones de clientes de red TCP.
        /// </summary>
        private TcpListener tcpListener = null; 
        /// <summary>
        /// Proporciona conexiones de cliente para servicios de red TCP.
        /// </summary>
        private TcpClient tcp = null;
        /// <summary>
        /// Datos a enviar.
        /// </summary>
        private byte[] dataToSend = null;
        /// <summary>
        /// Hilo para el servidor para recibir mensajes.
        /// </summary>
        private Thread serverThread = null;
        /// <summary>
        /// Hilo para el servidor para esperar conexiones de clientes.
        /// </summary>
        private Thread conectThread = null; 
        /// <summary>
        /// Hilo para el cliente para recibir mensajes.
        /// </summary>
        private Thread clientThread = null; 
        /// <summary>
        /// Instancia de la clase GameAdmin.
        /// </summary>
        private GameAdmin gAdmin = GameAdmin.Instance;
        /// <summary>
        /// Inicializa la clase <see cref="NetMode"/>.
        /// </summary>
        private bool disc = false;
        static NetMode()
        {
        }
        /// <summary>
        /// Impide la creación de una instancia por defecto de la clase <see cref="NetMode"/>.
        /// </summary>
        NetMode()
        {
        }
        /// <summary>
        /// Devuelve la instancia de tipo NetMode.
        /// </summary>
        public static NetMode Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// Actúa como servidor.
        /// </summary>
        /// <param name="ip_server">Dirección ip del servidor.</param>
        /// <param name="port_server">Puerto para la conexión.</param>
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

        /// <summary>
        /// Actúa como cliente.
        /// </summary>
        /// <param name="ip_server">Dirección ip del servidor.</param>
        /// <param name="ip_client">Dirección ip del cliente.</param>
        /// <param name="port_server">Puerto para la conexión.</param>
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
        /// <summary>
        /// El servidor espera la conexión de algún cliente.
        /// </summary>
        private void WaitListening()
        {
            bool follow = false;
            while (follow == false)
            {
                Thread.Sleep(100);
                // Si existe alguna petición se acepta y se crea un objeto con la conexión
                if (tcpListener.Pending())
                {
                    try
                    {
                        tcp = tcpListener.AcceptTcpClient();
                        disc = false;
                        this.serverThread = new Thread(this.InfiniteListening);
                        this.serverThread.Start();
                        follow = true;
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }
            }

        }
        /// <summary>
        /// Tanto el servidor como el cliente espera la llegada de mensajes.
        /// </summary>
        private void InfiniteListening()
        {
            String initial = "#%"+ Convert.ToString(this.gAdmin.PlayerNumber) +"%"+ this.gAdmin.Pl.Name + "%" + this.gAdmin.Pl.Avatar;
            sendMsg(initial);
            while(!disc)
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
        /// <summary>
        /// Enviar un mensaje.
        /// </summary>
        /// <param name="msg">Mensaje.</param>
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
        /// <summary>
        /// Analiza el mensaje recibido.
        /// </summary>
        /// <param name="message">Mensaje.</param>
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
            else if (message.StartsWith("#&"))
            {
                options = message.Split('&');
                game.delegateToRcvCoordinates(Convert.ToInt32(options[1]), Convert.ToInt32(options[2]),
                    Convert.ToInt32(options[3]), Convert.ToInt32(options[4]));
            }
        }
        /// <summary>
        /// Libera los recursos utilizados en la conexión.
        /// </summary>
        public void FreeResources()
        {
            if (tcp != null)
            {
                if (tcp.Connected)
                {
                    this.sendOff();
                }
            }
        }
        /// <summary>
        /// Enviar mensaje de desconexión.
        /// </summary>
        public void sendOff()
        {
            dataToSend = new byte[] { (byte)'s', (byte)'h', (byte)'u', (byte)'t', (byte)'d', (byte)'o', (byte)'w', (byte)'n' };
            tcp.GetStream().Write(dataToSend, 0, dataToSend.Length);
        }
        /// <summary>
        /// Comprobar el mensaje de desconexión.
        /// </summary>
        /// <param name="size">Tamaño del mensaje.</param>
        /// <param name="msg">Mensaje.</param>
        /// <returns></returns>
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
                    if (!disc) disc = true;
                    exit = true;
                }
            }
            return exit;
        }
    }
    /// <summary>
    /// Clase con las constantes utilizadas en la clase NetMode.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Constante con el número de bytes para leer en cada paquete.
        /// </summary>
        public const int noOfBytesToReadSuperPacket = 4;
        /// <summary>
        /// Constante con el número máximo de bytes.
        /// </summary>
        public const int maxNoOfBytes = 784;
    }
}
