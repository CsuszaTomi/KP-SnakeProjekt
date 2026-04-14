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
using KP_SnakeProjekt.View;

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
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Password;
                string passwordConfirm = txtPasswordConfirm.Password;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirm))
                {
                    UIHandler.ShowError("► Töltsd ki az összes mezőt!", txtError);
                    return;
                }
                if (password != passwordConfirm)
                {
                    UIHandler.ShowError("► A két jelszó nem egyezik!", txtError);
                    return;
                }
                if (username.Length < 3)
                {
                    UIHandler.ShowError("► A felhasználónév legalább 3 karakter legyen!", txtError);
                    return;
                }
                if (password.Length < 6)
                {
                    UIHandler.ShowError("► A jelszó legalább 6 karakter legyen!", txtError);
                    return;
                }
                if (!DatabaseController.Register(username, password))
                {
                    UIHandler.ShowError("► A felhasználónév már foglalt!", txtError);
                    return;
                }
                //MessageBox.Show($"► sikeres regisztráció, {username}!\nMost már bejelentkezhetsz.", "Siker", MessageBoxButton.OK);
                this.Close();
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
