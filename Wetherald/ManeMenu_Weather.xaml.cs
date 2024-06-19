using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Wetherald
{
    public partial class MainMenu_Weather : Page
    {
        private Storyboard ShowMenuStoryboard;
        private Storyboard HideMenuStoryboard;
        public string MostCommon(List<string> strings)
        {
            int curIndex = 0;
            int curCountOfItems = 0;

            int tmpCount = 0;

            for (int i = 0; i < strings.Count; i++)
            {
                tmpCount = CountOfStrInList(ref strings, strings[i]);
                if (tmpCount > curIndex)
                {
                    curIndex = i;
                    curCountOfItems = tmpCount;
                }
            }
            return strings[curIndex];
        }
        public static int CountOfStrInList(ref List<string> list, string str)
        {
            return list.Where(item => item == str).Count();
        }
        public MainMenu_Weather()
        {
            InitializeComponent();
            string testing = "";
            MainButton.IsEnabled = false;
            foreach (double item in App.ProcessDataMonthTemperture(App.trust, App.WeatherSources,source => source.MaxTemperatureData))
            {
                testing += (item.ToString() + " ");
            }
            MaxTodayTemprature.Text = App.ProcessDataTodayTemperture(App.trust, App.WeatherSources, source => source.TemperatureData).Max().ToString() + " °C";
            MinTodayTemprature.Text = App.ProcessDataTodayTemperture(App.trust, App.WeatherSources, source => source.TemperatureData).Min().ToString() + " °C";
            MaxTomorrowTemprature.Text = App.ProcessDataTomorrowTemperture(App.trust, App.WeatherSources, source => source.TemperatureData).Max().ToString() + " °C";
            MinTomorrowTemprature.Text = App.ProcessDataTomorrowTemperture(App.trust, App.WeatherSources, source => source.TemperatureData).Min().ToString() + " °C";
            MaxTodayHumidity.Text = App.ProcessDataTodayHumidity(App.trust, App.WeatherSources, source => source.HumidityData).Max().ToString() + "%";
            MinTodayHumidity.Text = App.ProcessDataTodayHumidity(App.trust, App.WeatherSources, source => source.HumidityData).Min().ToString() + "%";
            MaxTomorrowHumidity.Text = App.ProcessDataTomorrowHumidity(App.trust, App.WeatherSources, source => source.HumidityData).Max().ToString() + "%";
            MinTomorrowHumidity.Text = App.ProcessDataTomorrowHumidity(App.trust, App.WeatherSources, source => source.HumidityData).Min().ToString() + "%";
            MaxTodayWinf.Text = App.ProcessDataTodayWind(App.trust, App.WeatherSources, source => source.WindData).Max().ToString();
            MinTodayWind.Text = App.ProcessDataTodayWind(App.trust, App.WeatherSources, source => source.WindData).Min().ToString();
            MaxTomorrowWinf.Text = App.ProcessDataTomorrowWind(App.trust, App.WeatherSources, source => source.WindData).Max().ToString();
            MinTomorrowWind.Text = App.ProcessDataTomorrowWind(App.trust, App.WeatherSources, source => source.WindData).Min().ToString();
            switch (MostCommon(App.ProcessDataTodayWindString(App.trust, App.WeatherSources, source => source.WindData)))
            {
                case ("Ю"):
                    {
                        MainDirectionTodayWind.Text = (string) Application.Current.Resources["S"];
                        break;
                    }
                case ("З"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["W"];
                        break;
                    }
                case ("В"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["E"];
                        break;
                    }
                case ("С"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["N"];
                        break;
                    }
                case ("ЮЗ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["SW"];
                        break;
                    }
                case ("ЮВ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["SE"];
                        break;
                    }
                case ("СЗ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["NW"];
                        break;
                    }
                case ("СВ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["NE"];
                        break;
                    }
                case ("ЮЮЗ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["SSW"];
                        break;
                    }
                case ("ЮЮВ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["SSE"];
                        break;
                    }
                case ("ССЗ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["NNW"];
                        break;
                    }
                case ("ССВ"):
                    {
                        MainDirectionTodayWind.Text = (string)Application.Current.Resources["NNE"];
                        break;
                    }
            }
            switch (MostCommon(App.ProcessDataTommorowWindString(App.trust, App.WeatherSources, source => source.WindData)))
            {
                case ("Ю"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["S"];
                        break;
                    }
                case ("З"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["W"];
                        break;
                    }
                case ("В"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["E"];
                        break;
                    }
                case ("С"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["N"];
                        break;
                    }
                case ("ЮЗ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["SW"];
                        break;
                    }
                case ("ЮВ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["SE"];
                        break;
                    }
                case ("СЗ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["NW"];
                        break;
                    }
                case ("СВ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["NE"];
                        break;
                    }
                case ("ЮЮЗ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["SSW"];
                        break;
                    }
                case ("ЮЮВ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["SSE"];
                        break;
                    }
                case ("ССЗ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["NNW"];
                        break;
                    }
                case ("ССВ"):
                    {
                        MainDirectionTomorrowWind.Text = (string)Application.Current.Resources["NNE"];
                        break;
                    }
            }

            MainDirectionTodayWind.Text = MostCommon(App.ProcessDataTodayWindString(App.trust, App.WeatherSources, source => source.WindData));

            ShowMenuStoryboard = (Storyboard)FindResource("ShowMenuStoryboard");
            HideMenuStoryboard = (Storyboard)FindResource("HideMenuStoryboard");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void VisualPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VisualPage());
        }

        private void GraphButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GraphPage());
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });
        }

        private void SlideMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowMenuStoryboard.Begin();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Images/SlidePanelRight.png", UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            SlidePan_Left.Source = bitmap;
            
        }

        private void SlideMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            HideMenuStoryboard.Begin();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Images/SlidePanelLeft.png", UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            SlidePan_Left.Source = bitmap;
        }
    }
}