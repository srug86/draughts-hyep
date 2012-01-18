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
using System.Data;
using Draughts.Domain;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para SelectWin.xaml
    /// </summary>
    public partial class SelectWin : Window
    {
        Player p1, p2;
        InitWin init;
        BoundedQueue<String> images;
        int ind, ind2;
        String ruta1, ruta2;
        public SelectWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            ind = 0;
            ind2 = 0;
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
            ruta1 = "Anand(IND).jpg";
            ruta2 = "cpu.png";
            Imagej1.Source = loadImage(ruta1);
            Imagej2.Source = loadImage(ruta2);
        }

        public BitmapImage loadImage(String path)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path, UriKind.Relative);
            bi.EndInit();
            return bi;
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

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            ruta1 = images.getNext(ind);
            if (ruta1 == "cpu.png")
            {
                ind++;
                ruta1 = images.getNext(ind);
            }
            Imagej1.Source = loadImage(ruta1);
            ind++;
        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            ruta1 = images.getPrev(ind);
            if (ruta1 == "cpu.png")
            {
                ind--;
                ruta1 = images.getNext(ind);
            }
            Imagej1.Source = loadImage(ruta1);
            ind--;
        }

        private void btnPrev2_Click(object sender, RoutedEventArgs e)
        {
            ruta2 = images.getPrev(ind2);
            Imagej2.Source = loadImage(ruta2);
            ind2--;
        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            ruta2 = images.getNext(ind2);
            Imagej2.Source = loadImage(ruta2);
            ind2++;
        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            p1 = new Player(TextboxJ1.Text, "", ruta1);
            p2 = new Player(TextboxJ2.Text, "", ruta2);
            GameActions gameAdmin = new GameActions(init, p1, p2);
            this.Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            directory = Path.Combine(directory, "../../Resources");
            var file = Path.Combine(directory, "Documentation.chm");
            Process.Start(file); 
        }
    }
}
