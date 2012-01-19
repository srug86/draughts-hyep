using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Draughts.Presentation;
using Draughts.Domain;
using System.Reflection;
using System.Diagnostics;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para LoginWin.xaml
    /// Ventana de acceso para poder jugar en red.
    /// </summary>
	public partial class LoginWin: Window
	{
        /// <summary>
        /// Ventana inicial.
        /// </summary>
        InitWin init;
        /// <summary>
        /// Constructor de la clase <see cref="LoginWin"/>.
        /// </summary>
        /// <param name="init">The init.</param>
		public LoginWin(InitWin init)
		{
            this.init = init;
			InitializeComponent();
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
        /// Manejador para el botón Aceptar.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if ((Texboxname.Text.Length == 0) || (Texboxpwd.Password.Length == 0))
            {
                Textlogin.Text = "El usuario y/o la contraseña son vacios.";
                Textlogin.Foreground = Brushes.Orange;
            }
            else
            {
                GameAdmin g = GameAdmin.Instance;
                bool b = g.loginPlayer(Texboxname.Text, Texboxpwd.Password);
                if (b == true)
                {
                    ConnectWin conect = new ConnectWin(init, g);
                    conect.Show();
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    Textlogin.Text = "Usuario o contraseña incorrectos...";
                    Textlogin.Foreground = Brushes.Red;
                }
            }
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