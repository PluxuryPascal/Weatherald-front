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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScottPlot;

namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для GraphPage.xaml
    /// </summary>
    public partial class GraphPage : Page
    {
        private Storyboard ShowMenuStoryboard;
        private Storyboard HideMenuStoryboard;
        public GraphPage()
        {
            InitializeComponent();
            GraphButton.IsEnabled = false;
            ShowMenuStoryboard = (Storyboard)FindResource("ShowMenuStoryboard");
            HideMenuStoryboard = (Storyboard)FindResource("HideMenuStoryboard");


        }

        private void VisualButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VisualPage());
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
