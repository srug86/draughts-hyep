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
using Draughts.Domain;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para ProfileWin.xaml
    /// Ventana en la cual creamos un objeto de la clase player para poder jugar en red.
    /// </summary>
    public partial class ProfileWin : Window
    {
        /// <summary>
        /// Devuelve o modifica el campo passwordchar.
        /// </summary>
        /// <value>
        /// Passwordchar.
        /// </value>
        public char PasswordChar { get; set; }
        /// <summary>
        /// Ventana inicial.
        /// </summary>
        InitWin init;
        /// <summary>
        /// Jugador.
        /// </summary>
        Player p;
        /// <summary>
        /// Ruta para la imagenes.
        /// </summary>
        String ruta;
        /// <summary>
        /// Constructor de la clase <see cref="ProfileWin"/>.
        /// </summary>
        /// <param name="init">The init.</param>
        public ProfileWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            ruta = "";
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
        /// Manejador para el botón Cancelar.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        /// <summary>
        /// Manejador para el botón imagen 0.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image0_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Anand(IND).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image1_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Fischer(USA).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image2_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Karpov(URRS).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 3.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image3_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Kasimdzhanov(UZB).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 4.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image4_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Kasparov(RUS).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 5.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image5_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Kramnik(RUS).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 6.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image6_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Ponomariov(UCR).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 7.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image7_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Spassky(URRS).jpg"); }
        /// <summary>
        /// Manejador para el botón imagen 8.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Image8_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("/img/Topalov(BUL).jpg"); }

        /// <summary>
        /// Cargar una imagen.
        /// </summary>
        /// <param name="path">Ruta.</param>
        private void loadImage(string path)
        {
            ruta = path;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            imgProfile.Source = bitmap;
        }

        /// <summary>
        /// Manejador para el botón Aceptar.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if ((Tboxuser.Text.Length == 0) || (Tboxpwd.Password.Length == 0) || (ruta.Length == 0))
            {
                Textprofile.Text = "El usuario, contraseña y/o imagen no pueden ser vacios.";
                Textprofile.Foreground = Brushes.Orange;
            }
            else
            {
                p = new Player(Tboxuser.Text, Tboxpwd.Password, ruta);
                GameAdmin g = GameAdmin.Instance;
                if (g.existPlayer(p.Name))
                {
                    g.insertPlayer(p);
                    init.Visibility = Visibility.Visible;
                    this.Close();
                }
                else
                {
                    Textprofile.Text = "Nombre ya existente en la base de datos.";
                    Textprofile.Foreground = Brushes.Red;
                }
            }
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
