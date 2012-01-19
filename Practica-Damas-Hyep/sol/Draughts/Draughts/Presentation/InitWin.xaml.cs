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
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para InitWin.xaml
    /// Ventana inicial de la aplicación.
    /// </summary>
    public partial class InitWin : Window
    {
        /// <summary>
        /// Constructor de la clase <see cref="InitWin"/>.
        /// </summary>
        public InitWin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Manejador para el botón Exit.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "Draughts.vshost") p.Kill();
            }
        }

        /// <summary>
        /// Manejador para el botón Añadir Usuario.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProfileWin profile = new ProfileWin(this);
            profile.Show();
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Manejador para el botón Jugar en Red.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNet_Click(object sender, RoutedEventArgs e)
        {
            LoginWin login = new LoginWin(this);
            login.Show();
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Manejador para el botón Iniciar Partida.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            SelectWin select = new SelectWin(this);
            select.Show();
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Manejador para el botón Ranking.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnRanking_Click(object sender, RoutedEventArgs e)
        {
            RankingWin select = new RankingWin(this);
            select.Show();
            this.Visibility = Visibility.Hidden;
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
