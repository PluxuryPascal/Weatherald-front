using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.Threading;
using System.Windows.Media.Animation;
using System.IO;
namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private Storyboard ShowMenuStoryboard;
        private Storyboard HideMenuStoryboard;
        public SettingsPage()
        {
            InitializeComponent();
            App.ChangeLanguage(App.CurrentLanguage); // Установить начальный язык на текущий

            ShowMenuStoryboard = (Storyboard)FindResource("ShowMenuStoryboard");
            HideMenuStoryboard = (Storyboard)FindResource("HideMenuStoryboard");

            // Установить выбранный элемент ComboBox на основе текущего языка
            SetLanguageSelector(App.CurrentLanguage);
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu_Weather());
        }

        private void VisualPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VisualPage());
        }

        private void GraphButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GraphPage());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });
        }

        private void LanguageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageSelection.SelectedItem is ComboBoxItem selectedItem)
            {
                string culture = selectedItem.Tag.ToString();
                SetLanguage(culture);
            }
        }
        public void SetLanguage(string culture)
        {
            if (LanguageSelection.SelectedItem is ComboBoxItem selectedItem)
            {
                string culture1 = selectedItem.Tag.ToString();
                App.ChangeLanguage(culture1);
            }
        }
        private void SetLanguageSelector(string culture)
        {
            foreach (ComboBoxItem item in LanguageSelection.Items)
            {
                if (item.Tag.ToString() == culture)
                {
                    LanguageSelection.SelectedItem = item;
                    break;
                }
            }
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

        private void TrustChangeButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrustPage_Change());
        }

        private void CityChangeButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChangeTheCityPage());
        }
    }
}
