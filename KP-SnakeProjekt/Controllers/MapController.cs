using KP_SnakeProjekt;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PSZK_MarsRoverProject.Controllers
{
    internal class MapController
    {
        public static string[,] MapMaker(MainWindow mw)
        {
            string[,] map = new string[mw.mapsize, mw.mapsize];
            for (int i = 0; i < mw.mapsize; i++)
            {
                for (int j = 0; j < mw.mapsize; j++)
                {
                    map[i, j] = ".";
                }
            }
            return map;
        }

        public static void FillUpGameSpace(MainWindow mw,string skin)
        {
            int rows = mw.map.GetLength(0);
            int cols = mw.map.GetLength(1);
            int pixelWidth = cols * mw.tileSize;
            int pixelHeight = rows * mw.tileSize;
            mw.jatekter.Width = pixelWidth;
            mw.jatekter.Height = pixelHeight;

            // Talaj kirajzolása egyetlen háttérképbe (DrawingVisual)
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        ImageSource talajSource = GetGroundImageSource(mw);
                        Rect rect = new Rect(j * mw.tileSize, i * mw.tileSize, mw.tileSize, mw.tileSize);
                        drawingContext.DrawImage(talajSource, rect);
                    }
                }
            }

            // A kirajzolt vizuális elem bitmap-pé alakítása
            RenderTargetBitmap bmp = new RenderTargetBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            Image hattekKep = new Image()
            {
                Width = pixelWidth,
                Height = pixelHeight,
                Source = bmp,
                SnapsToDevicePixels = true,
                Tag = "Background"
            };
            mw.jatekter.Children.Clear();
            Canvas.SetTop(hattekKep, 0);
            Canvas.SetLeft(hattekKep, 0);
            Canvas.SetTop(hattekKep, 0);
            Panel.SetZIndex(hattekKep, 0);
            mw.jatekter.Children.Add(hattekKep);

            // snake rajzolás
            mw.SnakeHeadImage = new Image()
            {
                Width = mw.tileSize,
                Height = mw.tileSize,
                Source = new BitmapImage(new Uri($"pack://application:,,,/img/Skins/snakehead{skin}.png")),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = mw.rotateTransform
            };
            Panel.SetZIndex(mw.SnakeHeadImage, 10);
            mw.RefreshSnakePosition();
            mw.jatekter.Children.Add(mw.SnakeHeadImage);
        }

        private static readonly Random rnd = new Random();

        public static ImageSource GetGroundImageSource(MainWindow mw)
        {
            int szam = rnd.Next(1, 7);
            switch (szam)
            {
                case 1: return mw.groundImage1;
                default: return mw.groundImage1;
            }
        }
        /// <summary>
        /// Az alma megjelenítését végző metódus, amely a megadott alma darabszám alapján véletlenszerűen helyezi el az almákat a térképen. A metódus először ellenőrzi, hogy a véletlenszerűen kiválasztott pozíció üres-e (azaz "."), majd ha igen, akkor "A"-ra változtatja a térképet, létrehoz egy Image elemet az alma képével, és elhelyezi azt a játéktéren a megfelelő koordinátákon. A metódus végén beállítja az ApplesInMap flag-et true-ra, jelezve, hogy almák vannak a térképen.
        /// </summary>
        /// <param name="applescount"></param>
        /// <param name="mw"></param>

        public static void SpawnApples(int applescount, MainWindow mw)
        {
            for (int i = 0; i < applescount; i++)
            {
                int row = rnd.Next(0, mw.mapsize);
                int col = rnd.Next(0, mw.mapsize);
                if (mw.map[row, col] == ".")
                {
                    mw.map[row, col] = "A";
                    Image almaImage = new Image()
                    {
                        Width = mw.tileSize,
                        Height = mw.tileSize,
                        Source = new BitmapImage(new Uri("pack://application:,,,/img/Consumables/appleV2.png"))
                    };
                    Panel.SetZIndex(almaImage, 5);
                    Canvas.SetLeft(almaImage, col * mw.tileSize);
                    Canvas.SetTop(almaImage, row * mw.tileSize);
                    mw.jatekter.Children.Add(almaImage);
                }
            }
            mw.ApplesInMap = true;
        }

        /// <summary>
        /// Az alma eltávolítását végző metódus, amely a megadott sor és oszlop alapján frissíti a térképet, majd megkeresi és eltávolítja a megfelelő Image elemet a játéktérről. Fontos megjegyezni, hogy az alma képeknek nincs "Background" tagjük, így ez a feltétel biztosítja, hogy csak az alma képeket vizsgáljuk, és ne távolítsuk el véletlenül a háttérképet vagy a kígyó fejét/testét. A metódus végén frissíti a térképet és eltávolítja az alma képét a játéktérről.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="mw"></param>
        public static void RemoveApple(int row, int col, MainWindow mw)
        {
            mw.map[row, col] = ".";
            foreach (UIElement elem in mw.jatekter.Children)
            {
                if (elem is Image img)
                {
                    // Az apple képeknek nincs "Background" tagjük, így ez a feltétel biztosítja, hogy csak az alma képeket vizsgáljuk
                    if (img.Tag.ToString() == "Background") continue;
                    double left = Canvas.GetLeft(img);
                    double top = Canvas.GetTop(img);
                    if ((int)(left / mw.tileSize) == col && (int)(top / mw.tileSize) == row)
                    {
                        if (img == mw.SnakeHeadImage || mw.bodyImages.Contains(img))
                            continue;
                        mw.jatekter.Children.Remove(img);
                        break;
                    }
                }
            }
        }
    }
}