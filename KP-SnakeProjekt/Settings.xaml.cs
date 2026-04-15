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
        public static int SelectedMapSize = 20;
        private int tempSize = 20;

        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tempSize = SelectedMapSize;
            HighlightSelected(tempSize);
        }

        private void MapSizeButton_Click(object sender, RoutedEventArgs e)
        {
            int tag = int.Parse((sender as Button).Tag.ToString());
            tempSize = tag;
            HighlightSelected(tag);
        }

        private void HighlightSelected(int size)
        {
            foreach (var border in sizePanel.Children.OfType<Button>().Select(button => button.Content as Border).Where(button => button != null))
            {
                int borderTag = int.Parse((border.Parent as Button)?.Tag?.ToString() ?? "0");
                border.BorderBrush = borderTag == size
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a78bfa"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f2937"));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SelectedMapSize = tempSize;
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}