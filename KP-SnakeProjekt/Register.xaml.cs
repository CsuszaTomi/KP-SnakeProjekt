using KP_SnakeProjekt.Controllers;
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

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        List<Users> users = new List<Users>();
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            string passwordConfirm = txtPasswordConfirm.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirm))
            {
                ShowError("► töltsd ki az összes mezőt!");
                return;
            }
            if (password != passwordConfirm)
            {
                ShowError("► a két jelszó nem egyezik!");
                return;
            }
            if (username.Length < 3)
            {
                ShowError("► a felhasználónév legalább 3 karakter legyen!");
                return;
            }
            if (password.Length < 6)
            {
                ShowError("► a jelszó legalább 6 karakter legyen!");
                return;
            }
            if (!DatabaseController.Register(username, password))
            {
                ShowError("► A felhasználónév már foglalt!");
                return;
            }
            MessageBox.Show($"► sikeres regisztráció, {username}!\nMost már bejelentkezhetsz.", "Siker", MessageBoxButton.OK);
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowError(string msg)
        {
            txtError.Text = msg;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
