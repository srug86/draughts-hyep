using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using Draughts.Communications;
using System.Threading;
using System.IO;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para ConnectWin.xaml
    /// </summary>
    public partial class ConnectWin : Window
    {
        InitWin init;
        private TcpListener tcpListener = null; //Socket a la espera de punto remoto
        private TcpClient tcp = null; //Socket de comunicación para el cliente
        private byte[] dataToSend = null; //Datos a enviar
        private Thread serverThread = null; //Hilo para el servidor
        private Thread conectThread = null; //Hilo para la conexion al servidor
        private Thread clientThread = null; //Hilo para el cliente
        public ConnectWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            textIp.Text = this.LocalIPAddress();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    if (ip.ToString().Substring(0, 3) != "192")
                        localIP = ip.ToString();
                }
            }
            return localIP;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (tcp == null)
            {
                // Obtenemos el localhost
                IPHostEntry localHost = Dns.GetHostEntry(Dns.GetHostName());
                // Creamos el EndPoint
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(LocalIPAddress()), int.Parse(this.textPort.Text));
                // Instanciamos el canal TCP a la escucha.
                tcpListener = new TcpListener(localEndPoint);
                tcpListener.Start();
                conectThread = new Thread(this.WaitListening);
                conectThread.Start();
            }

        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (tcp == null)
            {
                //Obtener IP destino
                IPAddress address = IPAddress.Parse(this.textIp.Text);
                //Conectar al socket destino en espera
                tcp = new TcpClient(new IPEndPoint(IPAddress.Parse(LocalIPAddress()), int.Parse(this.textPort.Text) + 1));
                LingerOption lingerOption = new LingerOption(false, 1);
                tcp.LingerState = lingerOption;
                // Concetarnos al puerto e ip remotos
                tcp.Connect(IPAddress.Parse(this.textIp.Text), int.Parse(this.textPort.Text));
                //Crear hilo para que esté atento a lo que le envían
                clientThread = new Thread(this.InfiniteListening);
                clientThread.Start();
            }

        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (tcp != null)
            {
                if (tcp.Connected)
                {
                    if (this.textMsg.Text.Length != 0)
                    {
                        //Escribir mensaje en la ventana local
                        this.textMsg.Text = "> " +
                            textMsg.Text;
                        Paragraph p = new Paragraph();
                        p.Inlines.Add(new Run(textMsg.Text));
                        fdoc.Blocks.Add(p);
                        //Prepara mensaje para enviar como array de bytes
                        char[] charArray = textMsg.Text.ToCharArray(0, textMsg.Text.Length);
                        dataToSend = new byte[textMsg.Text.Length];
                        for (int charCount = 0; charCount < textMsg.Text.Length; charCount++)
                        {
                            dataToSend[charCount] = (byte)charArray[charCount];
                        }
                        //Usamos el socket para enviar el mensaje
                        tcp.GetStream().Write(dataToSend, 0, dataToSend.Length);
                        textMsg.Text = "";
                    }
                }
            }

        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {

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
                        serverThread = new Thread(this.InfiniteListening);
                        serverThread.Start();
                    }
                    catch (Exception e)
                    {
                        return;
                    }
                }
            }

        }

        private void InfiniteListening()
        {
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
                        //Pintamos el mensaje a través de un delegado.
                        this.delegateToRcvMsgs(str.ToString());
                    }
                }
                catch (IOException)
                {
                    return;
                }
            }
        }
        //Delegado para escribir en la ventana los mensajes recibidos
        private delegate void ReceiveMessagesFrom(string msg);
        private void delegateToRcvMsgs(string msg)
        {
            ReceiveMessagesFrom aux = new ReceiveMessagesFrom(this.writeMessage);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, aux, msg);
        }

        private void writeMessage(string msg)
        {
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(msg));
            fdoc.Blocks.Add(p);
        }
        private void FreeResources(object sender, System.ComponentModel.CancelEventArgs e)
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
                    this.delegateToRcvMsgs("--Desconectado--");
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
