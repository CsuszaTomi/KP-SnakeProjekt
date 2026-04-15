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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public Users LoggedUser;
        public MainMenu()
        {
            InitializeComponent();
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedUser == null)
            {
                Login loginWindow = new Login();
                loginWindow.ShowDialog();
                if (loginWindow.LoggedUser != null)
                {
                    LoggedUser = loginWindow.LoggedUser;
                    txtUsername.Text = LoggedUser.UserName;
                    int maxScore = DatabaseController.GetMaxScore(LoggedUser.Id);
                    txtMaxScore.Text = $"Rekord: {maxScore} pont";
                }
            }
            else
            {
                // Ha már be van jelentkezve, kijelentkezés
                MessageBoxResult result = MessageBox.Show(
                    $"Kijelentkezel, {LoggedUser.UserName}?",
                    "Kijelentkezés",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    LoggedUser = null;
                    txtUsername.Text = "Vendég";
                    txtMaxScore.Text = "";
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(LoggedUser != null)
            {
                txtUsername.Text = LoggedUser.UserName;
                int maxScore = DatabaseController.GetMaxScore(LoggedUser.Id);
                txtMaxScore.Text = $"Rekord: {maxScore} pont";
            }
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
            //if (LoggedUser == null)
            //{
            //    MessageBox.Show("Jelentkezz be a játékhoz!", "Snake");
            //    return;
            //}
            MainWindow game = new MainWindow();
            game.LoggedUser = LoggedUser;
            game.Show();
            this.Close();
        }

        private void btnSkins_Click(object sender, RoutedEventArgs e)
        {
            Skins skinswindow = new Skins();
            skinswindow.loggedUser = LoggedUser;
            skinswindow.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
