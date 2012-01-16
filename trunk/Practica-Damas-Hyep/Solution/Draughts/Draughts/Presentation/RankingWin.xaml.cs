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
            ga.loadRanking();
            _RankCollection.Add(new PlayerRank
            {
                Pos = "1",
                Usuario = "Juan Miguel",
                G = "7",
                E = "2",
                P = "1",
            });
            InitializeComponent();
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
