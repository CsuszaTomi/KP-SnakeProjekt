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
        public static string[,] CsvReader(MainWindow mw)
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

        public static void FillUpGameSpace(MainWindow mw)
        {
            int rows = mw.map.GetLength(0);
            int cols = mw.map.GetLength(1);
            int pixelWidth = cols * mw.tileSize;
            int pixelHeight = rows * mw.tileSize;

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
                SnapsToDevicePixels = true
            };
            Canvas.SetLeft(hattekKep, 0);
            Canvas.SetTop(hattekKep, 0);
            Panel.SetZIndex(hattekKep, 0);
            mw.jatekter.Children.Add(hattekKep);

            // Rover rajzolás
            mw.SnakeHeadImage = new Image()
            {
                Width = mw.tileSize,
                Height = mw.tileSize,
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/snakehead.png")),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(0)
            };
            Panel.SetZIndex(mw.SnakeHeadImage, 10);
            //mw.RefreshRoverPosition();
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
    }
}