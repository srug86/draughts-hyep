﻿using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Draughts.Presentation;
using Draughts.Domain;
using System.Reflection;
using System.Diagnostics;

namespace Draughts
{
    /// <summary>
    /// Lógica de interacción para LoginWin.xaml
    /// Ventana de acceso para poder jugar en red.
    /// </summary>
	public partial class LoginWin
	{
        InitWin init;
		public LoginWin(InitWin init)
		{
            this.init = init;
			InitializeComponent();
			// Insert code required on object creation below this point.
		}

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            init.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if ((Texboxname.Text.Length == 0) || (Texboxpwd.Password.Length == 0))
            {
                Textlogin.Text = "El usuario y/o la contraseña son vacios.";
                Textlogin.Foreground = Brushes.Orange;
            }
            else
            {
                GameAdmin g = GameAdmin.Instance;
                bool b = g.loginPlayer(Texboxname.Text, Texboxpwd.Password);
                if (b == true)
                {
                    ConnectWin conect = new ConnectWin(init, g);
                    conect.Show();
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    Textlogin.Text = "Usuario o contraseña incorrectos...";
                    Textlogin.Foreground = Brushes.Red;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {   
            init.Visibility = Visibility.Visible;
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