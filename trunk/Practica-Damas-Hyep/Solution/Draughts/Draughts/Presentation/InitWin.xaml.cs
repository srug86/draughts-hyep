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

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class InitWin : Window
    {
        public InitWin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProfileWin profile = new ProfileWin(this);
            profile.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnNet_Click(object sender, RoutedEventArgs e)
        {
            LoginWin login = new LoginWin(this);
            login.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            SelectWin select = new SelectWin(this);
            select.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnRanking_Click(object sender, RoutedEventArgs e)
        {
            RankingWin select = new RankingWin(this);
            select.Show();
            this.Visibility = Visibility.Hidden;
        }
    }
}
