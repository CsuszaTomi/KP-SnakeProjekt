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
        public List<Image> bodyImages = new List<Image>();
        BitmapImage bodyCorner = new BitmapImage();
        BitmapImage bodyStraight = new BitmapImage();
        int eatenApples = 0;
        int bodysize = 0;
        public Users LoggedUser;
        public MainWindow()
        {
            InitializeComponent();
            map = MapController.MapMaker(this);
            groundImage1 = new BitmapImage(new Uri("pack://application:,,,/img/kep57.png"));
            bodyStraight = new BitmapImage(new Uri("pack://application:,,,/img/Skins/snakebody.png"));
            bodyCorner = new BitmapImage(new Uri("pack://application:,,,/img/Skins/snakecorner.png"));
            SnakeHead = new Snake(15, 15, 15, 14, 0, 0, false);
            snakeBody.Clear();
            snakeBody.Add(SnakeHead);
            for (int i = 1; i <= 5; i++)
            {
                snakeBody.Add(new Snake(15, 15 - i, 15, 15 - i - 1, 0, 0, true));
                bodysize++;
                txtBodyLength.Text = $"{bodysize}";
            }
            MapController.FillUpGameSpace(this);
            bodyImages.Clear();
            for (int i = 1; i < snakeBody.Count; i++)
            {
                CreateSnakeBodyPart();
            }
            visualX = SnakeHead.PosX;
            visualY = SnakeHead.PosY;
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
            int prevX = SnakeHead.PosX;
            int prevY = SnakeHead.PosY;
            switch (SnakeHead.Rotation)
            {
                case 180: SnakeHead.PosY--; break;
                case 0: SnakeHead.PosY++; break;
                case 270: SnakeHead.PosX++; break;
                case 90: SnakeHead.PosX--; break;
            }
            //A testrészek követik egymást láncban
            for (int i = 1; i < snakeBody.Count; i++)
            {
                int tempX = snakeBody[i].PosX;
                int tempY = snakeBody[i].PosY;
                snakeBody[i].PosX = prevX;
                snakeBody[i].PosY = prevY;
                prevX = tempX;
                prevY = tempY;
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
            if(map[SnakeHead.PosY, SnakeHead.PosX] == "A")
            {
                MapController.SpawnApples(1, this);
                MapController.RemoveApple(SnakeHead.PosY, SnakeHead.PosX, this);
                snakeBody.Add(new Snake(prevX, prevY, prevX, prevY, 0, 0, true));
                CreateSnakeBodyPart();
                bodysize++;
                eatenApples++;
                txtApples.Text = $"{eatenApples} db";
                txtBodyLength.Text = $"{bodysize}";
            }
            kamera.ScrollToHorizontalOffset(visualX * tileSize - (kamera.ActualWidth / 2) + tileSize / 2);
            kamera.ScrollToVerticalOffset(visualY * tileSize - (kamera.ActualHeight / 2) + tileSize / 2);
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
        public void CreateSnakeBodyPart()
        {
            RotateTransform individualTransform = new RotateTransform(0);
            Image snakeBodyPartImage = new Image()
            {
                Width = tileSize,
                Height = tileSize,
                Source = new BitmapImage(new Uri("pack://application:,,,/img/Skins/snakebody.png")),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = individualTransform
            };
            Panel.SetZIndex(snakeBodyPartImage, 9);
            bodyImages.Add(snakeBodyPartImage);
            jatekter.Children.Add(snakeBodyPartImage);
        }

        public void RefreshSnakePosition()
        {
            if (SnakeHeadImage == null) return;

            // Fej pozíciója és forgatása
            Canvas.SetLeft(SnakeHeadImage, visualX * tileSize);
            Canvas.SetTop(SnakeHeadImage, visualY * tileSize);
            rotateTransform.Angle = SnakeHead.Rotation;
            // Testrészek pozíciója és forgatása
            for (int i = 0; i < bodyImages.Count; i++)
            {
                int currentIdx = i + 1; 
                Image img = bodyImages[i];
                Snake current = snakeBody[currentIdx];
                Snake lead = snakeBody[currentIdx - 1];
                Canvas.SetLeft(img, current.PosX * tileSize);
                Canvas.SetTop(img, current.PosY * tileSize);
                RotateTransform rt = (RotateTransform)img.RenderTransform;
                // Ha van követő, akkor sarok vagy egyenes
                if (currentIdx + 1 < snakeBody.Count)
                {
                    Snake follow = snakeBody[currentIdx + 1];
                    // Irányvektorok
                    int InX = lead.PosX - current.PosX;
                    int InY = lead.PosY - current.PosY;
                    int OutX = follow.PosX - current.PosX;
                    int OutY = follow.PosY - current.PosY;
                    if (InX + OutX != 0 || InY + OutY != 0)
                    {
                        img.Source = bodyCorner;
                        rt.Angle = CalculateCornerRotation(InX, InY, OutX, OutY);
                    }
                    else
                    {
                        img.Source = bodyStraight;
                        rt.Angle = (InX != 0) ? 90 : 0; 
                    }
                }
                else
                {
                    img.Source = bodyStraight;
                    rt.Angle = CalculateBodyRotation(current, lead);
                }
            }
        }
        /// <summary>
        /// A testresz forgatását számítja ki 
        /// </summary>  
        /// <param name="current">A jelenlegi testresz</param>
        /// <param name="lead">A vezető testresz</param>
        /// <returns>A forgatási szög</returns>
        private double CalculateBodyRotation(Snake current, Snake lead)
        {
            if (lead.PosX > current.PosX) return 270; // Jobbra
            if (lead.PosX < current.PosX) return 90;  // Balra
            if (lead.PosY > current.PosY) return 0;   // Le
            if (lead.PosY < current.PosY) return 180; // Fel
            return 0;
        }

        /// <summary>
        /// A sarok forgatását számítja ki az irányvektorok alapján
        /// </summary>
        /// <param name="InX">A bejövő delta x</param>
        /// <param name="InY">A bejövő delta y</param>
        /// <param name="OutX">A kimenő delta x</param>
        /// <param name="OutY">A kimenő delta y</param>
        /// <returns>A forgatási szög</returns>
        private double CalculateCornerRotation(int InX, int InY, int OutX, int OutY)
        {
            // Irányvektorok alapján meghatározzuk a sarok forgatását
            if ((InY == -1 && OutX == 1) || (InX == 1 && OutY == -1)) return 0;   // Bal-Felső sarok
            if ((InX == 1 && OutY == 1) || (InY == 1 && OutX == 1)) return 90;  // Bal-Alsó sarok
            if ((InY == 1 && OutX == -1) || (InX == -1 && OutY == 1)) return 180; // Jobb-Alsó sarok
            if ((InX == -1 && OutY == -1) || (InY == -1 && OutX == -1)) return 270; // Jobb-Felső sarok
            return 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kamera.ScrollToVerticalOffset(SnakeHead.PosY * tileSize - (kamera.ActualHeight / 2));
            kamera.ScrollToHorizontalOffset(SnakeHead.PosX * tileSize - (kamera.ActualWidth / 2));
            MessageBox.Show($"{LoggedUser.UserName}");
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