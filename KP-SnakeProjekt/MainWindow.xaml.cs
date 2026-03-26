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
        public DispatcherTimer renderTimer;
        double visualX;
        double visualY;
        double stepX = 0;
        double stepY = 0;
        Snake SnakeHead = new Snake();
        List<Snake> snakeBody = new List<Snake>();
        public RotateTransform rotateTransform = new RotateTransform(0);
        Random rnd = new Random();
        int maxalmaszam = 10;
        public bool ApplesInMap = false;
        public MainWindow()
        {
            map = MapController.MapMaker(this);
            InitializeComponent();
            SnakeHead = new Snake(15, 15, 0, 0, 0, 0, false);
            snakeBody.Add(SnakeHead);
            for (int i = 1; i <= 5; i++)
            {
                int tailPosX = SnakeHead.PosX;
                int tailPosY = SnakeHead.PosY - i; 
                int tailLastPosY = tailPosY - 1;  
                snakeBody.Add(new Snake(tailPosX, tailPosY, tailPosX, tailLastPosY, 0, 0, true));
            }
            groundImage1 = new BitmapImage(new Uri("pack://application:,,,/img/kep57.png"));
            simTimer = new DispatcherTimer();
            visualX = SnakeHead.PosX;
            visualY = SnakeHead.PosY;   
            MapController.FillUpGameSpace(this);
            RefreshSnakePosition();
            simTimer = new DispatcherTimer();
            simTimer.Interval = TimeSpan.FromMilliseconds(500);
            simTimer.Tick += SimTimer_Tick; 
            simTimer.Start();
            renderTimer = new DispatcherTimer();
            renderTimer.Interval = TimeSpan.FromMilliseconds(13);
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void SimTimer_Tick(object sender, EventArgs e)
        {
            SnakeHead.LastPosX = SnakeHead.PosX;
            SnakeHead.LastPosY = SnakeHead.PosY;

            switch (SnakeHead.Rotation)
            {
                case 180:
                    SnakeHead.PosY--;
                    break;
                case 0:
                    SnakeHead.PosY++;
                    break;
                case 270:
                    SnakeHead.PosX++;
                    break;
                case 90:
                    SnakeHead.PosX--;
                    break;
            }
            foreach (var bodyPart in snakeBody.Skip(1))
            {
                bodyPart.LastPosX = bodyPart.PosX;
                bodyPart.LastPosY = bodyPart.PosY;
                bodyPart.PosX = SnakeHead.LastPosX;
                bodyPart.PosY = SnakeHead.LastPosY;
            }
            if (SnakeHead.PosX < 0 || SnakeHead.PosX >= mapsize || SnakeHead.PosY < 0 || SnakeHead.PosY >= mapsize)
            {
                simTimer.Stop();
                renderTimer.Stop();
                MessageBox.Show("hékabéka");
                return;
            }
            int almaszam = rnd.Next(1, maxalmaszam);
            if (!ApplesInMap)
                MapController.SpawnApples(almaszam, this);
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            visualX += (SnakeHead.PosX - visualX) * 0.2;
            visualY += (SnakeHead.PosY - visualY) * 0.2;

            ClampVisualPosition();
            RefreshSnakePosition();
        }
        public void ClampVisualPosition()
        {
            visualX = Math.Max(0, Math.Min(mapsize - 1, visualX));
            visualY = Math.Max(0, Math.Min(mapsize - 1, visualY));
        }
        public void RefreshSnakePosition()
        {
            Canvas.SetLeft(SnakeHeadImage, visualX * tileSize);
            Canvas.SetTop(SnakeHeadImage, visualY * tileSize);
            txtPos.Text = $"X: {SnakeHead.PosX+1} Y: {SnakeHead.PosY+1} Direction: {SnakeHead.GetDirection()}";
            rotateTransform.Angle = SnakeHead.Rotation;
            kamera.ScrollToHorizontalOffset(visualX * tileSize - (kamera.ActualWidth / 2));
            kamera.ScrollToVerticalOffset(visualY * tileSize - (kamera.ActualHeight / 2));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kamera.ScrollToVerticalOffset(SnakeHead.PosY * tileSize - (kamera.ActualHeight / 2));
            kamera.ScrollToHorizontalOffset(SnakeHead.PosX * tileSize - (kamera.ActualWidth / 2));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    if (SnakeHead.Rotation != 0)
                        SnakeHead.Rotation = 180;
                    break;
                case Key.S:
                    if (SnakeHead.Rotation != 180)
                        SnakeHead.Rotation = 0;
                    break;
                case Key.D:
                    if (SnakeHead.Rotation != 90)
                        SnakeHead.Rotation = 270;
                    break;
                case Key.A:
                    if (SnakeHead.Rotation != 270)
                        SnakeHead.Rotation = 90;
                    break;
                case Key.Space:
                    kamera.ScrollToVerticalOffset(SnakeHead.PosY * tileSize - (kamera.ActualHeight / 2));
                    kamera.ScrollToHorizontalOffset(SnakeHead.PosX * tileSize - (kamera.ActualWidth / 2));
                    break;
            }
            RefreshSnakePosition();
        }
    }
}