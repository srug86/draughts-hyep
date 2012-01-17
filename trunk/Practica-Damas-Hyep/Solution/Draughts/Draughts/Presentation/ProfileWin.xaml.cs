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
    /// Lógica de interacción para ProfileWin.xaml
    /// Ventana en la cual creamos un objeto de la clase player para poder jugar en red.
    /// </summary>
    public partial class ProfileWin : Window
    {
        public char PasswordChar { get; set; }
        InitWin init;
        Player p;
        String ruta;
        public ProfileWin(InitWin init)
        {
            this.init = init;
            InitializeComponent();
            ruta = "";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Image0_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Anand(IND).jpg"); }
        private void Image1_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Fischer(USA).jpg"); }
        private void Image2_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Karpov(URRS).jpg"); }
        private void Image3_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Kasimdzhanov(UZB).jpg"); }
        private void Image4_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Kasparov(RUS).jpg"); }
        private void Image5_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Kramnik(RUS).jpg"); }
        private void Image6_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Ponomariov(UCR).jpg"); }
        private void Image7_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Spassky(URRS).jpg"); }
        private void Image8_MouseUp(object sender, MouseButtonEventArgs e) { loadImage("Topalov(BUL).jpg"); }

        private void loadImage(string path)
        {
            ruta = path;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            imgProfile.Source = bitmap;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if ((Tboxuser.Text.Length == 0) || (Tboxpwd.Password.Length == 0) || (ruta.Length == 0))
            {
                Textprofile.Text = "El usuario, contraseña y/o imagen no pueden ser vacios.";
                Textprofile.Foreground = Brushes.Orange;
            }
            else
            {
                p = new Player(Tboxuser.Text, Tboxpwd.Password, ruta);
                GameAdmin g = GameAdmin.Instance;
                if (g.existPlayer(p.Name))
                {
                    g.insertPlayer(p);
                    init.Visibility = Visibility.Visible;
                    this.Close();
                }
                else
                {
                    Textprofile.Text = "Nombre ya existente en la base de datos.";
                    Textprofile.Foreground = Brushes.Red;
                }
            }
        }
    }
}
