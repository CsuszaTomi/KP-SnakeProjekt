using KP_SnakeProjekt.Controllers;
using KP_SnakeProjekt.Models;
using KP_SnakeProjekt.View;
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
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Password;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    UIHandler.ShowError("► Töltsd ki az összes mezőt!", txtError);
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
                    UIHandler.ShowError("► Hibás felhasználónév vagy jelszó!", txtError);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                UIHandler.ShowError("► Nem sikerült csatlakozni az adatbázishoz!", txtError);
            }
            catch (Exception ex)
            {
                UIHandler.ShowError($"► Hiba történt: {ex.Message}", txtError);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.ShowDialog();
        }
    }
}
