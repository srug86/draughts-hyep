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
using System.Data;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para SelectWin.xaml
    /// </summary>
    public partial class SelectWin : Window
    {
        InitWin init;
        public SelectWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            string file = @"c:\data.xml";
            DataSet ds = new DataSet("Table");
            ds.ReadXml(file);
            //DG1.DataSource = ds.Tables[0].DefaultView;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            GameWin game = new GameWin(init);
            game.Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
