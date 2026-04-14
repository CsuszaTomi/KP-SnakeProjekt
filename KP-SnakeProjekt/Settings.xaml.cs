using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public static int SelectedMapSize = 20; // alapértelmezett
        private int tempSize = 20;

        private Dictionary<int, Border> sizeBorders;

        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sizeBorders = new Dictionary<int, Border>
            {
                { 10, border10 },
                { 15, border15 },
                { 20, border20 }
            };

            tempSize = SelectedMapSize;
            HighlightSelected(tempSize);
        }

        private void MapSizeButton_Click(object sender, RoutedEventArgs e)
        {
            int tag = int.Parse((sender as Button)?.Tag?.ToString());
            tempSize = tag;
            HighlightSelected(tag);
        }

        private void HighlightSelected(int size)
        {
            foreach (var kv in sizeBorders)
            {
                kv.Value.BorderBrush = kv.Key == size ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a78bfa"))
                                                      : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f2937"));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SelectedMapSize = tempSize;
            //MessageBox.Show($"Beállítások elmentve!", "Beállítások");
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}