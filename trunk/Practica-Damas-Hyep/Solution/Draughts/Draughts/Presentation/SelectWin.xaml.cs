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
using System.Data;
using Draughts.Domain;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para SelectWin.xaml
    /// Ventana que permite al usuario elegir jugadores (incluida la cpu) para una partida normal.
    /// </summary>
    public partial class SelectWin : Window
    {
        /// <summary>
        /// Jugadores de la partida.
        /// </summary>
        Player p1, p2;
        /// <summary>
        /// Ventana inicial.
        /// </summary>
        InitWin init;
        /// <summary>
        /// Cola circular con las rutas de las imágenes.
        /// </summary>
        BoundedQueue<String> images;
        /// <summary>
        /// Índices para la cola circular.
        /// </summary>
        int ind, ind2;
        /// <summary>
        /// Rutas para las imágenes de la ventana.
        /// </summary>
        String ruta1, ruta2;
        /// <summary>
        /// Constructor de la clase <see cref="SelectWin"/>.
        /// </summary>
        /// <param name="init">InitWin.</param>
        public SelectWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            ind = 0;
            ind2 = 0;
            images = new BoundedQueue<String>(10);
            images.enqueue("/img/Anand(IND).jpg");
            images.enqueue("/img/Fischer(USA).jpg");
            images.enqueue("/img/Karpov(URRS).jpg");
            images.enqueue("/img/Kasimdzhanov(UZB).jpg");
            images.enqueue("/img/Kasparov(RUS).jpg");
            images.enqueue("/img/Kramnik(RUS).jpg");
            images.enqueue("/img/Ponomariov(UCR).jpg");
            images.enqueue("/img/Spassky(URRS).jpg");
            images.enqueue("/img/Topalov(BUL).jpg");
            images.enqueue("/img/cpu.png");
            ruta1 = "/img/Anand(IND).jpg";
            ruta2 = "/img/cpu.png";
            Imagej1.Source = loadImage(ruta1);
            Imagej2.Source = loadImage(ruta2);
        }

        /// <summary>
        /// Cargar una imagen.
        /// </summary>
        /// <param name="path">Ruta.</param>
        /// <returns>Imagen</returns>
        public BitmapImage loadImage(String path)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path, UriKind.Relative);
            bi.EndInit();
            return bi;
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
        /// Manejador para el botón siguiente del jugador1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            ruta1 = images.getNext(ind);
            if (ruta1 == "/img/cpu.png")
            {
                ind++;
                ruta1 = images.getNext(ind);
            }
            Imagej1.Source = loadImage(ruta1);
            ind++;
        }

        /// <summary>
        /// Manejador para el boton anterior del jugador1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            ruta1 = images.getPrev(ind);
            if (ruta1 == "/img/cpu.png")
            {
                ind--;
                ruta1 = images.getNext(ind);
            }
            Imagej1.Source = loadImage(ruta1);
            ind--;
        }

        /// <summary>
        /// Manejador para el botón siguiente del jugador2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnPrev2_Click(object sender, RoutedEventArgs e)
        {
            ruta2 = images.getPrev(ind2);
            Imagej2.Source = loadImage(ruta2);
            ind2--;
        }

        /// <summary>
        /// Manejador para el botón anterior del jugador2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            ruta2 = images.getNext(ind2);
            Imagej2.Source = loadImage(ruta2);
            ind2++;
        }

        /// <summary>
        /// Manejador para el botón Empezar.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            p1 = new Player(TextboxJ1.Text, "", ruta1);
            p2 = new Player(TextboxJ2.Text, "", ruta2);
            GameActions gameAdmin = new GameActions(init, p1, p2);
            this.Close();
        }

        /// <summary>
        /// Manejador para el botón Ayuda.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
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
