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
using System.Data.SqlClient;
using System.Data;
using Draughts.Domain;
using Draughts.Communications;
using System.Collections.ObjectModel;
using System.Collections;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para RankingWin.xaml
    /// </summary>
    public partial class RankingWin : Window
    {
        InitWin init;
        GameAdmin ga;
        ObservableCollection<PlayerRank> _RankCollection = new ObservableCollection<PlayerRank>();

        public RankingWin(InitWin init)
        {
            this.init = init;
            ga = new GameAdmin(init);
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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }
        public ObservableCollection<PlayerRank> RankCollection
        { get { return _RankCollection; } }
    }

    public class PlayerRank
    {
        public string Pos { get; set; }
        public string Usuario { get; set; }
        public string G { get; set; }
        public string E { get; set; }
        public string P { get; set; }
    }
}
