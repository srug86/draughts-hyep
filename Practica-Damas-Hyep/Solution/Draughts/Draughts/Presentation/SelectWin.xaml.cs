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
        BoundedQueue<String> images;
        int ind;
        public SelectWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            ind = 0;
            images = new BoundedQueue<String>(10);
            images.enqueue("Anand(IND).jpg");
            images.enqueue("Fischer(USA).jpg");
            images.enqueue("Karpov(URRS).jpg");
            images.enqueue("Kasimdzhanov(UZB).jpg");
            images.enqueue("Kasparov(RUS).jpg");
            images.enqueue("Kramnik(RUS).jpg");
            images.enqueue("Ponomariov(UCR).jpg");
            images.enqueue("Spassky(URRS).jpg");
            images.enqueue("Topalov(BUL).jpg");
            images.enqueue("cpu.png");
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
        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            String ruta = images.getNext(ind);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(ruta, UriKind.Relative);
            bi.EndInit();
            Imagej1.Source = bi;
            ind++;
        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            String ruta = images.getPrev(ind);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(ruta, UriKind.Relative);
            bi.EndInit();
            Imagej1.Source = bi;
            ind--;
        }

        private void btnPrev2_Click(object sender, RoutedEventArgs e)
        {
            String ruta = images.getPrev(ind);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(ruta, UriKind.Relative);
            bi.EndInit();
            Imagej2.Source = bi;
            ind--;
        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            String ruta = images.getNext(ind);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(ruta, UriKind.Relative);
            bi.EndInit();
            Imagej2.Source = bi;
            ind++;
        }

    }
}
