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
    public partial class GameWin : Window, Observer
    {
        private InitWin init;
        private Player pl1, pl2;
        private GameAdmin gameAdmin;
        private Dictionary<int,string> colorBox;
        private Image [,] table;

        public GameWin(InitWin init, Subject subject, GameAdmin gameAdmin)
        {
            this.gameAdmin = gameAdmin;
            this.pl1 = gameAdmin.Pl1;
            this.pl2 = gameAdmin.Pl2;
            this.init = init;
            InitializeComponent();
            register(subject);
            textPl1.Text = pl1.Name;
            textPl2.Text = pl2.Name;
            BitmapImage bi1 = new BitmapImage();
            bi1.BeginInit();
            bi1.UriSource = new Uri(pl1.Avatar, UriKind.Relative);
            bi1.EndInit();
            imgPl1.Source = bi1;
            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri(pl2.Avatar, UriKind.Relative);
            bi2.EndInit();
            imgPl2.Source = bi2;
            this.table = new Image [8,8] {
            {img1x1, img1x2, img1x3, img1x4, img1x5, img1x6, img1x7, img1x8},
            {img2x1, img2x2, img2x3, img2x4, img2x5, img2x6, img2x7, img2x8},
            {img3x1, img3x2, img3x3, img3x4, img3x5, img3x6, img3x7, img3x8},
            {img4x1, img4x2, img4x3, img4x4, img4x5, img4x6, img4x7, img4x8},
            {img5x1, img5x2, img5x3, img5x4, img5x5, img5x6, img5x7, img5x8},
            {img6x1, img6x2, img6x3, img6x4, img6x5, img6x6, img6x7, img6x8},
            {img7x1, img7x2, img7x3, img7x4, img7x5, img7x6, img7x7, img7x8},
            {img8x1, img8x2, img8x3, img8x4, img8x5, img8x6, img8x7, img8x8}};
        }

        private void register(Subject subject)
        {
            subject.registerInterest(this);
            // Crea el Dictionary
            colorBox = new Dictionary<int,string>();
            colorBox.Add(0, "empty.jpg");       // Casilla vacía
            colorBox.Add(1, "red.jpg");         // Ficha roja
            colorBox.Add(2, "white.jpg");       // Ficha blanca
            colorBox.Add(11, "redQ.jpg");       // Reina roja
            colorBox.Add(22, "whiteQ.jpg");     // Reina blanca
            colorBox.Add(111, "onRed.jpg");     // Ficha roja señalada
            colorBox.Add(222, "onWhite.jpg");   // Ficha blanca señalada
            colorBox.Add(1111, "onRedQ.jpg");   // Reina roja señalada
            colorBox.Add(2222, "onWhiteQ.jpg"); // Reina blanca señalada
            colorBox.Add(11111, "move.jpg");    // Posible movimiento ficha roja
            colorBox.Add(22222, "move.jpg");    // Posible movimiento ficha blanca
            colorBox.Add(111111, "move.jpg");   // Posible movimiento reina roja
            colorBox.Add(222222, "move.jpg");   // Posible movimeinto reina blanca
        }

        // Modifica el contenido de la casilla al recibir una notificación
        public void notify(int row, int column, int state)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri((String)colorBox[state], UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.table[row,column].Source = bi;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Click_1x2(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(0, 1);
        }

        private void Click_1x4(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(0, 3);
        }

        private void Click_1x6(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(0, 5);
        }

        private void Click_1x8(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(0, 7);
        }

        private void Click_2x1(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(1, 0);
        }

        private void Click_2x3(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(1, 2);
        }

        private void Click_2x5(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(1, 4);
        }

        private void Click_2x7(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(1, 6);
        }

        private void Click_3x2(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(2, 1);
        }

        private void Click_3x4(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(2, 3);
        }

        private void Click_3x6(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(2, 5);
        }

        private void Click_3x8(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(2, 7);
        }

        private void Click_4x1(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(3, 0);
        }

        private void Click_4x3(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(3, 2);
        }

        private void Click_4x5(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(3, 4);
        }

        private void Click_4x7(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(3, 6);
        }

        private void Click_5x2(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(4, 1);
        }

        private void Click_5x4(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(4, 3);
        }

        private void Click_5x6(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(4, 5);
        }

        private void Click_5x8(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(4, 7);
        }

        private void Click_6x1(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(5, 0);
        }

        private void Click_6x3(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(5, 2);
        }

        private void Click_6x5(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(5, 4);
        }

        private void Click_6x7(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(5, 6);
        }

        private void Click_7x2(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(6, 1);
        }

        private void Click_7x4(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(6, 3);
        }

        private void Click_7x6(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(6, 5);
        }

        private void Click_7x8(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(6, 7);
        }

        private void Click_8x1(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(7, 0);
        }

        private void Click_8x3(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(7, 2);
        }

        private void Click_8x5(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(7, 4);
        }

        private void Click_8x7(object sender, MouseButtonEventArgs e)
        {
            this.gameAdmin.selectedBox(7, 6);
        }
    }
}
