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

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para ConnectWin.xaml
    /// </summary>
    public partial class ConnectWin : Window
    {
        InitWin init;
        Client client = null;
        Server server = null;
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
            server = new Server(textIp.Text, int.Parse(textPort.Text));

        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            client = new Client (LocalIPAddress(), textIp.Text, int.Parse(textPort.Text));
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (this.textMsg.Text.Length != 0 && client != null)
            {
                //Escribir mensaje en la ventana local
                textMsg.Text = "> " + textMsg.Text;
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run(textMsg.Text));
                fdoc.Blocks.Add(p);
                client.sendMsg(textMsg.Text);
                textMsg.Text = "";
            }
        }

        private void showMsg(String msg)
        {
            String m = client.writeMsg(msg);
            //Escribo en mi interfaz
            Paragraph p = new Paragraph();
            BitmapImage bmi0 = new BitmapImage();
            p.Inlines.Add(new Run(m));
            fdoc.Blocks.Add(p);
        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
