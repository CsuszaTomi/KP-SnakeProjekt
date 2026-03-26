using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using KP_SnakeProjekt.Models;
using PSZK_MarsRoverProject.Controllers;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int tileSize = 120;
        public string[,] map;
        public int mapsize = 20;
        public BitmapImage groundImage1;
        public Image SnakeHeadImage;
        public DispatcherTimer simTimer;
        double visualX;
        double visualY;
        double stepX = 0;
        double stepY = 0;
        Snake snake = new Snake();
        public MainWindow()
        {
            map = MapController.MapMaker(this);
            InitializeComponent();
            snake = new Snake(0, 0, 0, 0, 0);
            groundImage1 = new BitmapImage(new Uri("pack://application:,,,/img/kep57.png"));
            simTimer = new DispatcherTimer();
            visualX = snake.PosX;
            visualY = snake.PosY;
            MapController.FillUpGameSpace(this);
            RefreshSnakePosition();
            //simTimer.Interval = TimeSpan.FromSeconds(Time.TimeRate);
            //simTimer.Tick += SimTimer_Tick;
        }
        public void RefreshSnakePosition()
        {
            // Itt a logikai rover.Xposition helyett a folyamatosan változó visualX-et használjuk!
            Canvas.SetLeft(SnakeHeadImage, visualX * tileSize);
            Canvas.SetTop(SnakeHeadImage, visualY * tileSize);
            //txtPos.Text = $"X: {snake.PosX}, Y: {snake.PosY}";
            //if (FollowRoverBox.IsChecked == true)
            //{
            //    kamera.ScrollToVerticalOffset(visualY * tileSize - (kamera.ActualHeight / 2));
            //    kamera.ScrollToHorizontalOffset(visualX * tileSize - (kamera.ActualWidth / 2));
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kamera.ScrollToVerticalOffset(snake.PosY * tileSize - (kamera.ActualHeight / 2));
            kamera.ScrollToHorizontalOffset(snake.PosX * tileSize - (kamera.ActualWidth / 2));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    kamera.ScrollToVerticalOffset(kamera.VerticalOffset - tileSize);
                    break;
                case Key.S:
                    kamera.ScrollToVerticalOffset(kamera.VerticalOffset + tileSize);
                    break;
                case Key.A:
                    kamera.ScrollToHorizontalOffset(kamera.HorizontalOffset - tileSize);
                    break;
                case Key.D:
                    kamera.ScrollToHorizontalOffset(kamera.HorizontalOffset + tileSize);
                    break;
                case Key.Space:
                    kamera.ScrollToVerticalOffset(snake.PosY * tileSize - (kamera.ActualHeight / 2));
                    kamera.ScrollToHorizontalOffset(snake.PosX * tileSize - (kamera.ActualWidth / 2));
                    break;
            }
        }
    }
}