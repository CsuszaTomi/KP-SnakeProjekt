using KP_SnakeProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KP_SnakeProjekt.Controllers;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Users LoggedUser = null;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("► töltsd ki az összes mezőt!");
                return;
            }
            Users user = DatabaseController.Login(username, password);
            if (user != null)
            {
                LoggedUser = user;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                ShowError("► hibás felhasználónév vagy jelszó!");
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.ShowDialog();
        }

        private void ShowError(string msg)
        {
            txtError.Text = msg;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
