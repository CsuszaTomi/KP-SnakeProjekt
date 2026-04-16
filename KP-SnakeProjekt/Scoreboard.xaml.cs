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
using KP_SnakeProjekt.Models;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for Scoreboard.xaml
    /// </summary>
    public partial class Scoreboard : Window
    {
        public Scoreboard()
        {
            InitializeComponent();
            LoadScores();
        }

        private void LoadScores()
        {
            List<Score> scores = DatabaseController.GetTopScores();
            for (int i = 0; i < scores.Count; i++)
            {
                scores[i].Place = $"{i + 1}.";
                if (i == 0) scores[i].Place = "🥇";
                else if (i == 1) scores[i].Place = "🥈";
                else if (i == 2) scores[i].Place = "🥉";
            }
            lstScores.ItemsSource = scores;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
