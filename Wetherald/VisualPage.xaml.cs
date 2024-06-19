using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScottPlot.Plottable;
using System.Drawing;
using ScottPlot.Plottables;
using System.Windows.Media.Animation;
using ScottPlot.WPF;

namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для VisualPage.xaml
    /// </summary>
    public partial class VisualPage : Page
    {
        private Storyboard ShowMenuStoryboard;
        private Storyboard HideMenuStoryboard;
        public VisualPage()
        {
            InitializeComponent();
            DayWeatherGraph_Realisation();
            MonthWeatherGraph_Realisation();
            VisualButton.IsEnabled = false;
            ShowMenuStoryboard = (Storyboard)FindResource("ShowMenuStoryboard");
            HideMenuStoryboard = (Storyboard)FindResource("HideMenuStoryboard");
        }

        private void GraphButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GraphPage());
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu_Weather());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });
        }

        public void DayWeatherGraph_Realisation()
        {
            var plt = DayWeatherGraph.Plot;
            double[] dataY = App.ProcessDataTodayTemperture(App.trust, App.WeatherSources, source => source.TemperatureData);
            double max = dataY.Max();
            double min = dataY.Min();
            var barPlt = plt.Add.Bars(dataY);
            plt.Add.HorizontalLine(0, 1, ScottPlot.Color.FromColor(System.Drawing.Color.Black), LinePattern.Dashed);


            // visible items have public properties that can be customized
            plt.Axes.Bottom.Label.Text = (string)Application.Current.Resources["XAxisLabel"];
            plt.Axes.Left.Label.Text = (string)Application.Current.Resources["YAxisLabel"];
            plt.Axes.Title.Label.Text = (string)Application.Current.Resources["AxisName"];

            // Настройка прозрачного фона графика
            plt.FigureBackground.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Transparent);

            plt.Axes.Margins(bottom: 0, left: 0);

            ScottPlot.Color GetColorWarm(double value, double min, double max)
            {
                // Цвета для интерполяции
                ScottPlot.Color lowColor = ScottPlot.Color.FromColor(System.Drawing.Color.Orange);
                ScottPlot.Color highColor = ScottPlot.Color.FromColor(System.Drawing.Color.Red);

                // Вычисление промежуточных значений
                double ratio = (value - min) / (max - min);
                int r = (int)(lowColor.R + ratio * (highColor.R - lowColor.R));
                int g = (int)(lowColor.G + ratio * (highColor.G - lowColor.G));
                int b = (int)(lowColor.B + ratio * (highColor.B - lowColor.B));

                return ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(200, r, g, b));
            }
            ScottPlot.Color GetColorCold(double value, double min, double max)
            {
                // Цвета для интерполяции
                ScottPlot.Color lowColor = ScottPlot.Color.FromColor(System.Drawing.Color.Blue);
                ScottPlot.Color highColor = ScottPlot.Color.FromColor(System.Drawing.Color.LightBlue);

                // Вычисление промежуточных значений
                double ratio = (value - min) / (max - min);
                int r = (int)(lowColor.R + ratio * (highColor.R - lowColor.R));
                int g = (int)(lowColor.G + ratio * (highColor.G - lowColor.G));
                int b = (int)(lowColor.B + ratio * (highColor.B - lowColor.B));

                return ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(200, r, g, b));
            }
            // define the content of labels
            foreach (var bar in barPlt.Bars)
            {
                bar.Label = bar.Value.ToString();
                if (bar.Value > 0)
                {
                    bar.BorderColor = GetColorWarm(bar.Value, 0, max);
                    bar.FillColor = GetColorWarm(bar.Value, 0, max);
                }
                else
                {
                    if (bar.Value == 0)
                    {
                        bar.BorderLineWidth = 10;
                        bar.BorderColor = ScottPlot.Color.FromColor(System.Drawing.Color.LightBlue);
                    }
                    else
                    {
                        bar.BorderColor = GetColorCold(bar.Value, min, 0);
                        bar.FillColor = GetColorCold(bar.Value, min, 0) ;
                    }

                }
            }
            

            Tick[] ticks =
            {
                new(0, "2:00"),
                new(1, "5:00"),
                new(2, "8:00"),
                new(3, "11:00"),
                new(4, "14:00"),
                new(5, "17:00"),
                new(6, "20:00"),
                new(7, "23:00"),
            };


            // set limits that do not fit the data
            plt.Axes.SetLimits(-0.5, 7.5, dataY.Min() - 10, dataY.Max() + 10);
            plt.Axes.Frame(false);

            plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            plt.Axes.Bottom.MajorTickStyle.Length = 0;
            plt.HideGrid();

            // customize label style
            barPlt.ValueLabelStyle.Bold = true;
            barPlt.ValueLabelStyle.FontSize = 18;

            // Установите шрифт, поддерживающий китайский язык
            plt.Axes.Bottom.Label.FontName = "Microsoft YaHei";
            plt.Axes.Bottom.Label.FontSize = 12;
            plt.Axes.Left.Label.FontName = "Microsoft YaHei";
            plt.Axes.Left.Label.FontSize = 12;
            plt.Axes.Title.Label.FontName = "Microsoft YaHei";
            plt.Axes.Title.Label.FontSize = 14;

            DayWeatherGraph.Refresh();

        }

        public void MonthWeatherGraph_Realisation()
        {
            var plt = MonthWeatherGraph.Plot;
            double[] dataY = App.ProcessDataMonthTemperture(App.trust, App.WeatherSources, source => source.MaxTemperatureData);
            double max = dataY.Max();
            double min = dataY.Min();
            DateTime[] dates = Generate.ConsecutiveDays(100);

            var barPlt = plt.Add.Bars(dataY);
            plt.Add.HorizontalLine(0, 1, ScottPlot.Color.FromColor(System.Drawing.Color.Black), LinePattern.Dashed);


            // visible items have public properties that can be customized
            plt.Axes.Bottom.Label.Text = (string)Application.Current.Resources["Data"];
            plt.Axes.Left.Label.Text = (string)Application.Current.Resources["YAxisLabel"];
            plt.Axes.Title.Label.Text = (string)Application.Current.Resources["Name2"];


            // Настройка прозрачного фона графика
            plt.FigureBackground.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Transparent);

            plt.Axes.Margins(bottom: 0, left: 0);

            ScottPlot.Color GetColorWarm(double value, double min, double max)
            {
                // Цвета для интерполяции
                ScottPlot.Color lowColor = ScottPlot.Color.FromColor(System.Drawing.Color.Orange);
                ScottPlot.Color highColor = ScottPlot.Color.FromColor(System.Drawing.Color.Red);

                // Вычисление промежуточных значений
                double ratio = (value - min) / (max - min);
                int r = (int)(lowColor.R + ratio * (highColor.R - lowColor.R));
                int g = (int)(lowColor.G + ratio * (highColor.G - lowColor.G));
                int b = (int)(lowColor.B + ratio * (highColor.B - lowColor.B));

                return ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(200, r, g, b));
            }
            ScottPlot.Color GetColorCold(double value, double min, double max)
            {
                // Цвета для интерполяции
                ScottPlot.Color lowColor = ScottPlot.Color.FromColor(System.Drawing.Color.Blue);
                ScottPlot.Color highColor = ScottPlot.Color.FromColor(System.Drawing.Color.LightBlue);

                // Вычисление промежуточных значений
                double ratio = (value - min) / (max - min);
                int r = (int)(lowColor.R + ratio * (highColor.R - lowColor.R));
                int g = (int)(lowColor.G + ratio * (highColor.G - lowColor.G));
                int b = (int)(lowColor.B + ratio * (highColor.B - lowColor.B));

                return ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(200, r, g, b));
            }

            // define the content of labels
            foreach (var bar in barPlt.Bars)
            {
                bar.Label = bar.Value.ToString();
                if (bar.Value > 0)
                {
                    bar.BorderColor = GetColorWarm(bar.Value, 0, max);
                    bar.FillColor = GetColorWarm(bar.Value, 0, max);
                }
                else
                {
                    if (bar.Value == 0)
                    {
                        bar.BorderLineWidth = 10;
                        bar.BorderColor = ScottPlot.Color.FromColor(System.Drawing.Color.LightBlue);
                    }
                    else
                    {
                        bar.BorderColor = GetColorCold(bar.Value, min, 0);
                        bar.FillColor = GetColorCold(bar.Value, min, 0);
                    }

                }
            }
            DateTime today = DateTime.Today;

            Tick[] ticks =
            {
                new(0, (today.Day).ToString() + "." + (today.Month).ToString()),
                new(1, (today.AddDays(1).Day).ToString()  + "." + (today.AddDays(1).Month).ToString()),
                new(2, (today.AddDays(2).Day).ToString() + "." + (today.AddDays(2).Month).ToString()),
                new(3, (today.AddDays(3).Day).ToString() + "." + (today.AddDays(3).Month).ToString()),
                new(4, (today.AddDays(4).Day).ToString() + "." + (today.AddDays(4).Month).ToString()),
                new(5, (today.AddDays(5).Day).ToString() + "." +(today.AddDays(5).Month).ToString() ),
                new(6, (today.AddDays(6).Day).ToString() + "." +(today.AddDays(6).Month).ToString()),
                new(7, (today.AddDays(7).Day).ToString() + "." +(today.AddDays(7).Month).ToString()),
                new(8, (today.AddDays(8).Day).ToString() + "." +(today.AddDays(8).Month).ToString()),
                new(9, (today.AddDays(9).Day).ToString() + "." +(today.AddDays(9).Month).ToString()),
                new(10, (today.AddDays(10).Day).ToString() + "." +(today.AddDays(10).Month).ToString()),
                new(11, (today.AddDays(11).Day).ToString() + "." +(today.AddDays(11).Month).ToString()),
                new(12, (today.AddDays(12).Day).ToString() + "." +(today.AddDays(12).Month).ToString()),
                new(13, (today.AddDays(13).Day).ToString() + "." +(today.AddDays(13).Month).ToString()),
                new(14, (today.AddDays(14).Day).ToString() + "." +(today.AddDays(14).Month).ToString()),
                new(15, (today.AddDays(15).Day).ToString() + "." +(today.AddDays(15).Month).ToString()),
                new(16, (today.AddDays(16).Day).ToString() + "." +(today.AddDays(16).Month).ToString()),
                new(17, (today.AddDays(17).Day).ToString() + "." +(today.AddDays(17).Month).ToString()),
                new(18, (today.AddDays(18).Day).ToString() + "." +(today.AddDays(18).Month).ToString()),
                new(19, (today.AddDays(19).Day).ToString() + "." +(today.AddDays(19).Month).ToString()),
                new(20, (today.AddDays(20).Day).ToString() + "." + (today.AddDays(20).Month).ToString()),
                new(21, (today.AddDays(21).Day).ToString() + "." + (today.AddDays(21).Month).ToString()),
                new(22, (today.AddDays(22).Day).ToString() + "." + (today.AddDays(22).Month).ToString()),
                new(23, (today.AddDays(23).Day).ToString() + "." + (today.AddDays(23).Month).ToString()),
                new(24, (today.AddDays(24).Day).ToString() + "." +(today.AddDays(24).Month).ToString()),
                new(25, (today.AddDays(25).Day).ToString() + "." +(today.AddDays(25).Month).ToString()),
                new(26, (today.AddDays(26).Day).ToString() + "." +(today.AddDays(26).Month).ToString()),
                new(27, (today.AddDays(27).Day).ToString() + "." +(today.AddDays(27).Month).ToString()),
                new(28, (today.AddDays(28).Day).ToString() + "." +(today.AddDays(28).Month).ToString()),
                new(29, (today.AddDays(29).Day).ToString() + "." +(today.AddDays(29).Month).ToString()),
                new(30, (today.AddDays(30).Day).ToString() + "." +(today.AddDays(30).Month).ToString()),
            };


            // set limits that do not fit the data
            plt.Axes.SetLimits(-0.5, 30.5, dataY.Min() - 10, dataY.Max() + 20);
            plt.Axes.Frame(false);

            plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            plt.Axes.Bottom.MajorTickStyle.Length = 0;
            plt.HideGrid();

            // customize label style
            barPlt.ValueLabelStyle.Bold = true;
            barPlt.ValueLabelStyle.FontSize = 18;

            // Установите шрифт, поддерживающий китайский язык
            plt.Axes.Bottom.Label.FontName = "Microsoft YaHei";
            plt.Axes.Bottom.Label.FontSize = 12;
            plt.Axes.Bottom.Label.Alignment = Alignment.MiddleLeft;
            plt.Axes.Bottom.Label.Bold = true;
            plt.Axes.Left.Label.FontName = "Microsoft YaHei";
            plt.Axes.Left.Label.FontSize = 12;
            plt.Axes.Bottom.Label.Bold = true;
            plt.Axes.Title.Label.FontName = "Microsoft YaHei";
            plt.Axes.Title.Label.FontSize = 14;
            plt.Axes.Bottom.Label.Bold = true;



            MonthWeatherGraph.Refresh();

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
