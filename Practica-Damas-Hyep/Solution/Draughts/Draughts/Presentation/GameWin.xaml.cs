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
using Draughts.Domain;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para GameWin.xaml
    /// </summary>
    public partial class GameWin : Window
    {
        InitWin init;
        Player pl1, pl2;
        GameAdmin gameAdmin;

        public GameWin(InitWin init, Player pl1, Player pl2)
        {
            this.pl1 = pl1;
            this.pl2 = pl2;
            this.init = init;
            gameAdmin = new GameAdmin(pl1, pl2);
            InitializeComponent();
            textj1.Text = pl1.Name;
            textj2.Text = pl2.Name;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(pl1.Avatar, UriKind.Relative);
            bi.EndInit();
            imgj1.Source = bi;
            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri(pl2.Avatar, UriKind.Relative);
            bi2.EndInit();
            imgj2.Source = bi2;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Click_1x2(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(0, 1);
        }

        private void Click_1x4(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(0, 3);
        }

        private void Click_1x6(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(0, 5);
        }

        private void Click_1x8(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(0, 7);
        }

        private void Click_2x1(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(1, 0);
        }

        private void Click_2x3(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(1, 2);
        }

        private void Click_2x5(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(1, 4);
        }

        private void Click_2x7(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(1, 6);
        }

        private void Click_3x2(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(2, 1);
        }

        private void Click_3x4(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(2, 3);
        }

        private void Click_3x6(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(2, 5);
        }

        private void Click_3x8(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(2, 7);
        }

        private void Click_4x1(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(3, 0);
        }

        private void Click_4x3(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(3, 2);
        }

        private void Click_4x5(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(3, 4);
        }

        private void Click_4x7(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(3, 6);
        }

        private void Click_5x2(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(4, 1);
        }

        private void Click_5x4(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(4, 3);
        }

        private void Click_5x6(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(4, 5);
        }

        private void Click_5x8(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(4, 7);
        }

        private void Click_6x1(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(5, 0);
        }

        private void Click_6x3(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(5, 2);
        }

        private void Click_6x5(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(5, 4);
        }

        private void Click_6x7(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(5, 6);
        }

        private void Click_7x2(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(6, 1);
        }

        private void Click_7x4(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(6, 3);
        }

        private void Click_7x6(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(6, 5);
        }

        private void Click_7x8(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(6, 7);
        }

        private void Click_8x1(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(7, 0);
        }

        private void Click_8x3(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(7, 2);
        }

        private void Click_8x5(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(7, 4);
        }

        private void Click_8x7(object sender, MouseButtonEventArgs e)
        {
            gameAdmin.calculateOptions(7, 6);
        }
    }
}
