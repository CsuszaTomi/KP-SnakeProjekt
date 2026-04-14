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
    /// Interaction logic for Skins.xaml
    /// </summary>
    public partial class Skins : Window
    {
        public static string SelectedSkin = "default";
        private string tempSelected = "default";
        // A skin nevének és a hozzá tartozó Border elemnek a tárolására szolgáló szótár
        private Dictionary<string, Border> skinBorders;
        public Skins()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            skinBorders = new Dictionary<string, Border>
            {
                { "default", borderDefault },
                { "Bl",  borderBl  },
                { "Pi",  borderPi  },
                { "Pu",  borderPu  },
                { "Red", borderRed },
                { "Ye",  borderYe  },
                { "YG",  borderYG  }
            };
            tempSelected = SelectedSkin;
            HighlightSelected(tempSelected);
        }

        private void SkinButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button)?.Tag?.ToString();
            if (tag == null) 
                return;
            tempSelected = tag;
            HighlightSelected(tag);
        }

        private void HighlightSelected(string tag)
        {
            foreach (var kv in skinBorders)
            {
                kv.Value.BorderBrush = kv.Key == tag ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4ade80"))
                                                     : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f2937"));
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectedSkin = tempSelected;
            MessageBox.Show($"Skin elmentve: {SelectedSkin}", "Skinek");
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
