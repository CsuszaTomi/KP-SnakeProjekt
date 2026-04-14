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

        public static void RemoveApple(int row, int col, MainWindow mw)
        {
            mw.map[row, col] = ".";
            foreach (UIElement elem in mw.jatekter.Children)
            {
                if (elem is Image img)
                {
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