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
using KP_SnakeProjekt.Models;
using KP_SnakeProjekt.Controllers;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for Skins.xaml
    /// </summary>
    public partial class Skins : Window
    {
        public static string SelectedSkin = "default";
        private string tempSelected = "default";
        public Users loggedUser;
        public Skins()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tempSelected = SelectedSkin;
            HighlightSelected(tempSelected);
        }

        private void SkinButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();//A kiválasztott gomb Tag értékének lekérése
            if (tag == null) 
                return;
            if(tag == "RBW")
            {
                if(loggedUser != null){
                    if (DatabaseController.GetMaxScore(loggedUser.Id) < 10)
                    {
                        MessageBox.Show("A szivárvány skinhez legalább 10 pont szükséges!", "Skin zárolva", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Jelentkezz be a fiókodba, hogy láthasd a pontjaidat!", "Skin zárolva", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            tempSelected = tag;//A kiválasztott skin ideiglenes tárolása
            HighlightSelected(tag);//A kiválasztott skin kiemelése a UI-ban
        }

        private void HighlightSelected(string tag)
        {
            // Végigmegy az összes Border-en a skinPanel-en belül
            foreach (var border in skinPanel.Children.OfType<Button>().Select(b => b.Content as Border).Where(b => b != null))
            {
                string borderTag = (border.Parent as Button)?.Tag?.ToString();
                border.BorderBrush = borderTag == tag ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4ade80"))
                                                      : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f2937"));
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectedSkin = tempSelected;
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
