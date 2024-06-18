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

        public MainMenu_Weather()
        {
            InitializeComponent();
            MainButton.IsEnabled = false;

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