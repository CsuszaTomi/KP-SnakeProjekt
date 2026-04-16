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
using KP_SnakeProjekt.Controllers;
using KP_SnakeProjekt.Models;
using PSZK_MarsRoverProject.Controllers;
using KP_SnakeProjekt.View;

namespace KP_SnakeProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int tileSize = 120;
        public string[,] map;
        public int mapsize = 10;
        public BitmapImage groundImage1;
        public Image SnakeHeadImage;
        public DispatcherTimer simTimer;
        public DispatcherTimer renderTimer;
        public DispatcherTimer timeTimer;
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
        BitmapImage bodyTail = new BitmapImage();
        int eatenApples = 0;
        int bodysize = 0;
        public Users LoggedUser;
        public string[,] snakeBodyMap;
        public int timePassed = 0;
        int countdownValue = 2;
        public DispatcherTimer countdownTimer;
        public MainWindow()
        {
            InitializeComponent();
            string skin = Skins.SelectedSkin == "default" ? "" : Skins.SelectedSkin;
            mapsize = Settings.SelectedMapSize;
            map = MapController.MapMaker(this);
            groundImage1 = new BitmapImage(new Uri("pack://application:,,,/img/ground.png"));
            bodyStraight = new BitmapImage(new Uri($"pack://application:,,,/img/Skins/snakebody{skin}.png"));
            bodyCorner = new BitmapImage(new Uri($"pack://application:,,,/img/Skins/snakecorner{skin}.png"));
            bodyTail = new BitmapImage(new Uri($"pack://application:,,,/img/Skins/snaketail{skin}.png"));
            int snakeLocDisplace = GetSnakeStartDisplacement();
            SnakeHead = new Snake(snakeLocDisplace, snakeLocDisplace, snakeLocDisplace, snakeLocDisplace, 0, 0, false);
            snakeBody.Clear();
            snakeBody.Add(SnakeHead);
            snakeBodyMap = MapController.MapMaker(this);
            for (int i = 1; i <= 5; i++)
            {
                snakeBody.Add(new Snake(snakeLocDisplace, snakeLocDisplace - i, snakeLocDisplace, snakeLocDisplace - i - 1, 0, 0, true));
                bodysize++;
                txtBodyLength.Text = $"{bodysize}";
            }
            MapController.FillUpGameSpace(this,skin);
            bodyImages.Clear();
            for (int i = 1; i < snakeBody.Count; i++)
            {
                CreateSnakeBodyPart();
            }
            visualX = SnakeHead.PosX;
            visualY = SnakeHead.PosY;
            RefreshSnakePosition();
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();
            //A szimulációs időzítő, ami a kígyó logikáját futtatja, és a játék fő ciklusát jelenti
            simTimer = new DispatcherTimer();
            simTimer.Interval = TimeSpan.FromMilliseconds(500);
            simTimer.Tick += SimTimer_Tick;
            //A renderelő időzítő, ami a kígyó mozgását simává teszi
            renderTimer = new DispatcherTimer();
            renderTimer.Interval = TimeSpan.FromMilliseconds(13);
            renderTimer.Tick += RenderTimer_Tick;
            //A játékidő számlálója
            timeTimer = new DispatcherTimer();
            timeTimer.Interval = TimeSpan.FromSeconds(1);
            timeTimer.Tick += TimeTimer_Tick;

        }

        /// <summary>
        /// A játék indítása előtti visszaszámlálást kezelő időzítő eseménykezelője, ami minden másodpercben csökkenti a visszaszámláló értékét, és amikor eléri a 0-t, elindítja a szimulációs, renderelő és idő számláló időzítőket, valamint elrejti a visszaszámláló overlayt. Ez lehetőséget ad a játékosnak, hogy felkészüljön a játék kezdetére.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (countdownValue > 0)
            {
                txtCountdown.Text = countdownValue.ToString();
                countdownValue--;
            }
            else
            {
                countdownTimer.Stop();
                simTimer.Start();
                renderTimer.Start();
                timeTimer.Start();
                GameStartOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            timePassed++;
            txtTime.Text = $"{GetPassedTime()}";
        }

        private int GetSnakeStartDisplacement()
        {
            return mapsize / 2;
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
                EndGame();
                return;
            }
            if (snakeBodyMap[SnakeHead.PosY, SnakeHead.PosX] == "S")
            {
                EndGame();
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
            txtPos.Text = $"X: {SnakeHead.PosX} Y: {SnakeHead.PosY}";
            SetBodyPositionOnMap();
        }
        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            visualX += (SnakeHead.PosX - visualX) * 0.2;
            visualY += (SnakeHead.PosY - visualY) * 0.2;

            ClampVisualPosition();
            RefreshSnakePosition();
        }

        private void EndGame()
        {
            simTimer.Stop();
            renderTimer.Stop();
            timeTimer.Stop();
            //GameOverOverlay.Visibility = Visibility.Visible;
            if (LoggedUser != null)
            {
                try
                {
                    DatabaseController.AddScore(LoggedUser.Id, eatenApples);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Hiba a pontszám mentésekor: {ex.Message}");
                }
            }
            GOAlmak.Text = $"{eatenApples} db";
            GOTesthossz.Text = $"{bodysize}";
            GOIdo.Text = GetPassedTime();
            GameOverOverlay.Visibility = Visibility.Visible;
        }

        private string GetPassedTime()
        {
            int minutes = timePassed / 60;
            int seconds = timePassed % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        /// <summary>
        /// A kígyó testének pozícióját frissítő metódus, ami először kitölti a snakeBodyMap tömböt pontokkal, majd a kígyó minden testrészének pozícióját "S"-sel jelöli a tömbben. Ez a tömb szolgál a kígyó testének ütközésdetektálásához, mivel ha a fej pozíciója egy "S"-re lép, akkor az játék véget ér. Ez a megközelítés lehetővé teszi az egyszerű és gyors ütközésdetektálást anélkül, hogy minden testrészt külön-külön kellene ellenőrizni.
        /// </summary>
        private void SetBodyPositionOnMap()
        {
            for (int i = 0; i < mapsize; i++)
            {
                for(int j = 0; j < mapsize; j++)
                {
                    snakeBodyMap[i,j] = ".";
                }
            }
            for (int i = 1; i < snakeBody.Count; i++)
            {
                Snake part = snakeBody[i];
                snakeBodyMap[part.PosY, part.PosX] = "S";
            }
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
                        rt.Angle = SnakeView.CalculateCornerRotation(InX, InY, OutX, OutY);
                    }
                    else
                    {
                        img.Source = bodyStraight;
                        rt.Angle = (InX != 0) ? 90 : 0; 
                    }
                }
                else
                {
                    img.Source = bodyTail;
                    rt.Angle = SnakeView.CalculateBodyRotation(current, lead);
                }

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kamera.ScrollToVerticalOffset(SnakeHead.PosY * tileSize - (kamera.ActualHeight / 2));
            kamera.ScrollToHorizontalOffset(SnakeHead.PosX * tileSize - (kamera.ActualWidth / 2));
            //MessageBox.Show($"{LoggedUser.UserName}");
            txtPlayer.Text = LoggedUser?.UserName ?? "Vendég";
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

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newGame = new MainWindow();
            newGame.LoggedUser = LoggedUser;
            newGame.Show();
            this.Close();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.LoggedUser = LoggedUser;
            menu.Show();
            this.Close();
        }
    }
}