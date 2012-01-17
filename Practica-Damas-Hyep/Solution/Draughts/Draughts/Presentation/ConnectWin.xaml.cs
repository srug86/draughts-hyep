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
using Draughts.Domain;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para ConnectWin.xaml
    /// </summary>
    public partial class ConnectWin : Window
    {
        InitWin init;
        NetMode net;
        private GameAdmin gA;

        public ConnectWin(InitWin init, GameAdmin gA)
        {
            this.init = init;
            this.gA = gA;
            net = NetMode.Instance;
            net.Conn = this;
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
            this.txtPl1.Text = gA.Pl.Name;
            this.imgAvPl1.Source = loadImage(gA.Pl.Avatar);
            String ip = LocalIPAddress();
            int port = int.Parse(this.textPort.Text);
            net.ModeServer(ip, port);
        }

        public BitmapImage loadImage(String path)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path, UriKind.Relative);
            bi.EndInit();
            return bi;
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            String ips = this.textIp.Text;
            String ipc = LocalIPAddress();
            int port = int.Parse(this.textPort.Text);
            net.ModeClient(ips, ipc, port);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            String message = ">>" + textMsg.Text;
            net.sendMsg(message);
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(textMsg.Text));
            fdoc.Blocks.Add(p);
            textMsg.Text = "";
        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {

        }

        //Delegado para escribir en la ventana los mensajes recibidos
        private delegate void ReceiveMessagesFrom(string msg);
        public void delegateToRcvMsgs(string msg)
        {
            ReceiveMessagesFrom aux = new ReceiveMessagesFrom(this.writeMessage);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, aux, msg);
        }

        public void writeMessage(String msg)
        {
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(msg));
            this.fdoc.Blocks.Add(p);
        }
    }
}
