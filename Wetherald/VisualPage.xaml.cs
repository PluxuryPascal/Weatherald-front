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
            double[] dataY = { 3, 0, -4, 8, -11, 13, 9, -12 };
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
            double[] dataY = { 12, 21, 9, 15, 19, 20, 22, 23, 25, 21, 19, 19, 19, 10, 7, 3, 2, 1, -5 ,-7, -13, -32, 5, 4, 8, 7, 11, 15, 16, 23 };
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

            Tick[] ticks =
            {
                new(0, "1"),
                new(1, "2"),
                new(2, "3"),
                new(3, "4"),
                new(4, "5"),
                new(5, "6"),
                new(6, "7"),
                new(7, "8"),
                new(8, "9"),
                new(9, "10"),
                new(10, "11"),
                new(11, "12"),
                new(12, "13"),
                new(13, "14"),
                new(14, "15"),
                new(15, "16"),
                new(16, "17"),
                new(17, "18"),
                new(18, "19"),
                new(19, "20"),
                new(20, "21"),
                new(21, "22"),
                new(22, "23"),
                new(23, "24"),
                new(24, "25"),
                new(25, "26"),
                new(26, "27"),
                new(27, "28"),
                new(28, "29"),
                new(29, "30"),
                new(30, "31"),
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
