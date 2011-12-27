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
        Player j1, j2;
        public GameWin(InitWin init, Player j1, Player j2)
        {
            this.j1 = j1;
            this.j2 = j2;
            this.init = init;
            InitializeComponent();
            textj1.Text = j1.Name;
            textj2.Text = j2.Name;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(j1.Avatar, UriKind.Relative);
            bi.EndInit();
            imgj1.Source = bi;
            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri(j2.Avatar, UriKind.Relative);
            bi2.EndInit();
            imgj2.Source = bi2;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
