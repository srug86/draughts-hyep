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
using System.Data.SqlClient;
using System.Data;
using Draughts.Domain;
using Draughts.Communications;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para RankingWin.xaml
    /// Ventana que muestra el mejor jugador y el ranking con los 10 mejores jugadores.
    /// </summary>
    public partial class RankingWin : Window
    {
        /// <summary>
        /// Ventana inicial.
        /// </summary>
        InitWin init;
        /// <summary>
        /// Instancia de GameAdmin.
        /// </summary>
        GameAdmin ga = GameAdmin.Instance;
        /// <summary>
        /// Colección observable para los jugadores del ranking.
        /// </summary>
        ObservableCollection<PlayerRank> _RankCollection = new ObservableCollection<PlayerRank>();

        /// <summary>
        /// Constructor de la clase <see cref="RankingWin"/>.
        /// </summary>
        /// <param name="init">InitWin.</param>
        public RankingWin(InitWin init)
        {
            this.init = init;
            ArrayList l = ga.loadRanking();
            Player p = (Player)l[0];
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(p.Avatar, UriKind.Relative);
            bi.EndInit();
            showRanking(_RankCollection,l);
            InitializeComponent();
            this.textWin.Content = p.Name;
            this.imgWin.Source = bi;
        }

        /// <summary>
        /// Mostrar el ranking actual.
        /// </summary>
        /// <param name="_RankCollection">Colección ranking.</param>
        /// <param name="li">Lista players.</param>
        private void showRanking(ObservableCollection<PlayerRank> _RankCollection,ArrayList li)
        {
            Player aux = new Player("", "", "");
            int p = 1;
            while (li.Count > 0)
            {
                aux = (Player)li[0];
                _RankCollection.Add(new PlayerRank
                {
                    Pos = Convert.ToString(p),
                    Usuario = Convert.ToString(aux.Name),
                    G = Convert.ToString(aux.Wins),
                    E = Convert.ToString(aux.Draws),
                    P = Convert.ToString(aux.Loses),
                });
                p = p + 1;
                li.RemoveAt(0);
            }
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
        /// Devuelve la collección que contiene el ranking.
        /// </summary>
        public ObservableCollection<PlayerRank> RankCollection
        { get { return _RankCollection; } }

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

    /// <summary>
    /// Clase que usa la colección observable para los jugadores del ranking.
    /// </summary>
    public class PlayerRank
    {
        /// <summary>
        /// Devuelve o modifica la posición del jugador.
        /// </summary>
        /// <value>
        /// Posición.
        /// </value>
        public string Pos { get; set; }
        /// <summary>
        /// Devuelve o modifica el nombre del jugador.
        /// </summary>
        /// <value>
        /// Nombre.
        /// </value>
        public string Usuario { get; set; }
        /// <summary>
        /// Devuelve o modifica el número de partidas ganadas.
        /// </summary>
        /// <value>
        /// Partidas ganadas.
        /// </value>
        public string G { get; set; }
        /// <summary>
        /// Devuelve o modifica el número de partidas empatadas.
        /// </summary>
        /// <value>
        /// Partidas empatadas.
        /// </value>
        public string E { get; set; }
        /// <summary>
        /// Devuelve o modifica el número de partidas perdidas.
        /// </summary>
        /// <value>
        /// Partidas perdidas.
        /// </value>
        public string P { get; set; }
    }
}
