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
using System.Net;
using System.Net.Sockets;
using Draughts.Communications;
using System.Threading;
using System.IO;
using Draughts.Domain;
using System.Reflection;
using System.Diagnostics;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para ConnectWin.xaml
    /// Ventana que permite a un jugador unirse a otro y jugar una partida.
    /// </summary>
    public partial class ConnectWin : Window
    {
        /// <summary>
        /// Ventana inicial.
        /// </summary>
        InitWin init;
        /// <summary>
        /// Instancia de NetMode.
        /// </summary>
        NetMode net;
        /// <summary>
        /// Instanacia de GameAdmin.
        /// </summary>
        private GameAdmin gA;
        /// <summary>
        /// Jugador enemigo.
        /// </summary>
        private Player enemy;

        /// <summary>
        /// Constructor de la clase <see cref="ConnectWin"/>.
        /// </summary>
        /// <param name="init">InitWin.</param>
        /// <param name="gA">GameAdmin.</param>
        public ConnectWin(InitWin init, GameAdmin gA)
        {
            this.init = init;
            this.gA = gA;
            net = NetMode.Instance;
            net.Conn = this;
            InitializeComponent();
            textIp.Text = this.LocalIPAddress();
        }

        /// <summary>
        /// Manejador para el botón Atrás.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        /// <summary>
        /// Manejador para el botón Exit.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        /// <summary>
        /// Devuelve tu dirección IP.
        /// </summary>
        /// <returns>Dirección IP.</returns>
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                    localIP = ip.ToString();
            }
            return localIP;
        }

        /// <summary>
        /// Manejador para el botón Crear partida.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            gA.PlayerNumber = 1;
            this.txtPl1.Text = gA.Pl.Name;
            this.imgAvPl1.Source = loadImage(gA.Pl.Avatar);
            String ip = LocalIPAddress();
            int port = int.Parse(this.textPort.Text);
            net.ModeServer(ip, port);
        }

        /// <summary>
        /// Carga una imagen.
        /// </summary>
        /// <param name="path">Ruta.</param>
        /// <returns>Imagen.</returns>
        public BitmapImage loadImage(String path)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path, UriKind.Relative);
            bi.EndInit();
            return bi;
        }

        /// <summary>
        /// Manejador para el botón Unirse a una partida.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            gA.PlayerNumber = 2;
            this.txtPl2.Text = gA.Pl.Name;
            this.imgAvPl2.Source = loadImage(gA.Pl.Avatar);
            String ips = this.textIp.Text;
            String ipc = LocalIPAddress();
            int port = int.Parse(this.textPort.Text);
            net.ModeClient(ips, ipc, port);
        }

        /// <summary>
        /// Manejador para el botón Enviar.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            String message = gA.Pl.Name+ ":" + textMsg.Text;
            net.sendMsg(message);
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(message));
            fdoc.Blocks.Add(p);
            textMsg.Text = "";
        }

        /// <summary>
        /// Manejador para el botón Empezar.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            string begin = "#$";
            net.sendMsg(begin);
            GameActions gActions;
            if (gA.PlayerNumber == 1)
            {
                gActions = new GameActions(this.init, gA.Pl, this.enemy);
            }
            else
            {
                gActions = new GameActions(this.init, this.enemy, gA.Pl);
            }
            gActions.NetGame = true;
            this.Close();
        }

        /// <summary>
        /// Delegado para escribir en la ventana los mensajes recibidos.
        /// </summary>
        /// <param name="msg">Mensaje.</param>
        private delegate void ReceiveMessagesFrom(string msg);
        /// <summary>
        /// Método para el delegado.
        /// </summary>
        /// <param name="msg">Mensaje.</param>
        public void delegateToRcvMsgs(string msg)
        {
            ReceiveMessagesFrom aux = new ReceiveMessagesFrom(this.writeMessage);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, aux, msg);
        }

        /// <summary>
        /// Escribir el mensaje en la ventana.
        /// </summary>
        /// <param name="msg">Mensaje.</param>
        public void writeMessage(String msg)
        {
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(msg));
            this.fdoc.Blocks.Add(p);
        }
        
        /// <summary>
        /// Delegado para mostrar los datos del enemigo.
        /// </summary>
        /// <param name="opt">Opción.</param>
        /// <param name="name">Nombre.</param>
        /// <param name="path">Ruta imagen.</param>
        private delegate void ReceiveEnemyData(string opt, string name, string path);
        /// <summary>
        /// Método para el delegado.
        /// </summary>
        /// <param name="opt">Opción.</param>
        /// <param name="name">Nombre.</param>
        /// <param name="path">Ruta imagen.</param>
        public void delegateToRcvData(string opt, string name, string path)
        {
            ReceiveEnemyData auxi = new ReceiveEnemyData(this.showEnemy);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, auxi, opt, name, path);
        }

        /// <summary>
        /// Mostrar al enemigo en la ventana.
        /// </summary>
        /// <param name="opt">Opción.</param>
        /// <param name="name">Nombre.</param>
        /// <param name="path">Ruta imagen.</param>
        public void showEnemy(string opt, string name, string path)
        {
            enemy = new Player(name, "", path);
            if (opt.Equals("1"))
            {
                this.txtPl1.Text = name;
                this.imgAvPl1.Source = loadImage(path);
            }
            else
            {
                this.txtPl2.Text = name;
                this.imgAvPl2.Source = loadImage(path);
            }
            this.btnBegin.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Delegado para recibir el juego
        /// </summary>
        private delegate void ReceiveGame();
        /// <summary>
        /// Método para el delegado.
        /// </summary>
        public void delegateToBeginGame()
        {
            ReceiveGame au = new ReceiveGame(this.openPerspective);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, au);
        }

        /// <summary>
        /// Abrir la ventana de juego.
        /// </summary>
        public void openPerspective()
        {
            GameActions gActions;
            if (gA.PlayerNumber == 1)
            {
                gActions = new GameActions(this.init, gA.Pl, this.enemy);
            }
            else
            {
                gActions = new GameActions(this.init, this.enemy, gA.Pl);
            }
            gActions.NetGame = true;
            this.Close();
        }

        /// <summary>
        /// Manejador para el botón Ayuda.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            directory = Path.Combine(directory, "../../Resources");
            var file = Path.Combine(directory, "Documentation.chm");
            Process.Start(file); 
        }
    }
}
