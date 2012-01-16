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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using System.Windows.Threading;

namespace Draughts.Communications
{
    class Client
    {
        private const int noOfBytesToReadForSuperPacket = 4;
        private const int maxNoOfBytes = 784;

        private TcpClient socket = null;   //Socket de comunicación para el cliente
        private Thread th;
        private String myIP;
        private String ipServer;
        private int port;

        public Client(String ipClient, String ipServer, int port)
        {
            this.myIP = ipClient;
            this.ipServer = ipServer;
            this.port = port;
            //Conectar al socket destino en espera
            socket = new TcpClient(new IPEndPoint(IPAddress.Parse(this.myIP), this.port + 1));
            LingerOption lingerOption = new LingerOption(false, 1);
            socket.LingerState = lingerOption;
            //Conectarnos al puerto IP remotos
            socket.Connect(IPAddress.Parse(this.ipServer), this.port);
            string newPort = rcvPort();
            socket.Close();
            socket = new TcpClient(new IPEndPoint(IPAddress.Parse(this.myIP), int.Parse(newPort) + 1));
            socket.Connect(IPAddress.Parse(this.ipServer), int.Parse(newPort));
            th = new Thread(this.rcvInfiniteMsg);
            th.Start();
        }

        private string rcvPort()
        {
            int readBytes = 0;
            StringBuilder msg = new StringBuilder(maxNoOfBytes);
            byte[] byteNom = new byte[maxNoOfBytes];
            byte count1 = 0x01;
            for (int i = 0; i < byteNom.Length; i++)
                byteNom[i] = count1++;
            while (readBytes == 0)
            {
                readBytes = socket.GetStream().Read(byteNom, 0, byteNom.Length);
                for (int i = 0; i < readBytes; i++)
                    msg.Append((char)byteNom[i]);
                Thread.Sleep(200);
            }
            string message = msg.ToString();
            if (message == null) message = rcvPort();
            return message;
        }

        private void rcvInfiniteMsg()
        {
            for (; ; )
            {
                Thread.Sleep(100);
                byte[] msg = new Byte[maxNoOfBytes];
                byte count1 = 0x01;
                for (int i = 0; i < msg.Length; i++) msg[i] = count1++;
                try
                {
                    //Leemos el buffer por si se ha recibido algo
                    int readBytes = socket.GetStream().Read(msg, 0, msg.Length);
                    if (checkOff(readBytes, msg)) { return; }
                    else
                    {
                        //intentamos recoger el mensaje y ponerlo en pantalla
                        StringBuilder str = new StringBuilder(maxNoOfBytes);
                        for (int count = 0; count < readBytes; count++)
                        {
                            char ch = (char)msg[count];
                            str = str.Append(ch);
                        }
                        //Pintamos el mensaje a través de un delegado
                        this.delegateToRcvMsgs(str.ToString());
                    }
                }
                catch (IOException) { return; }
            }
        }

        public void sendMsg(String msg)
        {
            char[] charArray = msg.ToCharArray();
            byte[] byteArray = new byte[charArray.Length];
            for (int count = 0; count < charArray.Length; count++)
                byteArray[count] = (byte)charArray[count];
            socket.GetStream().Write(byteArray, 0, byteArray.Length);
        }

        //Delegado para escribir en la ventana los mensajes recibidos
        private delegate String RcvMsgFrom(String msg);
        private void delegateToRcvMsgs(String msg)
        {
            RcvMsgFrom aux = new RcvMsgFrom(this.writeMsg);
            Dispatcher.CurrentDispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, aux, msg);
        }

        public String writeMsg(String msg)
        {
            string[] aux = msg.Split('>');
            return aux[1];
        }

        private void sendOff()
        {
            byte[] dataToSend = new byte[] { (byte)'s', (byte)'h', (byte)'u', (byte)'t', (byte)'d', (byte)'o', (byte)'w', (byte)'n' };
            socket.GetStream().Write(dataToSend, 0, dataToSend.Length);
        }

        public bool checkOff(int size, byte[] msg)
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
                    if (socket != null)
                        socket.GetStream().Close();
                    if (socket != null)
                        socket.Close();
                    exit = true;
                }
            }
            return exit;
        }
    }
}
