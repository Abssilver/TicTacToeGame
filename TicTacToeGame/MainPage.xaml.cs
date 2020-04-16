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
        int ceilWIdth;
        int[,] map = new int[3, 3] { {-1,-1,-1}, {-1,-1,-1}, {-1,-1,-1 } };
        string player = "cross";
        int checkResult;
        string displayText;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Canv_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int x = ((int)e.GetPosition(Canv).X) / ceilWIdth;
            int y = ((int)e.GetPosition(Canv).Y) / ceilWIdth;
            
            if (map[x,y]==-1)
            {
                Image img = new Image();
                BitmapImage bi = new BitmapImage();
                bi.UriSource = new Uri(Canv.BaseUri, $"TicTacToe_{player}.png");
                img.Width = ceilWIdth;
                img.Height = ceilWIdth;
                img.Source = bi;
                img.Stretch = Stretch.Fill;
                img.Margin = new Thickness(x * ceilWIdth, y * ceilWIdth, 0, 0);
                Canv.Children.Add(img);
                map[x, y] = player == "cross" ? 1 : 0;
                player = player == "cross" ? "circle" : "cross";
                checkResult = Check();
                switch (checkResult)
                {
                    case 0:
                        displayText = "Circles won!";
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
            ceilWIdth = (int)Canv.Height / 3;
            Result.Text = "Start your game!";
        }
    }
}
