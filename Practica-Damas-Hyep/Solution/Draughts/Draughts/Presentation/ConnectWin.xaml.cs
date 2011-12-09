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
    /// Lógica de interacción para ConnectWin.xaml
    /// </summary>
    public partial class ConnectWin : Window
    {
        InitWin init;

        public ConnectWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
