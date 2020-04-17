using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace TicTacToeGame
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int cellWIdth;
        int[,] map = new int[3, 3] { {-1,-1,-1}, {-1,-1,-1}, {-1,-1,-1 } };
        string player = "cross";
        int checkResult;
        string displayText;
        Random rand = new Random();
        DispatcherTimer tmr;
        bool playerTurn = true;
        bool gameOver = false;
        public MainPage()
        {
            this.InitializeComponent();
            tmr = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
            tmr.Tick += Tmr_Tick;
        }

        private void Tmr_Tick(object sender, object e)
        {
            if (AvaiblePlacement() && !gameOver)
            {
                AITurn();
                playerTurn = !playerTurn;
            }
            tmr.Stop();
        }

        private void Canv_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (listView.IsEnabled)
                listView.IsEnabled = false;
            if (playerTurn && !gameOver)
            {
                playerTurn = !playerTurn;
                int x = ((int)e.GetPosition(Canv).X) / cellWIdth;
                int y = ((int)e.GetPosition(Canv).Y) / cellWIdth;

                if (map[x, y] == -1)
                {
                    DrawImage(x, y);
                }
                tmr.Start();
            }
        }

        private void DrawImage(int x, int y)
        {
            Image img = new Image();
            BitmapImage bi = new BitmapImage();
            bi.UriSource = new Uri(Canv.BaseUri, $"TicTacToe_{player}.png");
            img.Width = cellWIdth;
            img.Height = cellWIdth;
            img.Source = bi;
            img.Stretch = Stretch.Fill;
            img.Margin = new Thickness(x * cellWIdth, y * cellWIdth, 0, 0);
            Canv.Children.Add(img);
            map[x, y] = player == "cross" ? 1 : 0;
            player = player == "cross" ? "circle" : "cross";
            checkResult = Check();
            switch (checkResult)
            {
                case 0:
                    displayText = "Circles won!";
                    gameOver = true;
                    break;
                case 1:
                    displayText = "Crosses won!";
                    break;
                default:
                    displayText = $"It's {player}'s turn";
                    break;
            }
            Result.Text = displayText;
        }

        int Check() 
        {
           int toReturn=-1;
           for (int i = 0; i < map.GetUpperBound(0)+1; i++)
           {
                toReturn = CheckRow(map, i);
                if (toReturn != -1)
                    break;
                toReturn = CheckColumn(map, i);
                if (toReturn != -1)
                    break;
           }
           if (toReturn==-1)
           {
                toReturn = CheckDiagonals(map);
           }
           return toReturn;
        }
        int CheckRow(int [,] toCheck, int row)
        {
            for (int i = 0; i < toCheck.GetUpperBound(1); i++)
            {
                if (toCheck[row, i] != toCheck[row, i + 1])
                    return -1;
            }
            return toCheck[row, 0];
        }
        int CheckColumn(int[,] toCheck, int column)
        {
            for (int i = 0; i < toCheck.GetUpperBound(0); i++)
            {
                if (toCheck[i, column] != toCheck[i + 1, column])
                    return -1;
            }
            return toCheck[0, column];
        }
        int CheckDiagonals(int[,] toCheck)
        {
            for (int i = 0; i < toCheck.GetUpperBound(0); i++)
            {
                if (toCheck[i, i] != toCheck[i + 1, i + 1])
                    break;
                else if (i == 1)
                    return toCheck[i, i];
            }
            for (int i = toCheck.GetUpperBound(0); i > 0; i--)
            {
                if (toCheck[i, toCheck.GetUpperBound(0) - i] !=
                    toCheck[i - 1, toCheck.GetUpperBound(0) - i + 1])
                    break;
                else if (i == 1)
                    return toCheck[i, toCheck.GetUpperBound(0) - i];
            }
            return -1;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            cellWIdth = (int)Canv.Height / 3;
            Result.Text = "Start your game!";
        }

        bool AvaiblePlacement()
        {
            foreach (var cell in map)
            {
                if (cell == -1)
                {
                    return true;
                }
            }
            return false;
        }
        void AITurn()
        {
            int x = rand.Next(0, map.GetUpperBound(0) + 1);
            int y = rand.Next(0, map.GetUpperBound(0) + 1);
            while (map[x,y]!=-1)
            {
                x = rand.Next(0, map.GetUpperBound(0) + 1);
                y = rand.Next(0, map.GetUpperBound(0) + 1);
            }
            DrawImage(x,y);
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int user = Convert.ToInt32((sender as Image).Tag);
            player = user == 0 ? "circle" : "cross";
        }
    }
}
