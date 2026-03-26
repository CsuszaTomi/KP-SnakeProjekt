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

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int tileSize = 40;
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
            InitializeComponent();
            snake = new Snake(0,0,0,0,0);
            groundImage1 = new BitmapImage(new Uri("pack://application:,,,/Images/kep57.png"));
            simTimer = new DispatcherTimer();
            visualX = snake.PosX;
            visualY = snake.PosY;
            //simTimer.Interval = TimeSpan.FromSeconds(Time.TimeRate);
            //simTimer.Tick += SimTimer_Tick;
        }
    }
}