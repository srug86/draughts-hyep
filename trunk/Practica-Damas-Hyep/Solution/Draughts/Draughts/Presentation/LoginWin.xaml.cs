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

namespace Draughts
{
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
            GameActions g = new GameActions(init);
            bool b = g.loginPlayer(Texboxname.Text, Texboxpwd.Text);
            if (b == true)
            {
                ConnectWin conect = new ConnectWin(init);
                conect.Show();
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                Textlogin.Text = "Usuario o contraseña incorrectos...";
                Textlogin.Foreground = Brushes.Red;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {   
            init.Visibility = Visibility.Visible;
            this.Close();
        }
	}
}